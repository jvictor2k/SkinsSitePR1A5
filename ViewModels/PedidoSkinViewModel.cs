using SkinsSite.Models;

namespace SkinsSite.ViewModels
{
    public class PedidoSkinViewModel
    {
        public Pedido Pedido { get; set; }
        public IEnumerable<PedidoDetalhe> PedidoDetalhes { get; set; }
    }
}
