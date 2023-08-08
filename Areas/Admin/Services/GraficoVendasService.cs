using SkinsSite.Context;
using SkinsSite.Models;

namespace SkinsSite.Areas.Admin.Services
{
    public class GraficoVendasService
    {
        private readonly AppDbContext context;

        public GraficoVendasService(AppDbContext context)
        {
            this.context = context;
        }

        public List<SkinGrafico> GetVendasSkins(int dias = 360)
        {
            var data = DateTime.Now.AddDays(-dias);

            var skins = (from pd in context.PedidoDetalhes
                         join s in context.Skins on pd.SkinId equals s.SkinId
                         where pd.Pedido.PedidoEnviado >= data
                         group pd by new { pd.SkinId, s.Nome }
                         into g
                         select new
                         {
                             SkinNome = g.Key.Nome,
                             SkinsQuantidade = g.Sum(q => q.Quantidade),
                             SkinsValorTotal = g.Sum(a => a.Preco * a.Quantidade)
                         });
            var lista = new List<SkinGrafico>();

            foreach(var item in skins)
            {
                var skin = new SkinGrafico();
                skin.SkinNome = item.SkinNome;
                skin.SkinsQuantidade = item.SkinsQuantidade;
                skin.SkinsValorTotal = item.SkinsValorTotal;
                lista.Add(skin);
            }

            return lista;
        }
    }
}
