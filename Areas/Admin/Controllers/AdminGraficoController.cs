using Microsoft.AspNetCore.Mvc;
using SkinsSite.Areas.Admin.Services;

namespace SkinsSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminGraficoController : Controller
    {
        private readonly GraficoVendasService _graficoVendas;

        public AdminGraficoController(GraficoVendasService graficoVendas)
        {
            _graficoVendas = graficoVendas ?? throw
                new ArgumentNullException(nameof(graficoVendas));
        }

        public JsonResult VendasSkins(int dias)
        {
            var skinsVendasTotais = _graficoVendas.GetVendasSkins(dias);
            return Json(skinsVendasTotais);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult VendasMensal()
        {
            return View();
        }

        [HttpGet]
        public IActionResult VendasSemanal()
        {
            return View();
        }
    }
}
