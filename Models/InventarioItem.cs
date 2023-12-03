namespace SkinsSite.Models
{
    public class InventarioItem
    {
        public int InventarioItemId { get; set; }
        public int PedidoDetalheId { get; set; }
        public int SkinId { get; set; }
        public decimal Preco { get; set; }
        public decimal? Desconto { get; set; }
        public bool Solicitado { get; set; } = false;
        public string TradeLink { get; set; }
        public DateTime? ItemEntregueEm { get; set; }
        public virtual Skin Skin { get; set; }
        public virtual PedidoDetalhe PedidoDetalhe { get; set; }
    }
}
