namespace SkinsSite.Models
{
    public class Inventario
    {
        public int InventarioId { get; set; }
        public string UserId { get; set; }
        public int TotalItensInventario { get; set; }
        public List<InventarioItem> InventarioItems { get; set; }
    }
}
