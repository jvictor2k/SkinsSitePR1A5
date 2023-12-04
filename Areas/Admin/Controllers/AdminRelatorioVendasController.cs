using iText.Kernel.Pdf;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using SkinsSite.Areas.Admin.Services;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace SkinsSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminRelatorioVendasController : Controller
    {
        private readonly RelatorioVendasService relatorioVendasService;

        public AdminRelatorioVendasController(RelatorioVendasService _relatorioVendasService)
        {
            relatorioVendasService = _relatorioVendasService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult IndexComCupons()
        {
            return View();
        }

        public IActionResult IndexSkinsMaisVendidas()
        {
            return View();
        }

        public async Task<IActionResult> RelatorioVendasSimples(DateTime? minDate,
            DateTime? maxDate)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }
            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }

            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            var result = await relatorioVendasService.FindByDateAsync(minDate, maxDate);

            return View(result);
        }

        public async Task<IActionResult> RelatorioVendasComCupons(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }
            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }

            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            var result = await relatorioVendasService.FindByDateComCuponsAsync(minDate, maxDate);

            return View(result);
        }

        public async Task<IActionResult> RelatorioSkinsMaisVendidas(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }
            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }

            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            var pedidoDetalhes = await relatorioVendasService.FindPedidoDetalhesByDateAsync(minDate, maxDate);

            return View(pedidoDetalhes);
        }

        public async Task<IActionResult> ExportarRelatorioVendasSimples(DateTime? minDate, DateTime? maxDate)
        {
            var result = await relatorioVendasService.FindByDateAsync(minDate, maxDate);

            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new PdfWriter(memoryStream).SetSmartMode(true))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf);

                        document.Add(new Paragraph("Relatório de Vendas Simples"));

                        // Adicione outras informações ou formatação conforme necessário

                        // Adicione dados da tabela
                        var table = new Table(7); // Ajuste o número de colunas conforme sua tabela
                        table.AddHeaderCell("Pedido ID");
                        table.AddHeaderCell("Nome");
                        table.AddHeaderCell("Email");
                        table.AddHeaderCell("Telefone");
                        table.AddHeaderCell("Pedido Total");
                        table.AddHeaderCell("Total Itens Pedido");
                        table.AddHeaderCell("Pedido Entregue Em");

                        foreach (var item in result)
                        {
                            table.AddCell(item.PedidoId.ToString());
                            table.AddCell(item.Nome);
                            table.AddCell(item.Email);
                            table.AddCell(item.Telefone);
                            table.AddCell(item.PedidoTotal.ToString());
                            table.AddCell(item.TotalItensPedido.ToString());
                            table.AddCell(item.PedidoEntregueEm?.ToString("yyyy-MM-dd") ?? "-");
                        }

                        document.Add(table);
                    }
                }

                // Converta o MemoryStream para um array de bytes antes de retorná-lo
                var contentBytes = memoryStream.ToArray();

                return File(contentBytes, "application/pdf", "RelatorioVendasSimples.pdf");
            }
        }

        public async Task<IActionResult> ExportarRelatorioVendasComCupons(DateTime? minDate, DateTime? maxDate)
        {
            var result = await relatorioVendasService.FindByDateComCuponsAsync(minDate, maxDate);

            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new PdfWriter(memoryStream).SetSmartMode(true))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf);

                        document.Add(new Paragraph("Relatório de Pedidos Agrupados por Cupom"));

                        // Adicione outras informações ou formatação conforme necessário

                        // Filtrar apenas os pedidos que possuem cupons
                        var pedidosComCupons = result.Where(p => !string.IsNullOrEmpty(p.CuponsAplicados)).ToList();

                        // Adicione dados da tabela
                        var table = new Table(5); // Ajuste o número de colunas conforme sua tabela
                        table.AddHeaderCell("Cupons");
                        table.AddHeaderCell("Pedido ID");
                        table.AddHeaderCell("Data do Pedido");
                        table.AddHeaderCell("Valor Total");
                        table.AddHeaderCell("Desconto");

                        foreach (var pedido in pedidosComCupons)
                        {
                            table.AddCell(pedido.CuponsAplicados);
                            table.AddCell(pedido.PedidoId.ToString());
                            table.AddCell(pedido.PedidoEnviado.ToString("yyyy-MM-dd HH:mm"));
                            table.AddCell(pedido.PedidoTotal.ToString());
                            table.AddCell(pedido.DescontoTotal.ToString());
                        }

                        document.Add(table);
                    }
                }

                var contentBytes = memoryStream.ToArray();

                return File(contentBytes, "application/pdf", "RelatorioVendasComCupons.pdf");
            }
        }


        public async Task<IActionResult> ExportarRelatorioSkinsMaisVendidas(DateTime? minDate, DateTime? maxDate)
        {
            var pedidoDetalhes = await relatorioVendasService.FindPedidoDetalhesByDateAsync(minDate, maxDate);

            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new PdfWriter(memoryStream).SetSmartMode(true))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf);

                        document.Add(new Paragraph("Relatório de Skins Mais Vendidas"));

                        // Adicione outras informações ou formatação conforme necessário

                        // Adicione dados da tabela
                        var table = new Table(4); // Ajuste o número de colunas conforme sua tabela
                        table.AddHeaderCell("Skin ID");
                        table.AddHeaderCell("Descrição");
                        table.AddHeaderCell("Preço");
                        table.AddHeaderCell("Quantidade Vendida");

                        foreach (var skin in pedidoDetalhes.GroupBy(pd => pd.SkinId).Select(grp => new { SkinId = grp.Key, Vendas = grp.Sum(s => s.Quantidade) }))
                        {
                            var skinItem = pedidoDetalhes.First(pd => pd.SkinId == skin.SkinId).Skin;

                            table.AddCell(skin.SkinId.ToString());
                            table.AddCell(skinItem.DescricaoCurta);
                            table.AddCell(pedidoDetalhes.First(pd => pd.SkinId == skin.SkinId).Preco.ToString());
                            table.AddCell(skin.Vendas.ToString());
                        }

                        document.Add(table);
                    }
                }

                var contentBytes = memoryStream.ToArray();

                return File(contentBytes, "application/pdf", "RelatorioSkinsMaisVendidas.pdf");
            }
        }
    }
}
