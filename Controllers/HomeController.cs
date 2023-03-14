using Microsoft.AspNetCore.Mvc;
using SkinsSite.Models;
using SkinsSite.Repositories.Interfaces;
using SkinsSite.ViewModels;
using System.Diagnostics;

namespace SkinsSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISkinRepository _skinRepository;

        public HomeController(ISkinRepository skinRepository)
        {
            _skinRepository = skinRepository;
        }

        public IActionResult Index()
        {
            var homeViewModel = new HomeViewModel
            {
                SkinsPreferidas = _skinRepository.SkinsPreferidas
            };
            return View(homeViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}