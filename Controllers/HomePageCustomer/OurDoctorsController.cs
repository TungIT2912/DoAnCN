//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using WebQuanLyNhaKhoa.Data;

//namespace WebQuanLyNhaKhoa.Controllers.HomePageCustomer
//{
//    public class OurDoctorsController : Controller
//    {
//        private readonly QlnhaKhoaContext _context;

//        public OurDoctorsController(QlnhaKhoaContext context)
//        {
//            _context = context;
//        }

//        // GET: OurDoctors
//        public async Task<IActionResult> Index()
//        {
//            var qlnhaKhoaContext = _context.NhanViens.Include(n => n.MaCvNavigation).Include(n => n.TenDangNhapNavigation);
//            return View(await qlnhaKhoaContext.ToListAsync());
//        }
//    }
//}
