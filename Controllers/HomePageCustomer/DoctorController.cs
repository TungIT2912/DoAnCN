using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.DTO;

public class DoctorController : Controller
{
    private readonly ApplicationDbContext _context;

    public DoctorController(ApplicationDbContext context)
    {
        _context = context;
    }

    public ActionResult Doctors()
    {
       
        var doctors = _context.NhanViens
            .Where(nv => nv.ChucVu.MaCv == 1) 
            .ToList();

        return View(doctors); 
    }



    public async Task<IActionResult> DoctorDetails(int id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var nhanVien = await _context.NhanViens
                    .Include(n => n.ChucVu)
                    .Include(n => n.DichVu)
                    .Include(n => n.TaiKhoan)
                    .Include(n => n.DichVu)
                    .FirstOrDefaultAsync(n => n.MaNv == id);
        if (nhanVien == null)
        {
            Console.WriteLine($"Nhân viên với mã nhân viên bạn vừa tìm không tồn tại.");
            return NotFound();
        }

        var nhanVienDTO = new NhanVienDTO
        {
            MaNv = id,
            Ten = nhanVien.Ten,
            Sdt = nhanVien.Sdt,
            TenCv = nhanVien.ChucVu.TenCv,
            KinhNghiem = nhanVien.KinhNghiem,
            IddichVu = nhanVien.IddichVu,
            TenChuyenNghanh = nhanVien.DichVu.TenDichVu,
            Diachi = nhanVien.Diachi,
            Gioi = nhanVien.Gioi,
            Hinh = nhanVien.Hinh,
            Email = nhanVien.Email,
            MatKhau = nhanVien.TaiKhoan.MatKhau,
            Role = nhanVien.TaiKhoan.Role,
            Mota = nhanVien.Mota,
        };
        ViewBag.ChucVuList = new SelectList(_context.ChucVus, "MaCv", "TenCv");
        ViewBag.Roles = new SelectList(new[] { "Admin", "Customer" });
        ViewBag.DichVuList = new SelectList(_context.DichVus, "IddichVu", "TenDichVu");
        return View(nhanVienDTO);

    }
}
