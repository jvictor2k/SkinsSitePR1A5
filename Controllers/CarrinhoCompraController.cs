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

            if (cupom == null)
            {
                ModelState.AddModelError("", "Cupom inválido.");
                return RedirectToAction("Index");
            }
            else
            {
                var itensNoCarrinho = _carrinhoCompra.GetCarrinhoCompraItens();

                decimal descontoTotal = 0;

                foreach(var item in itensNoCarrinho)
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

                _context.SaveChanges();

                return RedirectToAction("Index");
            }
        }
    }
}
