using SkinsSite.Models;

namespace SkinsSite.Repositories.Interfaces
{
    public interface IInventarioRepository
    {
        public Inventario GetInventarioByUserId(string userId);
        Skin GetSkinDetails(int skinId);
    }
}
