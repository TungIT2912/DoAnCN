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
            .Where(nv => nv.ChucVu.TenCv == "Doctor") 
            .ToList();

        return View(doctors); 
    }

    public ActionResult DoctorDetails(int id)
    {
        // Fetch details of a specific doctor by their ID
        var doctor = _context.NhanViens
            .FirstOrDefault(nv => nv.MaNv == id);

        if (doctor == null)
        {
            return NotFound(); // Return 404 if not found
        }

        return View(doctor); // Pass the doctor details to the view
    }
}
