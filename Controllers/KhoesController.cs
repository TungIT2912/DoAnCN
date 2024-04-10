using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebQuanLyNhaKhoa.Data;

namespace WebQuanLyNhaKhoa.Controllers
{
    public class KhoesController : Controller
    {
        private readonly QlnhaKhoaContext _context;

        public KhoesController(QlnhaKhoaContext context)
        {
            _context = context;
        }

        // GET: Khoes
        public async Task<IActionResult> Index()
        {
            var qlnhaKhoaContext = _context.Khos.Include(k => k.IdsanPhamNavigation);
            return View(await qlnhaKhoaContext.ToListAsync());
        }

        // GET: Khoes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kho = await _context.Khos
                .Include(k => k.IdsanPhamNavigation)
                .FirstOrDefaultAsync(m => m.IddungCu == id);
            if (kho == null)
            {
                return NotFound();
            }

            return View(kho);
        }

        // GET: Khoes/Create
        public IActionResult Create()
        {
            ViewData["IdsanPham"] = new SelectList(_context.ThiTruongs, "IdsanPham", "IdsanPham");
            return View();
        }

        // POST: Khoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdsanPham,IddungCu,TenDungCu,Loai,DonViTinh,SoLuong")] Kho kho)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kho);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdsanPham"] = new SelectList(_context.ThiTruongs, "IdsanPham", "IdsanPham", kho.IdsanPham);
            return View(kho);
        }

        // GET: Khoes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kho = await _context.Khos.FindAsync(id);
            if (kho == null)
            {
                return NotFound();
            }
            ViewData["IdsanPham"] = new SelectList(_context.ThiTruongs, "IdsanPham", "IdsanPham", kho.IdsanPham);
            return View(kho);
        }

        // POST: Khoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdsanPham,IddungCu,TenDungCu,Loai,DonViTinh,SoLuong")] Kho kho)
        {
            if (id != kho.IddungCu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kho);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhoExists(kho.IddungCu))
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
            ViewData["IdsanPham"] = new SelectList(_context.ThiTruongs, "IdsanPham", "IdsanPham", kho.IdsanPham);
            return View(kho);
        }

        // GET: Khoes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kho = await _context.Khos
                .Include(k => k.IdsanPhamNavigation)
                .FirstOrDefaultAsync(m => m.IddungCu == id);
            if (kho == null)
            {
                return NotFound();
            }

            return View(kho);
        }

        // POST: Khoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var kho = await _context.Khos.FindAsync(id);
            if (kho != null)
            {
                _context.Khos.Remove(kho);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KhoExists(string id)
        {
            return _context.Khos.Any(e => e.IddungCu == id);
        }
    }
}
