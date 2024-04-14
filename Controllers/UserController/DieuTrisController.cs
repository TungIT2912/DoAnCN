using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebQuanLyNhaKhoa.Data;

namespace WebQuanLyNhaKhoa.Controllers.UserController
{
    public class DieuTrisController : Controller
    {
        private readonly QlnhaKhoaContext _context;

        public DieuTrisController(QlnhaKhoaContext context)
        {
            _context = context;
        }

        // GET: DieuTris
        public async Task<IActionResult> Index()
        {
            var qlnhaKhoaContext = _context.DieuTris.Include(d => d.IddichVuNavigation).Include(d => d.IddungCuNavigation).Include(d => d.IdkhamNavigation);
            return View(await qlnhaKhoaContext.ToListAsync());
        }

        // GET: DieuTris/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dieuTri = await _context.DieuTris
                .Include(d => d.IddichVuNavigation)
                .Include(d => d.IddungCuNavigation)
                .Include(d => d.IdkhamNavigation)
                .FirstOrDefaultAsync(m => m.IddieuTri == id);
            if (dieuTri == null)
            {
                return NotFound();
            }

            return View(dieuTri);
        }

        // GET: DieuTris/Create
        public IActionResult Create()
        {
            ViewData["IddichVu"] = new SelectList(_context.DichVus, "IddichVu", "IddichVu");
            ViewData["IddungCu"] = new SelectList(_context.Khos, "IddungCu", "IddungCu");
            ViewData["Idkham"] = new SelectList(_context.DanhSachKhams, "Idkham", "Idkham");
            return View();
        }

        // POST: DieuTris/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IddieuTri,IddichVu,Idkham,IddungCu,SoLuong,ThanhTien")] DieuTri dieuTri)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dieuTri);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IddichVu"] = new SelectList(_context.DichVus, "IddichVu", "IddichVu", dieuTri.IddichVu);
            ViewData["IddungCu"] = new SelectList(_context.Khos, "IddungCu", "IddungCu", dieuTri.IddungCu);
            ViewData["Idkham"] = new SelectList(_context.DanhSachKhams, "Idkham", "Idkham", dieuTri.Idkham);
            return View(dieuTri);
        }

        // GET: DieuTris/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dieuTri = await _context.DieuTris.FindAsync(id);
            if (dieuTri == null)
            {
                return NotFound();
            }
            ViewData["IddichVu"] = new SelectList(_context.DichVus, "IddichVu", "IddichVu", dieuTri.IddichVu);
            ViewData["IddungCu"] = new SelectList(_context.Khos, "IddungCu", "IddungCu", dieuTri.IddungCu);
            ViewData["Idkham"] = new SelectList(_context.DanhSachKhams, "Idkham", "Idkham", dieuTri.Idkham);
            return View(dieuTri);
        }

        // POST: DieuTris/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IddieuTri,IddichVu,Idkham,IddungCu,SoLuong,ThanhTien")] DieuTri dieuTri)
        {
            if (id != dieuTri.IddieuTri)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dieuTri);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DieuTriExists(dieuTri.IddieuTri))
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
            ViewData["IddichVu"] = new SelectList(_context.DichVus, "IddichVu", "IddichVu", dieuTri.IddichVu);
            ViewData["IddungCu"] = new SelectList(_context.Khos, "IddungCu", "IddungCu", dieuTri.IddungCu);
            ViewData["Idkham"] = new SelectList(_context.DanhSachKhams, "Idkham", "Idkham", dieuTri.Idkham);
            return View(dieuTri);
        }

        // GET: DieuTris/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dieuTri = await _context.DieuTris
                .Include(d => d.IddichVuNavigation)
                .Include(d => d.IddungCuNavigation)
                .Include(d => d.IdkhamNavigation)
                .FirstOrDefaultAsync(m => m.IddieuTri == id);
            if (dieuTri == null)
            {
                return NotFound();
            }

            return View(dieuTri);
        }

        // POST: DieuTris/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dieuTri = await _context.DieuTris.FindAsync(id);
            if (dieuTri != null)
            {
                _context.DieuTris.Remove(dieuTri);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DieuTriExists(int id)
        {
            return _context.DieuTris.Any(e => e.IddieuTri == id);
        }
    }
}
