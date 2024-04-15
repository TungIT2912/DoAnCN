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
    public class ServicesController : Controller
    {
        private readonly QlnhaKhoaContext _context;

        public ServicesController(QlnhaKhoaContext context)
        {
            _context = context;
        }

        // GET: Services
        public async Task<IActionResult> Index()
        {
            var qlnhaKhoaContext = _context.DichVus.Include(d => d.IdchanDoanNavigation);
            return View(await qlnhaKhoaContext.ToListAsync());
        }
    }
}
