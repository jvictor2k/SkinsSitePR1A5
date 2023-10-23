using Microsoft.AspNetCore.Authorization;
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
        private readonly AppDbContext _context;

        public CarrinhoCompraController(ISkinRepository skinRepository, CarrinhoCompra carrinhoCompra, ICupomRepository cupomRepository, AppDbContext context)
        {
            _skinRepository = skinRepository;
            _carrinhoCompra = carrinhoCompra;
            _cupomRepository = cupomRepository;
            _context = context;
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

        [Authorize]
        public IActionResult AdicionarItemNoCarrinhoCompra(int skinId)
        {
            var skinSelecionado = _skinRepository.Skins.FirstOrDefault(p => p.SkinId == skinId);
            if (skinSelecionado != null)
            {
                _carrinhoCompra.AdicionarAoCarrinho(skinSelecionado);
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

                            descontoTotal += descontoItem;
                        }
                        else
                        {
                            item.DescontoPreco = null;
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
