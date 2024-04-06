using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebQuanLyNhaKhoa.Data;

namespace WebQuanLyNhaKhoa.Controllers.HomePageCustomer
{
    public class HomeController : Controller
    {
        private readonly QlnhaKhoaContext _context;

        public HomeController(QlnhaKhoaContext context)
        {
            _context = context;
        }

        // GET: Home
        public async Task<IActionResult> Index()
        {
            int Experienced = 10;
            var qlnhaKhoaContext = _context.NhanViens.Where(n => Convert.ToInt16(n.KinhNghiem) > Experienced).Take(4);
            return View(await qlnhaKhoaContext.ToListAsync());
        }
        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images", image.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + image.FileName;
        }
       
    }
}
