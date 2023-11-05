using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkinsSite.Models;
using SkinsSite.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkinsSite.ViewModels;
using SkinsSite.Context;
using MercadoPago.Config;
using Microsoft.AspNetCore.Http.Features;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;

namespace SkinsSite.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly CarrinhoCompra _carrinhoCompra;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _context;
        private readonly ICupomRepository _cupomRepository;

        public PedidoController(IPedidoRepository pedidoRepository, 
               CarrinhoCompra carrinhoCompra,
               UserManager<IdentityUser> userManager,
               AppDbContext context,
               ICupomRepository cupomRepository)
        {
            _pedidoRepository = pedidoRepository;
            _carrinhoCompra = carrinhoCompra;
            _userManager = userManager;
            _context = context;
            _cupomRepository = cupomRepository;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Checkout(Pedido pedido)
        {

            int totalItensPedido = 0;
            decimal precoTotalPedido = 0.0m;
            decimal descontoTotal = 0.0m;

            //obtem os itens do carrinho de compra cliente
            List<CarrinhoCompraItem> items = _carrinhoCompra.GetCarrinhoCompraItens();
            _carrinhoCompra.CarrinhoCompraItems = items;

            List<string> cuponsAplicados = new List<string>();

            //verifica se existem itens de pedido
            if (_carrinhoCompra.CarrinhoCompraItems.Count == 0)
            {
                ModelState.AddModelError("", "Seu carrinho está vazio, que tal incluir uma skin...");
            }

            //calcula o total de itens e o total do pedido
            foreach(var item in items)
            {
                decimal descontoItem = 0.0m;

                if(item.DescontoPreco.HasValue)
                {
                    if (!cuponsAplicados.Contains(item.CupomUsado))
                    {
                        cuponsAplicados.Add(item.CupomUsado);

                        var cupom = _cupomRepository.ObterCupomPorCodigo(item.CupomUsado);

                        var usoCupom = new UsoCupom
                        {
                            CupomId = cupom.CupomId,
                            UserId = _userManager.GetUserId(User)
                        };

                        _context.UsosCupons.Add(usoCupom);
                        _context.SaveChanges();
                    }

                    descontoItem = item.Skin.Preco - item.DescontoPreco.Value;
                    descontoTotal += descontoItem;
                }

                totalItensPedido += item.Quantidade;
                precoTotalPedido += ((item.Skin.Preco - descontoItem) * item.Quantidade);
            }

            //atribui os valores obtidos ao pedido
            pedido.TotalItensPedido = totalItensPedido;
            pedido.PedidoTotal = precoTotalPedido;
            pedido.DescontoTotal = descontoTotal;
            pedido.CuponsAplicados = string.Join(",", cuponsAplicados);



            //valida os dados do pedido
            if (ModelState.IsValid)
            {
                // Define o ID do usuário
                pedido.UserId = _userManager.GetUserId(User);

                //cria o pedido e os detalhes
                _pedidoRepository.CriarPedido(pedido);

                //define mensangens ao cliente
                ViewBag.CheckoutCompletoMensagem = "Obrigado pelo seu pedido!";
                ViewBag.TotalPedido = _carrinhoCompra.GetCarrinhoCompraTotal();

                //limpa o carrinho do cliente
                _carrinhoCompra.LimparCarrinho();

                var response = await MercadoPagoCheckout(pedido);
                if (response != null)
                {
                    ViewBag.ResponseId = response.Id;
                }
                else
                {
                    ViewBag.ResponseId = 0;
                }

                //exibe a view com dados do cliente e pedido
                return View("~/Views/Pedido/CheckoutCompleto.cshtml", pedido);
            }


            return View(pedido);
        }

        public async Task<Preference> MercadoPagoCheckout(Pedido pedido)
        {
            MercadoPagoConfig.AccessToken = "TEST-5933096959985730-110215-ea3fc3965620f675592d7c7da87756e8-1185600433";

            var request = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
                {
                    new PreferenceItemRequest
                    {
                        Title = "Skins",
                        Quantity = 1,
                        CurrencyId = "BRL",
                        UnitPrice = pedido.PedidoTotal,
                    },
                },
            };

            var client = new PreferenceClient();
            Preference preference = await client.CreateAsync(request);

            return preference;
        }

        [Authorize]
        public IActionResult HistoricoPedidos()
        {
            string userId = _userManager.GetUserId(User);
            var pedidos = _pedidoRepository.GetPedidosByUserId(userId);

            return View(pedidos);
        }

        public IActionResult VerDetalhes(int? id)
        {
            var pedido = _context.Pedidos
                         .Include(pd => pd.PedidoItens)
                         .ThenInclude(s => s.Skin)
                         .FirstOrDefault(p => p.PedidoId == id);

            if (pedido == null)
            {
                Response.StatusCode = 404;
                return View("PedidoNotFound", id.Value);
            }

            PedidoSkinViewModel pedidoSkins = new PedidoSkinViewModel()
            {
                Pedido = pedido,
                PedidoDetalhes = pedido.PedidoItens
            };

            return View(pedidoSkins);
        }
    }
}
