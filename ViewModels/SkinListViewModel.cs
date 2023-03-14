using SkinsSite.Models;

namespace SkinsSite.ViewModels
{
    public class SkinListViewModel
    {
        public IEnumerable<Skin> Skins { get; set; }
        public string CategoriaAtual { get; set; }
    }
}
