using SkinsSite.Models;

namespace SkinsSite.ViewModels
{
    public class DetailsViewModel
    {
        public Skin SelectedSkin { get; set; }
        public IEnumerable<Skin> AdditionalSkins { get; set; }
    }
}
