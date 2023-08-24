using Microsoft.AspNetCore.Mvc;
using SkinsSite.Repositories.Interfaces;

namespace SkinsSite.Components
{
    public class CategoriaGroup : ViewComponent
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaGroup(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public IViewComponentResult Invoke()
        {
            var categorias = _categoriaRepository.Categorias.OrderBy(c => c.CategoriaNome);
            return View(categorias);
        }
    }
}
