using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkinsSite.Context;
using SkinsSite.Models;
using SkinsSite.Repositories.Interfaces;
using SkinsSite.ViewModels;

namespace SkinsSite.Controllers
{
    public class CarrinhoCompraController : Controller
    {
        private readonly ISkinRepository _skinRepository;
        private readonly CarrinhoCompra _carrinhoCompra;
        private readonly ICupomRepository _cupomRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _context;

        public CarrinhoCompraController(ISkinRepository skinRepository, CarrinhoCompra carrinhoCompra, ICupomRepository cupomRepository, AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _skinRepository = skinRepository;
            _carrinhoCompra = carrinhoCompra;
            _cupomRepository = cupomRepository;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var itens = _carrinhoCompra.GetCarrinhoCompraItens();
            _carrinhoCompra.CarrinhoCompraItems = itens;

            var carrinhoCompraVM = new CarrinhoCompraViewModel
            {
                CarrinhoCompra = _carrinhoCompra,
                CarrinhoCompraTotal = _carrinhoCompra.GetCarrinhoCompraTotal()
            };

            return View(carrinhoCompraVM);
        }

        public IActionResult AdicionarItemNoCarrinhoCompra(int skinId, int quantidade)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Se não estiver autenticado, redireciona para a tela de login
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("AdicionarItemNoCarrinhoCompra", "CarrinhoCompra", new { skinId, quantidade }) });
            }

            var skinSelecionado = _skinRepository.Skins.FirstOrDefault(p => p.SkinId == skinId);
            if (skinSelecionado != null)
            {
                var adicaoComSucesso = _carrinhoCompra.AdicionarAoCarrinho(skinSelecionado, quantidade);
                if (adicaoComSucesso)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    string urlReferencia = Request.Headers["Referer"].ToString();
                    TempData["Erro"] = "Impossível adicionar mais deste item ao carrinho.";
                    return Redirect(urlReferencia);
                }
            }
            return RedirectToAction("Index");
        }


        [Authorize]
        public IActionResult RemoverItemDoCarrinhoCompra(int skinId)
        {
            var skinSelecionado = _skinRepository.Skins.FirstOrDefault(p => p.SkinId == skinId);
            if (skinSelecionado != null)
            {
                _carrinhoCompra.RemoverDoCarrinho(skinSelecionado);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public IActionResult AplicarCupom(string cupomCodigo)
        {
            var cupom = _cupomRepository.ObterCupomPorCodigo(cupomCodigo);

            if (cupom == null || cupom.Ativo == false)
            {
                ModelState.AddModelError("", "Cupom inválido.");
                return RedirectToAction("Index");
            }
            else
            {
                string userId = _userManager.GetUserId(User);

                if (!_cupomRepository.VerificaLimiteCupom(cupom, userId))
                {
                    string urlReferencia = Request.Headers["Referer"].ToString();
                    TempData["Erro"] = "Impossível utilizar o cupom informado, você atingiu o limite de usos.";
                    return Redirect(urlReferencia);
                }

                if (_carrinhoCompra.CategoriasCuponsAplicados.Contains(cupom.CategoriaId))
                {
                    ModelState.AddModelError("", "Você já aplicou um cupom desta categoria no carrinho.");
                    return RedirectToAction("Index");
                }
                else
                {
                    var itensNoCarrinho = _carrinhoCompra.GetCarrinhoCompraItens();

                    decimal descontoTotal = _carrinhoCompra.DescontoTotal;

                    if (itensNoCarrinho == null)
                    {
                        ModelState.AddModelError("", "Não há itens no carrinho.");
                        return RedirectToAction("Index");
                    }

                    foreach (var item in itensNoCarrinho)
                    {
                        if (item.Skin.CategoriaId == cupom.CategoriaId)
                        {
                            decimal descontoItem = item.Skin.Preco * (cupom.ValorDesconto / 100);

                            item.DescontoPreco = item.Skin.Preco - descontoItem;

                            item.CupomUsado = cupomCodigo;

                            descontoTotal += descontoItem;
                        }
                    }

                    _carrinhoCompra.DescontoTotal = descontoTotal;

                    _carrinhoCompra.CuponsAplicados.Add(cupomCodigo);

                    _carrinhoCompra.CategoriasCuponsAplicados.Add(cupom.CategoriaId);

                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
        }
    }
}
