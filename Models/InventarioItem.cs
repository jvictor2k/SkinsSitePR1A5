namespace SkinsSite.Models
{
    public class InventarioItem
    {
        public int InventarioItemId { get; set; }
        public int PedidoDetalheId { get; set; }
        public int SkinId { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
        public bool Solicitado { get; set; } = false;
        public virtual Skin Skin { get; set; }
        public virtual PedidoDetalhe PedidoDetalhe { get; set; }
    }
}
