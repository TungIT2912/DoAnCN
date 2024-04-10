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
    public class LichSuNhapXuatsController : Controller
    {
        private readonly QlnhaKhoaContext _context;

        public LichSuNhapXuatsController(QlnhaKhoaContext context)
        {
            _context = context;
        }

        // GET: LichSuNhapXuats
        public async Task<IActionResult> Index()
        {
            var qlnhaKhoaContext = _context.LichSuNhapXuats.Include(l => l.IddungCuNavigation);
            return View(await qlnhaKhoaContext.ToListAsync());
        }

        // GET: LichSuNhapXuats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lichSuNhapXuat = await _context.LichSuNhapXuats
                .Include(l => l.IddungCuNavigation)
                .FirstOrDefaultAsync(m => m.MaLs == id);
            if (lichSuNhapXuat == null)
            {
                return NotFound();
            }

            return View(lichSuNhapXuat);
        }

        // GET: LichSuNhapXuats/Create
        public IActionResult Create()
        {
            string[] Contents = { "Nhập", "Xuất" };
            ViewBag.Contents = new SelectList(Contents);

            ViewData["IddungCu"] = new SelectList(_context.Khos, "IddungCu", "TenDungCu");

            return View();
        }

        // POST: LichSuNhapXuats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaLs,NoiDung,IddungCu,TenDungCu,Loai,DonViTinh,SoLuongNhapXuat,Don,ThanhTien,NgayNhap")] LichSuNhapXuat lichSuNhapXuat)
        {
            var khoExists = await _context.Khos.AnyAsync(k => k.IddungCu == lichSuNhapXuat.IddungCu);
            if (khoExists)
            {
                if (lichSuNhapXuat.SoLuongNhapXuat <= 0)
                {
                    ModelState.AddModelError("SoLuongNhapXuat", "Số lượng phải lớn hơn 0.");
                    string[] Contents = { "Nhập", "Xuất" };
                    ViewBag.Contents = new SelectList(Contents);
                    ViewData["IddungCu"] = new SelectList(_context.Khos, "IddungCu", "TenDungCu", lichSuNhapXuat.IddungCu);
                    return View(lichSuNhapXuat);
                }
                else
                {
                    var tenDungCu = await _context.Khos.Where(k => k.IddungCu == lichSuNhapXuat.IddungCu).Select(k => k.TenDungCu).FirstOrDefaultAsync();
                    lichSuNhapXuat.TenDungCu = tenDungCu;
                    var Loai = await _context.Khos.Where(k => k.IddungCu == lichSuNhapXuat.IddungCu).Select(k => k.Loai).FirstOrDefaultAsync();
                    lichSuNhapXuat.Loai = Loai;
                    var DVT = await _context.Khos.Where(k => k.IddungCu == lichSuNhapXuat.IddungCu).Select(k => k.DonViTinh).FirstOrDefaultAsync();
                    lichSuNhapXuat.DonViTinh = DVT;
                    var Don = await _context.Khos.Where(k => k.IddungCu == lichSuNhapXuat.IddungCu).Select(k => k.IdsanPhamNavigation.DonGia).FirstOrDefaultAsync();
                    lichSuNhapXuat.Don = Don;
                    lichSuNhapXuat.ThanhTien = (decimal)lichSuNhapXuat.SoLuongNhapXuat * Don;
                    var productInKho = await _context.Khos.FirstOrDefaultAsync(k => k.IddungCu == lichSuNhapXuat.IddungCu);
                    if (productInKho != null)
                    {
                        if (lichSuNhapXuat.NoiDung == "Nhập")
                        {
                            // Tăng số lượng sản phẩm trong kho
                            productInKho.SoLuong += lichSuNhapXuat.SoLuongNhapXuat;
                            _context.Update(productInKho);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            // Giảm số lượng sản phẩm trong kho
                            productInKho.SoLuong -= lichSuNhapXuat.SoLuongNhapXuat;
                            _context.Update(productInKho);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Không tìm thấy sản phẩm trong kho.");
                        string[] Contents = { "Nhập", "Xuất" };
                        ViewBag.Contents = new SelectList(Contents);
                        ViewData["IddungCu"] = new SelectList(_context.Khos, "IddungCu", "TenDungCu", lichSuNhapXuat.IddungCu);
                        return View(lichSuNhapXuat);
                    }
                    _context.LichSuNhapXuats.Add(lichSuNhapXuat);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }

            return View(lichSuNhapXuat);
        }

        // GET: LichSuNhapXuats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lichSuNhapXuat = await _context.LichSuNhapXuats.FindAsync(id);
            if (lichSuNhapXuat == null)
            {
                return NotFound();
            }
            ViewData["IddungCu"] = new SelectList(_context.Khos, "IddungCu", "IddungCu", lichSuNhapXuat.IddungCu);
            return View(lichSuNhapXuat);
        }

        // POST: LichSuNhapXuats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaLs,NoiDung,IddungCu,TenDungCu,Loai,DonViTinh,SoLuongNhapXuat,Don,ThanhTien,NgayNhap")] LichSuNhapXuat lichSuNhapXuat)
        {
            if (id != lichSuNhapXuat.MaLs)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lichSuNhapXuat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LichSuNhapXuatExists(lichSuNhapXuat.MaLs))
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
            ViewData["IddungCu"] = new SelectList(_context.Khos, "IddungCu", "IddungCu", lichSuNhapXuat.IddungCu);
            return View(lichSuNhapXuat);
        }

        // GET: LichSuNhapXuats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lichSuNhapXuat = await _context.LichSuNhapXuats
                .Include(l => l.IddungCuNavigation)
                .FirstOrDefaultAsync(m => m.MaLs == id);
            if (lichSuNhapXuat == null)
            {
                return NotFound();
            }

            return View(lichSuNhapXuat);
        }

        // POST: LichSuNhapXuats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lichSuNhapXuat = await _context.LichSuNhapXuats.FindAsync(id);
            if (lichSuNhapXuat != null)
            {
                _context.LichSuNhapXuats.Remove(lichSuNhapXuat);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LichSuNhapXuatExists(int id)
        {
            return _context.LichSuNhapXuats.Any(e => e.MaLs == id);
        }
    }
}
