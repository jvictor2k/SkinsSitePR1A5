using SkinsSite.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SkinsSite.ViewModels;

namespace SkinsSite.Controllers
{
    public class SkinController : Controller
    {
        private readonly ISkinRepository _skinRepository;

        public SkinController(ISkinRepository skinRepository)
        {
            _skinRepository = skinRepository;
        }

        public IActionResult List()
        {
            //var skins = _skinRepository.Skins;
            //return View(skins);
            var skinsListViewModel = new SkinListViewModel();
            skinsListViewModel.Skins = _skinRepository.Skins;
            skinsListViewModel.CategoriaAtual = "Categoria Atual";

            return View(skinsListViewModel);
        }
    }
}
