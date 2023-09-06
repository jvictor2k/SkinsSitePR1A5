﻿using Microsoft.AspNetCore.Authorization;
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
                                  .Include(ii => ii.PedidoDetalhe)
                                  .ToList();
            var viewModelList = new List<InventarioItemDetalheViewModel>();

            foreach (var inventarioItem in inventarioItens)
            {
                var viewModel = InventarioItemDetalhes(inventarioItem.InventarioItemId);
                viewModelList.Add(viewModel);
            }

            return View(viewModelList);
        }
    }
}
