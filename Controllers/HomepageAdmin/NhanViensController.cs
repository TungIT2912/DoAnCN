using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebQuanLyNhaKhoa.Data;

namespace WebQuanLyNhaKhoa.Controllers.HomepageAdmin
{
    public class NhanViensController : Controller
    {
        private readonly QlnhaKhoaContext _context;

        public NhanViensController(QlnhaKhoaContext context)
        {
            _context = context;
        }

        // GET: NhanViens
        public IActionResult Index(string query = "", string role = "nothing", string sort = "nothing")
        {
            if (query == "")
            {
                if (role == "nothing")
                {
                    if (sort == "nothing")
                    {
                        List<NhanVien> nhanViens = _context.NhanViens.Include(n => n.MaCvNavigation).Include(n => n.TenDangNhapNavigation).ToList();
                        return View(nhanViens);
                    }
                    else
                    {
                        if (sort == "Ten")
                        {
                            List<NhanVien> nhanViens = _context.NhanViens.Include(n => n.MaCvNavigation).Include(n => n.TenDangNhapNavigation).OrderBy(n => n.Ten).ToList();
                            return View(nhanViens);
                        }
                        else
                        {
                            List<NhanVien> nhanViens = _context.NhanViens.Include(n => n.MaCvNavigation).Include(n => n.TenDangNhapNavigation).OrderBy(n => n.MaCvNavigation.TenCv).ToList();
                            return View(nhanViens);
                        }
                    }
                }
                else
                {
                    if (sort == "Ten")
                    {
                        List<NhanVien> nhanViens = _context.NhanViens.Include(n => n.MaCvNavigation).Include(n => n.TenDangNhapNavigation).Where(n => n.TenDangNhap == role).OrderBy(n => n.Ten).ToList();
                        return View(nhanViens);
                    }
                    else
                    {
                        List<NhanVien> nhanViens = _context.NhanViens.Include(n => n.MaCvNavigation).Include(n => n.TenDangNhapNavigation).Where(n => n.TenDangNhap == role).OrderBy(n => n.MaCvNavigation.TenCv).ToList();
                        return View(nhanViens);
                    }
                }
            }
            else
            {
                if (role == "nothing")
                {
                    if (sort == "nothing")
                    {
                        List<NhanVien> nhanViens = _context.NhanViens
                        .Include(n => n.MaCvNavigation)
                        .Include(n => n.TenDangNhapNavigation)
                        .Where(n => n.Ten.Contains(query)).ToList();
                        return View(nhanViens);
                    }
                    else
                    {
                        if (sort == "Ten")
                        {
                            List<NhanVien> nhanViens = _context.NhanViens
                       .Include(n => n.MaCvNavigation)
                       .Include(n => n.TenDangNhapNavigation)
                       .Where(n => n.Ten.Contains(query)).OrderBy(n => n.Ten).ToList();
                            return View(nhanViens);
                        }
                        else
                        {
                            List<NhanVien> nhanViens = _context.NhanViens
                    .Include(n => n.MaCvNavigation)
                    .Include(n => n.TenDangNhapNavigation)
                     .Where(n => n.Ten.Contains(query)).OrderBy(n => n.MaCvNavigation.TenCv).ToList();
                            return View(nhanViens);
                        }
                    }

                }
                else
                {
                    if (sort == "nothing")
                    {
                        List<NhanVien> nhanViens = _context.NhanViens
                                 .Include(n => n.MaCvNavigation)
                                 .Include(n => n.TenDangNhapNavigation)
                                 .Where(n => n.Ten.Contains(query)).Where(n => n.TenDangNhap == role).ToList();
                        return View(nhanViens);
                    }
                    else
                    {
                        if (sort == "Ten")
                        {
                            List<NhanVien> nhanViens = _context.NhanViens
                       .Include(n => n.MaCvNavigation)
                       .Include(n => n.TenDangNhapNavigation)
                       .Where(n => n.Ten.Contains(query)).Where(n => n.TenDangNhap == role)
                       .OrderBy(n => n.Ten).ToList();
                            return View(nhanViens);
                        }
                        else
                        {
                            List<NhanVien> nhanViens = _context.NhanViens
                    .Include(n => n.MaCvNavigation)
                    .Include(n => n.TenDangNhapNavigation)
                     .Where(n => n.Ten.Contains(query)).Where(n => n.TenDangNhap == role)
                     .OrderBy(n => n.MaCvNavigation.TenCv).ToList();
                            return View(nhanViens);
                        }
                    }

                }
            }

            return View();
        }
        // GET: NhanViens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens
                .Include(n => n.MaCvNavigation)
                .Include(n => n.TenDangNhapNavigation)
                .FirstOrDefaultAsync(m => m.MaNv == id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            return View(nhanVien);
        }
        private async Task<string> SaveImage(IFormFile Hinh)
        {
            var savePath = Path.Combine("wwwroot/images", Hinh.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await Hinh.CopyToAsync(fileStream);
            }
            return "/images/" + Hinh.FileName; // Trả về đường dẫn tương đối
        }
        // GET: NhanViens/Create
        public IActionResult Create()
        {
            ViewData["MaCv"] = new SelectList(_context.ChucVus, "MaCv", "TenCv");
            ViewData["TenDangNhap"] = new SelectList(_context.TaiKhoans, "TenDangNhap", "TenDangNhap");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaNv,TenDangNhap,Ten,Sdt,MaCv,KinhNghiem,Hinh")] NhanVien nhanVien, IFormFile Hinh)
        {

            if (Hinh != null)
            {
                //Lưu hình ảnh đại diện
                nhanVien.Hinh = await SaveImage(Hinh);
                _context.NhanViens.Add(nhanVien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaCv"] = new SelectList(_context.ChucVus, "MaCv", "TenCv", nhanVien.MaCv);
            ViewData["TenDangNhap"] = new SelectList(_context.TaiKhoans, "TenDangNhap", "TenDangNhap", nhanVien.TenDangNhap);
            return View(nhanVien);
        }
        // GET: NhanViens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            ViewData["MaCv"] = new SelectList(_context.ChucVus, "MaCv", "TenCv", nhanVien.MaCv);
            ViewData["TenDangNhap"] = new SelectList(_context.TaiKhoans, "TenDangNhap", "TenDangNhap", nhanVien.TenDangNhap);
            return View(nhanVien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,[Bind("MaNv,TenDangNhap,Ten,Sdt,MaCv,KinhNghiem,Hinh")] NhanVien nhanVien, IFormFile Hinh)
        {
            ModelState.Remove("Hinh"); // Loại bỏ xác thực ModelState cho ImageUrl
            if (id != nhanVien.MaNv)
            {
                return NotFound();
            }

            var existingNV = _context.NhanViens.FirstOrDefault(n => n.MaNv == id); // Giả định có phương thức GetByIdAsync

            // Giữ nguyên thông tin hình ảnh nếu không có hình mới được tải lên
            if (Hinh == null)
            {
                existingNV.Hinh = existingNV.Hinh;
            }
            else
            {
                // Lưu hình ảnh mới
                existingNV.Hinh = await SaveImage(Hinh);
            }
            // Cập nhật các thông tin khác của sản phẩm
            existingNV.TenDangNhap = nhanVien.TenDangNhap;
            existingNV.Ten = nhanVien.Ten;
            existingNV.Sdt = nhanVien.Sdt;
            existingNV.MaCv = nhanVien.MaCv;
            existingNV.KinhNghiem = nhanVien.KinhNghiem;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: NhanViens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens
                .Include(n => n.MaCvNavigation)
                .Include(n => n.TenDangNhapNavigation)
                .FirstOrDefaultAsync(m => m.MaNv == id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            return View(nhanVien);
        }

        // POST: NhanViens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien != null)
            {
                _context.NhanViens.Remove(nhanVien);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhanVienExists(int id)
        {
            return _context.NhanViens.Any(e => e.MaNv == id);
        }

    }
}
