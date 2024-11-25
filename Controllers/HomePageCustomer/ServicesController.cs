using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQuanLyNhaKhoa.Data;
using X.PagedList;
using X.PagedList.Extensions;  // Make sure to include this namespace

namespace WebQuanLyNhaKhoa.Controllers.HomePageCustomer
{
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Services
        public async Task<IActionResult> Index()
        {
            var qlnhaKhoaContext = _context.DichVus;
            return View(await qlnhaKhoaContext.ToListAsync());
        }

        public ActionResult Pricing(int? page)
        {
            int pageSize = 4;  
            int pageNumber = (page ?? 1);  // Default to page 1 if no page parameter

            // Use AsQueryable() to ensure that ToPagedList can be applied
            var pricingData = _context.DichVus.AsQueryable();  // Convert to IQueryable

            return View(pricingData.ToPagedList(pageNumber, pageSize)); // Use ToPagedList from X.PagedList
        }

public IActionResult HoaDonDetails(string searchQuery)
{
    var chiTietHoaDons = string.IsNullOrEmpty(searchQuery) 
        ? _context.ChiTietHoaDons.ToList()
        : _context.ChiTietHoaDons
            .Where(c => c.IdhoaDon.ToString().Contains(searchQuery))
            .ToList();

    ViewData["SearchQuery"] = searchQuery; // Pass the search query to the view
    return View(chiTietHoaDons);
}


//namespace WebQuanLyNhaKhoa.Controllers.HomePageCustomer
//{
//    public class ServicesController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public ServicesController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

    }
}
