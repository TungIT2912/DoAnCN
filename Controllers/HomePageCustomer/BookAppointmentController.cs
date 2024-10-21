using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQuanLyNhaKhoa.Data;

namespace WebQuanLyNhaKhoa.Controllers.HomePageCustomer
{
    public class BookAppointmentController : Controller
    {
        private readonly QlnhaKhoaContext _context;

        public BookAppointmentController(QlnhaKhoaContext context)
        {
            _context = context;
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(BenhNhan benhNhan)
        {
         _context.BenhNhans.Add(benhNhan);
         _context.SaveChanges();
            //return View();
            return RedirectToAction("Index", "Home");
        }
    }
}
