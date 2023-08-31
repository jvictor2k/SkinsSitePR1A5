using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SkinsSite.Context;
using SkinsSite.Models;

namespace SkinsSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Admin")]
    public class AdminCuponsController : Controller
    {
        private readonly AppDbContext _context;

        public AdminCuponsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminCupons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cupons == null)
            {
                return NotFound();
            }

            var cupom = await _context.Cupons
                .Include(s => s.Categoria)
                .FirstOrDefaultAsync(m => m.CupomId == id);
            if (cupom == null)
            {
                return NotFound();
            }

            return View(cupom);
        }

        // GET: Admin/AdminCupons
        public async Task<IActionResult> Index()
        {
            var cupons = await _context.Cupons.Include(c => c.Categoria).ToListAsync();
            return View(cupons);
        }

        //GET: Admin/AdminCupons/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaNome");
            return View();
        }

        //POST: Admin/AdminCupons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CupomId,CupomCodigo,ValorDesconto,Ativo,CategoriaId")] Cupom cupom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cupom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaNome", cupom.CategoriaId);
            return View(cupom);
        }

        // GET: Admin/AdminSkins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cupons == null)
            {
                return NotFound();
            }

            var cupom = await _context.Cupons.FindAsync(id);
            if (cupom == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaNome", cupom.CategoriaId);
            return View(cupom);
        }

        // POST: Admin/AdminSkins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CupomId,CupomCodigo,ValorDesconto,Ativo,CategoriaId")] Cupom cupom)
        {
            if (id != cupom.CupomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cupom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CupomExists(cupom.CupomId))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaNome", cupom.CategoriaId);
            return View(cupom);
        }

        // GET: Admin/AdminSkins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cupons == null)
            {
                return NotFound();
            }

            var cupom = await _context.Cupons
                .Include(s => s.Categoria)
                .FirstOrDefaultAsync(m => m.CupomId == id);
            if (cupom == null)
            {
                return NotFound();
            }

            return View(cupom);
        }

        // POST: Admin/AdminSkins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cupons == null)
            {
                return Problem("Entity set 'AppDbContext.Cupons'  is null.");
            }
            var cupom = await _context.Cupons.FindAsync(id);
            if (cupom != null)
            {
                _context.Cupons.Remove(cupom);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CupomExists(int id)
        {
            return _context.Cupons.Any(e => e.CupomId == id);
        }
    }
}