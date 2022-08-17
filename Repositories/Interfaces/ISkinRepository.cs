using SkinsSite.Models;

namespace SkinsSite.Repositories.Interfaces
{
    public interface ISkinRepository
    {
        IEnumerable<Skin> Skins { get; }
        IEnumerable<Skin> SkinsPreferidas { get; }
        Skin GetSkinId(int skinId);
    }
}
