﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkinsSite.Models;
using SkinsSite.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkinsSite.ViewModels;
using SkinsSite.Context;

namespace SkinsSite.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly CarrinhoCompra _carrinhoCompra;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _context;

        public PedidoController(IPedidoRepository pedidoRepository, 
               CarrinhoCompra carrinhoCompra,
               UserManager<IdentityUser> userManager,
               AppDbContext context)
        {
            _pedidoRepository = pedidoRepository;
            _carrinhoCompra = carrinhoCompra;
            _userManager = userManager;
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Checkout(Pedido pedido)
        {
            int totalItensPedido = 0;
            decimal precoTotalPedido = 0.0m;

            //obtem os itens do carrinho de compra cliente
            List<CarrinhoCompraItem> items = _carrinhoCompra.GetCarrinhoCompraItens();
            _carrinhoCompra.CarrinhoCompraItems = items;

            //verifica se existem itens de pedido
            if(_carrinhoCompra.CarrinhoCompraItems.Count == 0)
            {
                ModelState.AddModelError("", "Seu carrinho está vazio, que tal incluir uma skin...");
            }

            //calcula o total de itens e o total do pedido
            foreach(var item in items)
            {
                totalItensPedido += item.Quantidade;
                precoTotalPedido += (item.Skin.Preco * item.Quantidade);
            }

            //atribui os valores obtidos ao pedido
            pedido.TotalItensPedido = totalItensPedido;
            pedido.PedidoTotal = precoTotalPedido;

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

                //exibe a view com dados do cliente e pedido
                return View("~/Views/Pedido/CheckoutCompleto.cshtml", pedido);
            }
            return View(pedido);
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
