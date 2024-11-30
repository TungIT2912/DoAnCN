using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQuanLyNhaKhoa.Data;


namespace WebQuanLyNhaKhoa.Controllers.HomePageCustomer
{
    public class ContactController : Controller
    {
                private readonly ApplicationDbContext _context;

          public ContactController(ApplicationDbContext context)
    {
        _context = context;  // Assigning the injected context to the _context field
    }
        public IActionResult Index()
        {
            return View();
        }
    
    
public async Task<IActionResult> Aboutus()
{
    int nhanViensCount = await _context.NhanViens.CountAsync();
    int khachHangCount = await _context.HoaDons.CountAsync();

    ViewBag.NhanViensCount = nhanViensCount;
    ViewBag.KhachHangCount = khachHangCount;

    return View();
}

    }
}
