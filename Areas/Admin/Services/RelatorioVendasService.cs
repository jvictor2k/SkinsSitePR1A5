using Microsoft.EntityFrameworkCore;
using SkinsSite.Context;
using SkinsSite.Models;

namespace SkinsSite.Areas.Admin.Services
{
    public class RelatorioVendasService
    {
        private readonly AppDbContext context;
        public RelatorioVendasService(AppDbContext _context)
        {
            context = _context;
        }

        public async Task<List<Pedido>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var resultado = from obj in context.Pedidos select obj;

            if (minDate.HasValue)
            {
                resultado = resultado.Where(x => x.PedidoEnviado >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                resultado = resultado.Where(x => x.PedidoEnviado <= maxDate.Value);
            }

            return await resultado
                         .Include(s => s.PedidoItens)
                         .ThenInclude(s => s.Skin)
                         .OrderByDescending(x => x.PedidoEnviado)
                         .ToListAsync();
        }

        public async Task<List<Pedido>> FindByDateComCuponsAsync(DateTime? minDate, DateTime? maxDate)
        {
            var resultado = from obj in context.Pedidos where obj.CuponsAplicados != null select obj;

            if (minDate.HasValue)
            {
                resultado = resultado.Where(x => x.PedidoEnviado >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                resultado = resultado.Where(x => x.PedidoEnviado <= maxDate.Value);
            }

            return await resultado
                         .Include(s => s.PedidoItens)
                         .ThenInclude(s => s.Skin)
                         .OrderBy(x => x.CuponsAplicados)
                         .ThenByDescending(x => x.PedidoEnviado)
                         .ToListAsync();
        }

        public async Task<List<PedidoDetalhe>> FindPedidoDetalhesByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            IQueryable<Pedido> pedidosNoIntervalo = context.Pedidos;

            if (minDate.HasValue)
            {
                pedidosNoIntervalo = pedidosNoIntervalo.Where(p => p.PedidoEnviado >= minDate);
            }

            if (maxDate.HasValue)
            {
                pedidosNoIntervalo = pedidosNoIntervalo.Where(p => p.PedidoEnviado <= maxDate);
            }

            var pedidosNoIntervaloIds = await pedidosNoIntervalo.Select(p => p.PedidoId).ToListAsync();

            var pedidoDetalhesNoIntervalo = context.PedidoDetalhes
                .Where(pd => pedidosNoIntervaloIds.Contains(pd.PedidoId))
                .Include(pd => pd.Skin);

            return await pedidoDetalhesNoIntervalo.ToListAsync();
        }
    }
}
