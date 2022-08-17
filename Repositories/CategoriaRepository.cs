using SkinsSite.Context;
using SkinsSite.Models;
using SkinsSite.Repositories.Interfaces;

namespace SkinsSite.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Categoria> Categorias => _context.Categorias;
    }
}
