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
    public class DonThuocsController : Controller
    {
        private readonly QlnhaKhoaContext _context;

        public DonThuocsController(QlnhaKhoaContext context)
        {
            _context = context;
        }

        // GET: DonThuocs
        public async Task<IActionResult> Index()
        {

            var qlnhaKhoaContext = _context.DonThuocs.Include(d => d.IddungCuNavigation).Include(d => d.IdkhamNavigation);
            return View(await qlnhaKhoaContext.ToListAsync());
        }

        // GET: DonThuocs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donThuoc = await _context.DonThuocs
                .Include(d => d.IddungCuNavigation)
                .Include(d => d.IdkhamNavigation)
                .FirstOrDefaultAsync(m => m.IddonThuoc == id);
            if (donThuoc == null)
            {
                return NotFound();
            }

            return View(donThuoc);
        }

        // GET: DonThuocs/Create
        public IActionResult Create()
        {
            ViewData["Idkham"] = new SelectList(_context.DanhSachKhams, "Idkham", "Idkham");
            var medicineItems = _context.Khos.Where(k => k.Loai == "Thuốc").ToList();
            ViewData["IddungCu"] = new SelectList(medicineItems, "IddungCu", "TenDungCu");
           
            return View();
        }

        // POST: DonThuocs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IddonThuoc,Idkham,IddungCu,SoLuong,ThanhGia,TongTien,NgayLapDt")] DonThuoc donThuoc,string id)
        {
            if (donThuoc.NgayLapDt < DateTime.Today)
            {
                ModelState.AddModelError("NgayLapDt", "Ngày không thể là một ngày trong quá khứ.");
            }

            // Kiểm tra xem ngày có nằm trong phạm vi cho phép của SQL Server không
            if (donThuoc.NgayLapDt < new DateTime(1753, 1, 1) || donThuoc.NgayLapDt > new DateTime(9999, 12, 31))
            {
                ModelState.AddModelError("NgayLapDt", "Ngày không hợp lệ.");
            }
            var khoExists = await _context.Khos.AnyAsync(k => k.IddungCu == donThuoc.IddungCu);
            if (khoExists)
            {
                if (donThuoc.SoLuong <= 0)
                {
                    ModelState.AddModelError("SoLuong", "Số lượng phải lớn hơn 0.");
                    ViewData["Idkham"] = new SelectList(_context.DanhSachKhams, "Idkham", "Idkham", donThuoc.Idkham);
                    var medicineItems = _context.Khos.Where(k => k.Loai == "Thuốc").ToList();
                    ViewData["IddungCu"] = new SelectList(medicineItems, "IddungCu", "TenDungCu");
                    return View(donThuoc);
                }
                else
                {
                    var tenDungCu = await _context.Khos.Where(k => k.IddungCu == donThuoc.IddungCu).Select(k => k.TenDungCu).FirstOrDefaultAsync();
                    var Loai = await _context.Khos.Where(k => k.IddungCu == donThuoc.IddungCu).Select(k => k.Loai).FirstOrDefaultAsync();
                    var DVT = await _context.Khos.Where(k => k.IddungCu == donThuoc.IddungCu).Select(k => k.DonViTinh).FirstOrDefaultAsync();
                    var Don = await _context.Khos.Where(k => k.IddungCu == donThuoc.IddungCu).Select(k => k.IdsanPhamNavigation.DonGia).FirstOrDefaultAsync();
                    //ViewData["Idkham"] = new SelectList(_context.DanhSachKhams, "Idkham", "Idkham", donThuoc.Idkham);
                    //donThuoc.Idkham = (Request.Form["Idkham"]).ToString();
                    donThuoc.Idkham = id;
                    donThuoc.ThanhGia = Don;
                    donThuoc.NgayLapDt = DateTime.Today;
                    donThuoc.TongTien = (decimal)donThuoc.SoLuong * Don;
                    var productInKho = await _context.Khos.FirstOrDefaultAsync(k => k.IddungCu == donThuoc.IddungCu);
                    LichSuNhapXuat ls = new LichSuNhapXuat() { 
                        IddungCu= donThuoc.IddungCu,
                        NoiDung="Xuất",
                        TenDungCu= tenDungCu,
                        Loai= Loai,
                        DonViTinh= DVT,
                        Don= Don,
                        SoLuongNhapXuat=donThuoc.SoLuong,
                        ThanhTien=donThuoc.TongTien,
                        NgayNhap= (DateTime)donThuoc.NgayLapDt
                    };
                    _context.LichSuNhapXuats.Add(ls);

                    await _context.SaveChangesAsync();
                    if (productInKho != null)
                    {
                            // Giảm số lượng sản phẩm trong kho
                            productInKho.SoLuong -= donThuoc.SoLuong;
                            _context.Update(productInKho);
                            await _context.SaveChangesAsync();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Không tìm thấy sản phẩm trong kho.");
                        ViewData["Idkham"] = new SelectList(_context.DanhSachKhams, "Idkham", "Idkham", donThuoc.Idkham);
                        var medicineItems = _context.Khos.Where(k => k.Loai == "Thuốc").ToList();
                        ViewData["IddungCu"] = new SelectList(medicineItems, "IddungCu", "TenDungCu");
                        return View(donThuoc);
                    }
                    _context.DonThuocs.Add(donThuoc);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(donThuoc);
        }

        
        private bool DonThuocExists(int id)
        {
            return _context.DonThuocs.Any(e => e.IddonThuoc == id);
        }
    }
}
