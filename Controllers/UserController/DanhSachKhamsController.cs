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
    public class DanhSachKhamsController : Controller
    {
        private readonly QlnhaKhoaContext _context;

        public DanhSachKhamsController(QlnhaKhoaContext context)
        {
            _context = context;
        }

        // GET: DanhSachKhams
        public async Task<IActionResult> Index()
        {
            var qlnhaKhoaContext = _context.DanhSachKhams.Include(d => d.IdbenhNhanNavigation).Include(d => d.MaNvNavigation);
            return View(await qlnhaKhoaContext.ToListAsync());
        }

        // GET: DanhSachKhams/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhSachKham = await _context.DanhSachKhams
                .Include(d => d.IdbenhNhanNavigation)
                .Include(d => d.MaNvNavigation)
                .FirstOrDefaultAsync(m => m.Idkham == id);
            if (danhSachKham == null)
            {
                return NotFound();
            }

            return View(danhSachKham);
        }

        // GET: DanhSachKhams/Create
        public IActionResult Create()
        {
            ViewData["IdbenhNhan"] = new SelectList(_context.BenhNhans, "IdbenhNhan", "IdbenhNhan");
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv");
            return View();
        }

        // POST: DanhSachKhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idkham,IdbenhNhan,MaNv,NgayKham")] DanhSachKham danhSachKham)
        {
            if (ModelState.IsValid)
            {
                _context.Add(danhSachKham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdbenhNhan"] = new SelectList(_context.BenhNhans, "IdbenhNhan", "IdbenhNhan", danhSachKham.IdbenhNhan);
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv", danhSachKham.MaNv);
            return View(danhSachKham);
        }

        // GET: DanhSachKhams/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhSachKham = await _context.DanhSachKhams.FindAsync(id);
            if (danhSachKham == null)
            {
                return NotFound();
            }
            ViewData["IdbenhNhan"] = new SelectList(_context.BenhNhans, "IdbenhNhan", "IdbenhNhan", danhSachKham.IdbenhNhan);
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv", danhSachKham.MaNv);
            return View(danhSachKham);
        }

        // POST: DanhSachKhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Idkham,IdbenhNhan,MaNv,NgayKham")] DanhSachKham danhSachKham)
        {
            if (id != danhSachKham.Idkham)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(danhSachKham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DanhSachKhamExists(danhSachKham.Idkham))
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
            ViewData["IdbenhNhan"] = new SelectList(_context.BenhNhans, "IdbenhNhan", "IdbenhNhan", danhSachKham.IdbenhNhan);
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv", danhSachKham.MaNv);
            return View(danhSachKham);
        }

        // GET: DanhSachKhams/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhSachKham = await _context.DanhSachKhams
                .Include(d => d.IdbenhNhanNavigation)
                .Include(d => d.MaNvNavigation)
                .FirstOrDefaultAsync(m => m.Idkham == id);
            if (danhSachKham == null)
            {
                return NotFound();
            }

            return View(danhSachKham);
        }

        // POST: DanhSachKhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var danhSachKham = await _context.DanhSachKhams.FindAsync(id);
            if (danhSachKham != null)
            {
                _context.DanhSachKhams.Remove(danhSachKham);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DanhSachKhamExists(string id)
        {
            return _context.DanhSachKhams.Any(e => e.Idkham == id);
        }
    }
}
