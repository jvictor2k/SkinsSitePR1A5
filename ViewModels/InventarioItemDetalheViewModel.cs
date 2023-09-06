using SkinsSite.Models;

namespace SkinsSite.ViewModels
{
    public class InventarioItemDetalheViewModel
    {
        public Skin Skin { get; set; }
        public PedidoDetalhe PedidoDetalhe { get; set; }
        public InventarioItem InventarioItem { get; set; }
    }
}
