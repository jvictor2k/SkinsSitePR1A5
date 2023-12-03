using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using SkinsSite.Context;
using SkinsSite.Models;
using SkinsSite.ViewModels;
using System.Data;

namespace SkinsSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminInventarioItens : Controller
    {
        private readonly AppDbContext _AppDbContext;

        public AdminInventarioItens (AppDbContext appDbContext)
        {
            _AppDbContext = appDbContext;
        }

        public InventarioItemDetalheViewModel InventarioItemDetalhes(int? id)
        {
            var inventarioItem = _AppDbContext.InventarioItens
                               .FirstOrDefault(i => i.InventarioItemId == id);

            if (inventarioItem == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            InventarioItemDetalheViewModel itemInventarioDetalhes = new InventarioItemDetalheViewModel()
            {
                Skin = inventarioItem.Skin,
                PedidoDetalhe = inventarioItem.PedidoDetalhe,
                InventarioItem = inventarioItem
            };

            return itemInventarioDetalhes;
        }

        public IActionResult Index()
        {
            var inventarioItens = _AppDbContext.InventarioItens
                                  .Include(ii => ii.Skin)
                                  .ToList();

            var viewModelList = new List<InventarioItemDetalheViewModel>();

            foreach (var inventarioItem in inventarioItens)
            {
                var pedidoDetalhe = _AppDbContext.PedidoDetalhes
                             .Include(pd => pd.Pedido)
                             .FirstOrDefault(pd => pd.PedidoDetalheId == inventarioItem.PedidoDetalheId);

                var viewModel = new InventarioItemDetalheViewModel()
                {
                    InventarioItem = inventarioItem,
                    Skin = inventarioItem.Skin,
                    PedidoDetalhe = pedidoDetalhe,
                    Pedido = pedidoDetalhe.Pedido
                };
                viewModelList.Add(viewModel);
            }

            viewModelList = viewModelList.OrderByDescending(item => item.InventarioItem.Solicitado)
                                         .ThenBy(item => item.InventarioItem.ItemEntregueEm.HasValue)
                                         .ToList();

            return View(viewModelList);
        }

        [HttpPost]
        public IActionResult ConfirmarEnvios(int[] itensSelecionados)
        {
            if (itensSelecionados != null && itensSelecionados.Length > 0)
            {
                foreach (var itemId in itensSelecionados)
                {
                    var inventarioItem = _AppDbContext.InventarioItens.FirstOrDefault(item => item.InventarioItemId == itemId);

                    if (inventarioItem != null)
                    {
                        inventarioItem.ItemEntregueEm = DateTime.Now;

                        _AppDbContext.SaveChanges();

                        // Verificar se todos os outros itens relacionados ao mesmo PedidoDetalhe foram entregues
                        var todosItensEntregues = _AppDbContext.InventarioItens
                            .Where(item => item.PedidoDetalheId == inventarioItem.PedidoDetalheId)
                            .All(item => item.ItemEntregueEm != null);

                        if (todosItensEntregues)
                        {
                            // Se todos os itens foram entregues, atualizar o PedidoDetalheEntregueEm
                            var pedidoDetalhe = _AppDbContext.PedidoDetalhes
                                .FirstOrDefault(pd => pd.PedidoDetalheId == inventarioItem.PedidoDetalheId);

                            if (pedidoDetalhe != null)
                            {
                                pedidoDetalhe.PedidoDetalheEntregueEm = DateTime.Now;

                                _AppDbContext.SaveChanges();

                                // Verificar se todos os outros PedidoDetalhe relacionados ao mesmo PedidoId foram entregues
                                var todosDetalhesEntregues = _AppDbContext.PedidoDetalhes
                                    .Where(pd => pd.PedidoId == pedidoDetalhe.PedidoId)
                                    .All(pd => pd.PedidoDetalheEntregueEm != null);

                                if (todosDetalhesEntregues)
                                {
                                    // Se todos os detalhes foram entregues, atualizar o PedidoEntregueEm
                                    var pedido = _AppDbContext.Pedidos
                                        .FirstOrDefault(p => p.PedidoId == pedidoDetalhe.PedidoId);

                                    if (pedido != null)
                                    {
                                        pedido.PedidoEntregueEm = DateTime.Now;

                                        _AppDbContext.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                }

                _AppDbContext.SaveChanges();

                TempData["MensagemSucesso"] = "Confirmações de Entrega Feitas com Sucesso!";
            }

            return RedirectToAction("Index");
        }

    }
}
