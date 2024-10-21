using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;
using WebQuanLyNhaKhoa.Data;

namespace WebQuanLyNhaKhoa.Controllers.HomepageAdmin
{
    public class DieuTriController : Controller
    {
        private readonly QlnhaKhoaContext _context;

        public DieuTriController(QlnhaKhoaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var qlnhaKhoaContext = _context.DieuTris.Include(d => d.IddichVuNavigation).Include(d => d.IddungCuNavigation).ToList();
            ViewData["Idkham"] = new SelectList(_context.DanhSachKhams, "Idkham", "Idkham");
            ViewData["IddichVu"] = new SelectList(_context.DichVus, "IddichVu", "TenDichVu");
            ViewData["IddungCu"] = new SelectList(_context.Khos, "IddungCu", "TenDungCu");
            return View(qlnhaKhoaContext);
        }
        public async Task<IActionResult> Add(string id)
        {
            ViewData["Idkham"] = new SelectList(_context.DanhSachKhams, "Idkham", "Idkham");
            ViewData["IddichVu"] = new SelectList(_context.DichVus, "IddichVu", "TenDichVu");
            ViewData["IddungCu"] = new SelectList(_context.Khos, "IddungCu", "TenDungCu");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(string id, DieuTri dt)
        {
            var khoExists = await _context.Khos.AnyAsync(k => k.IddungCu == dt.IddungCu);
            var khoId = _context.Khos.Find(dt.IddungCu);
            var thanhTienLSNX = _context.ThiTruongs.SingleOrDefault(k => k.TenSanPham == khoId.TenDungCu).DonGia;
            var DonGia = _context.DichVus.SingleOrDefault(m => m.IddichVu == dt.IddichVu).DonGia;
            var DonviTinh = _context.ThiTruongs.FirstOrDefault(k => k.TenSanPham == khoId.TenDungCu);
            if (khoExists)
            {
                if (dt.SoLuong <= 0)
                {
                    ModelState.AddModelError("SoLuong", "Số lượng phải lớn hơn 0.");
                    ViewData["IddungCu"] = new SelectList(_context.Khos, "IddungCu", "TenDungCu", dt.IddungCu);
                    return View(dt);
                }
                else
                {
                    if (id == null)
                    {
                        return NotFound();
                    }
                    LichSuNhapXuat lsnew = new LichSuNhapXuat
                    {
                        IddungCu = dt.IddungCu,
                        NoiDung = "Xuất",
                        TenDungCu = khoId.TenDungCu,
                        Loai = khoId.Loai,
                        DonViTinh = khoId.DonViTinh,
                        SoLuongNhapXuat = dt.SoLuong,
                        Don = (decimal)DonviTinh.DonGia,
                        NgayNhap = DateTime.Now,
                        ThanhTien = dt.SoLuong * (DonviTinh.DonGia)
                    };

                    _context.LichSuNhapXuats.Add(lsnew);
                    _context.SaveChanges();
                    if (khoExists)
                    {

                        if (khoId != null)
                        {
                            khoId.SoLuong -= dt.SoLuong;
                             _context.SaveChanges();
                        }

                    }
                    DieuTri newdt = new DieuTri
                    {
                        Idkham = dt.Idkham,
                        IddichVu = dt.IddichVu,
                        IddungCu = dt.IddungCu,
                        SoLuong = dt.SoLuong,
                        ThanhTien = (dt.SoLuong * thanhTienLSNX) + DonGia,
                    };
                    await _context.DieuTris.AddAsync(newdt);
                     _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
    }
}

