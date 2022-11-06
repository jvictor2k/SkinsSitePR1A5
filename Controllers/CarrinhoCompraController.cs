using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkinsSite.Models;
using SkinsSite.Repositories.Interfaces;
using SkinsSite.ViewModels;

namespace SkinsSite.Controllers
{
    public class CarrinhoCompraController : Controller
    {
        private readonly ISkinRepository _skinRepository;
        private readonly CarrinhoCompra _carrinhoCompra;

        public CarrinhoCompraController(ISkinRepository skinRepository, CarrinhoCompra carrinhoCompra)
        {
            _skinRepository = skinRepository;
            _carrinhoCompra = carrinhoCompra;
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
    }
}
