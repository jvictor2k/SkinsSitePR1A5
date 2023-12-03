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
    public class InventarioController : Controller
    {
        private readonly IInventarioRepository _inventarioRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _appDbContext;

        public InventarioController(IInventarioRepository inventarioRepository,
                       UserManager<IdentityUser> userManager,
                       AppDbContext appDbContext)
        {
            _inventarioRepository = inventarioRepository;
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            string userId = _userManager.GetUserId(User);
            var inventario = _inventarioRepository.GetInventarioByUserId(userId);

            return View(inventario);
        }

        [HttpPost]
        public IActionResult EnviarSolicitacoes(int[] itensSelecionados, string novoTradeLink)
        {
            if (itensSelecionados != null && itensSelecionados.Length > 0)
            {
                foreach (var itemId in itensSelecionados)
                {
                    var inventarioItem = _appDbContext.InventarioItens.FirstOrDefault(item => item.InventarioItemId == itemId);

                    if (inventarioItem != null)
                    {
                        inventarioItem.Solicitado = true;
                        if(novoTradeLink != null)
                        {
                            inventarioItem.TradeLink = novoTradeLink;
                        }
                    }
                }

                _appDbContext.SaveChanges();

                TempData["MensagemSucesso"] = "Solicitações enviadas com sucesso!";
            }

            return RedirectToAction("Index");
        }
    }
}
