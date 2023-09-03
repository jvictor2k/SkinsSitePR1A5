using SkinsSite.Models;

namespace SkinsSite.Repositories.Interfaces
{
    public interface IPedidoRepository
    {
        void CriarPedido(Pedido pedido);
        IEnumerable<Pedido> GetPedidosByUserId(string userId);
    }
}
