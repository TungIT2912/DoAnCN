using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebQuanLyNhaKhoa.Data;

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

    

public IActionResult DoctorDetails(int id)
    {
        var nhanVien = _context.NhanViens
            .Include(nv => nv.ChucVu)
            .FirstOrDefault(nv => nv.MaNv == id);

        if (nhanVien == null)
        {
            return NotFound();
        }

        return View(nhanVien); 

        
    }
}
