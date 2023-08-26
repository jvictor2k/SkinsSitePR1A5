using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using SkinsSite.Context;
using SkinsSite.Models;

namespace SkinsSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminSkinsController : Controller
    {
        private readonly AppDbContext _context;

        public AdminSkinsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminSkins
        //public async Task<IActionResult> Index()
        //{
        //    var appDbContext = _context.Skins.Include(s => s.Categoria);
        //    return View(await appDbContext.ToListAsync());
        //}
        public async Task<IActionResult> Index(string filter, int pageindex = 1, string sort = "Nome")
        {
            var resultado = _context.Skins.Include(s => s.Categoria).AsQueryable();

            if(!string.IsNullOrWhiteSpace(filter))
            {
                resultado = resultado.Where(p => p.Nome.Contains(filter));
            }

            var model = await PagingList.CreateAsync(resultado, 6, pageindex, sort, "Nome");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };
            return View(model);
        }

        // GET: Admin/AdminSkins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Skins == null)
            {
                return NotFound();
            }

            var skin = await _context.Skins
                .Include(s => s.Categoria)
                .FirstOrDefaultAsync(m => m.SkinId == id);
            if (skin == null)
            {
                return NotFound();
            }

            return View(skin);
        }

        // GET: Admin/AdminSkins/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaNome");
            return View();
        }

        // POST: Admin/AdminSkins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SkinId,Tipo,Nome,Estado,SkinFloat,DescricaoDetalhada,Preco,ImagemUrl,ImagemThumbnailUrl,IsSkinPreferida,EmEstoque,CategoriaId")] Skin skin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(skin);
                skin.DescricaoCurta = skin.Tipo + " " + skin.Nome + " - " + skin.Estado;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaNome", skin.CategoriaId);
            return View(skin);
        }

        // GET: Admin/AdminSkins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Skins == null)
            {
                return NotFound();
            }

            var skin = await _context.Skins.FindAsync(id);
            if (skin == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaNome", skin.CategoriaId);
            return View(skin);
        }

        // POST: Admin/AdminSkins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SkinId,Tipo,Nome,Estado,SkinFloat,DescricaoDetalhada,Preco,ImagemUrl,ImagemThumbnailUrl,IsSkinPreferida,EmEstoque,CategoriaId")] Skin skin)
        {
            if (id != skin.SkinId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(skin);
                    skin.DescricaoCurta = skin.Tipo + " " + skin.Nome + " - " + skin.Estado;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SkinExists(skin.SkinId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaNome", skin.CategoriaId);
            return View(skin);
        }

        // GET: Admin/AdminSkins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Skins == null)
            {
                return NotFound();
            }

            var skin = await _context.Skins
                .Include(s => s.Categoria)
                .FirstOrDefaultAsync(m => m.SkinId == id);
            if (skin == null)
            {
                return NotFound();
            }

            return View(skin);
        }

        // POST: Admin/AdminSkins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Skins == null)
            {
                return Problem("Entity set 'AppDbContext.Skins'  is null.");
            }
            var skin = await _context.Skins.FindAsync(id);
            if (skin != null)
            {
                _context.Skins.Remove(skin);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SkinExists(int id)
        {
          return _context.Skins.Any(e => e.SkinId == id);
        }
    }
}
