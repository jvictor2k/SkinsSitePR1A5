using SkinsSite.Repositories.Interfaces;
using SkinsSite.Models;
using SkinsSite.Context;
using Microsoft.EntityFrameworkCore;

namespace SkinsSite.Repositories
{
    public class InventarioRepository : IInventarioRepository
    {
        private readonly AppDbContext _appDbContext;

        public InventarioRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Inventario GetInventarioByUserId(string userId)
        {
            var inventario = _appDbContext.Inventarios
                .Include(i => i.InventarioItems)
                .FirstOrDefault(i => i.UserId == userId);
            return inventario;
        }

        public Skin GetSkinDetails(int skinId)
        {
            var skin = _appDbContext.Skins.FirstOrDefault(s => s.SkinId == skinId);
            return skin;
        }
    }
}
