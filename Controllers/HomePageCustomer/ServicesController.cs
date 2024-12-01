﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQuanLyNhaKhoa.Data;
using X.PagedList;
using X.PagedList.Extensions;


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
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 6;

            // Retrieve paginated DichVu data
            var dichVus = await _context.DichVus
                .OrderBy(dv => dv.IddichVu)  // Optional: Order by specific field
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Retrieve total count for pagination
            var totalCount = await _context.DichVus.CountAsync();

            // Create a PagedList for the DichVu data
            var pagedList = new StaticPagedList<DichVu>(dichVus, page, pageSize, totalCount);

            // Pass the paged list to the view
            return View(pagedList);
        }

        // Pricing page with pagination
        public ActionResult Pricing(int? page)
        {
            int pageSize = 4;  
            int pageNumber = (page ?? 1);  // Default to page 1 if no page parameter

            // Use AsQueryable() to ensure that ToPagedList can be applied
            var pricingData = _context.DichVus.AsQueryable();  // Convert to IQueryable

            return View(pricingData.ToPagedList(pageNumber, pageSize)); // Use ToPagedList from X.PagedList
        }

        // HoaDonDetails search page
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

        // Service Details page
        public IActionResult ServicesDetail(int id)
        {
            // Step 1: Fetch the service details based on the ID
            var service = _context.DichVus.FirstOrDefault(s => s.IddichVu == id);

            if (service == null)
            {
                return NotFound(); // Handle cases where the service isn't found
            }

            // Step 2: Fetch the list of doctors associated with the service
            // Assuming doctors are filtered based on their ChucVu (Role) or another field
            var doctors = _context.NhanViens
                .Where(d => d.ChucVu.TenCv == "Doctor")  // Adjust this filter to fit your needs
                .ToList();

            // Step 3: Create and populate the ViewModel
            var viewModel = new ServiceDetailViewModel
            {
                Service = service,
                Doctors = doctors
            };

            // Step 4: Pass the ViewModel to the view
            return View(viewModel);
        }
    }
}
