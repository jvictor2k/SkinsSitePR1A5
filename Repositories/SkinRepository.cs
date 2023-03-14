using Microsoft.EntityFrameworkCore;
using SkinsSite.Context;
using SkinsSite.Models;
using SkinsSite.Repositories.Interfaces;

namespace SkinsSite.Repositories
{
    public class SkinRepository : ISkinRepository
    {
        private readonly AppDbContext _context;
        public SkinRepository(AppDbContext contexto)
        {
            _context = contexto;
        }
        public IEnumerable<Skin> Skins => _context.Skins.Include(c => c.Categoria);

        public IEnumerable<Skin> SkinsPreferidas => _context.Skins
                                                   .Where(s => s.IsSkinPreferida)
                                                   .Include(c => c.Categoria);

        public Skin GetSkinId(int skinId)
        {
            return _context.Skins.FirstOrDefault(s => s.SkinId == skinId);
        }
    }
}
