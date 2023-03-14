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
    }
}
