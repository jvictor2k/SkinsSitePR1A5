using Microsoft.AspNetCore.Mvc;

namespace SkinsSite.Controllers
{
    public class InventarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
