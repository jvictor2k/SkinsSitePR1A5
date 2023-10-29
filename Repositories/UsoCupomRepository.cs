using SkinsSite.Context;
using SkinsSite.Models;
using SkinsSite.Repositories.Interfaces;

namespace SkinsSite.Repositories
{
    public class UsoCupomRepository : IUsoCupomRepository
    {
        private readonly AppDbContext _context;

        public UsoCupomRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<UsoCupom> UsosCupons => _context.UsosCupons;
    }
}
