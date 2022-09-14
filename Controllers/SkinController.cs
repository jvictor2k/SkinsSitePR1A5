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
                skins = _skinRepository.Skins
                    .Where(s => s.Categoria.CategoriaNome.Equals(categoria))
                    .OrderBy(s => s.Nome);

                categoriaAtual = categoria;
            }

            var skinsListViewModel = new SkinListViewModel
            {
                Skins = skins,
                CategoriaAtual = categoriaAtual
            };

            return View(skinsListViewModel);
        }

        public IActionResult Details(int skinId)
        {
            var skin = _skinRepository.Skins.FirstOrDefault(s => s.SkinId == skinId);
            return View(skin);
        }

        public ViewResult Search(string searchString)
        {
            IEnumerable<Skin> skins;
            string categoriaAtual = string.Empty;

            if (string.IsNullOrEmpty(searchString))
            {
                skins = _skinRepository.Skins.OrderBy(p => p.SkinId);
                categoriaAtual = "Todas as Skins";
            }
            else
            {
                skins = _skinRepository.Skins
                        .Where(p => p.Nome.ToLower().Contains(searchString.ToLower()));

                if (skins.Any())
                {
                    categoriaAtual = "Skins";
                }
                else if (skins.Any() == false)
                {
                    skins = _skinRepository.Skins
                        .Where(p => p.Tipo.ToLower().Contains(searchString.ToLower()));

                    if (skins.Any())
                    {
                        categoriaAtual = "Tipo";
                    }
                    else
                    {
                        categoriaAtual = "Nenhuma skin foi encontrada";
                    }
                }
            }

            return View("~/Views/Skin/List.cshtml", new SkinListViewModel
            {
                Skins = skins,
                CategoriaAtual = categoriaAtual
            });
        }
    }
}
