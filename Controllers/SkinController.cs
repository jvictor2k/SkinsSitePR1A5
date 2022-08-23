using SkinsSite.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SkinsSite.ViewModels;
using SkinsSite.Models;

namespace SkinsSite.Controllers
{
    public class SkinController : Controller
    {
        private readonly ISkinRepository _skinRepository;

        public SkinController(ISkinRepository skinRepository)
        {
            _skinRepository = skinRepository;
        }

        public IActionResult List(string categoria)
        {
            IEnumerable<Skin> skins;
            string categoriaAtual = string.Empty;

            if(string.IsNullOrEmpty(categoria))
            {
                skins = _skinRepository.Skins.OrderBy(s => s.SkinId);
                categoriaAtual = "Todas as skins";
            }
            else
            {
                if(string.Equals("Facas", categoria, StringComparison.OrdinalIgnoreCase))
                {
                    skins = _skinRepository.Skins
                        .Where(s => s.Categoria.CategoriaNome.Equals("Facas"))
                        .OrderBy(s => s.Nome);
                }
                else
                {
                    skins = _skinRepository.Skins
                        .Where(s => s.Categoria.CategoriaNome.Equals("Luvas"))
                        .OrderBy(s => s.Nome);
                }
                categoriaAtual = categoria;
            }

            var skinsListViewModel = new SkinListViewModel
            {
                Skins = skins,
                CategoriaAtual = categoriaAtual
            };

            return View(skinsListViewModel);
        }
    }
}
