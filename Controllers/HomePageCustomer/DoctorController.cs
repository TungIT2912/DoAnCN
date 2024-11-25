using Microsoft.AspNetCore.Mvc;
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
            .Where(nv => nv.ChucVu.TenCv == "Bác sĩ") 
            .ToList();

        return View(doctors); 
    }

    public ActionResult DoctorDetails(int id)
    {



                {
            return View();
        }
        // var doctor = _context.NhanViens
        //     .FirstOrDefault(nv => nv.MaNv == id);

        // if (doctor == null)
        // {
        //     return NotFound(); 
        // }

        // return View(doctor); 
    }
}
