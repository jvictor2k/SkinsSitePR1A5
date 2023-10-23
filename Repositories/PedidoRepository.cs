using SkinsSite.Repositories.Interfaces;
using SkinsSite.Models;
using SkinsSite.Context;
using Microsoft.AspNetCore.Identity;

namespace SkinsSite.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly CarrinhoCompra _carrinhoCompra;

        public PedidoRepository(AppDbContext appDbContext, 
            CarrinhoCompra carrinhoCompra)
        {
            _appDbContext = appDbContext;
            _carrinhoCompra = carrinhoCompra;
        }

        public void CriarPedido(Pedido pedido)
        {
            pedido.PedidoEnviado = DateTime.Now;
            //Define o userId com base no usuário do pedido
            string userId = pedido.UserId;

            //Adiciona o pedido ao contexto
            _appDbContext.Pedidos.Add(pedido);
            _appDbContext.SaveChanges();

            var carrinhoCompraItens = _carrinhoCompra.CarrinhoCompraItems;
            
            foreach (var carrinhoItem in carrinhoCompraItens)
            {
                decimal descontoItem = carrinhoItem.Skin.Preco - (carrinhoItem.DescontoPreco ?? carrinhoItem.Skin.Preco);

                var pedidoDetail = new PedidoDetalhe()
                {
                    Quantidade = carrinhoItem.Quantidade,
                    SkinId = carrinhoItem.Skin.SkinId,
                    PedidoId = pedido.PedidoId,
                    Preco = carrinhoItem.DescontoPreco ?? carrinhoItem.Skin.Preco,
                    Desconto = descontoItem
                };
                _appDbContext.PedidoDetalhes.Add(pedidoDetail);
                _appDbContext.SaveChanges();
                var pedidoDetailId = pedidoDetail.PedidoDetalheId;

                var inventarioItem = new InventarioItem()
                {
                    Quantidade = carrinhoItem.Quantidade,
                    SkinId = carrinhoItem.Skin.SkinId,
                    Preco = carrinhoItem.DescontoPreco ?? carrinhoItem.Skin.Preco,
                    Desconto = descontoItem,
                    PedidoDetalheId = pedidoDetailId
                };

                //Encontra ou cria um inventário para o usuário com base no userId
                var inventario = _appDbContext.Inventarios
                    .FirstOrDefault(i => i.UserId == userId);

                if (inventario == null)
                {
                    inventario = new Inventario()
                    {
                        UserId = userId, 
                        InventarioItems = new List<InventarioItem>()
                    };
                }
                else if (inventario.InventarioItems == null)
                {
                    inventario.InventarioItems = new List<InventarioItem>();
                }

                inventario.InventarioItems.Add(inventarioItem);

                //Atualize o total de itens no inventário
                inventario.TotalItensInventario += inventarioItem.Quantidade;

                //Atualize ou adicione o inventário ao contexto
                _appDbContext.Inventarios.Update(inventario);
            }
            _appDbContext.SaveChanges();
        }

        public IEnumerable<Pedido> GetPedidosByUserId(string userId)
        {
            return _appDbContext.Pedidos
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.PedidoEnviado)
                .ToList();
        }
    }
}
