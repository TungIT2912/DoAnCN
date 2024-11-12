using WebQuanLyNhaKhoa.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace WebQuanLyNhaKhoa.Controllers.AdminController
{
    [Route("Admin/[controller]")]
    public class Statics : Controller
    {
        ApplicationDbContext _context;
        public Statics(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var totalRevenue1 =  _context.HoaDons 
                .SumAsync(o => o.TongTien);
            var userCount =  _context.BenhNhans.CountAsync();
            ViewData["TotalRevenue1"] = totalRevenue1;
            ViewData["UserCount"] = userCount;
            return View();
        }
        [HttpGet("api/Statics/{year}")]
        public async Task<IActionResult> GetRevenue(int year)
        {
            if (year < 1900 || year > 2100)
            {
                return BadRequest("Năm không hợp lệ.");
            }

            DateTime startDate = new DateTime(year, 1, 1);
            DateTime endDate = startDate.AddYears(1).AddDays(-1);

            var invoices = await _context.HoaDons
                .Where(h => h.NgayLap >= startDate && h.NgayLap <= endDate)
                .ToListAsync();

            decimal[] revenuePerMonth = new decimal[12];
            decimal totalRevenue = 0;

            foreach (var invoice in invoices)
            {
                int monthIndex = invoice.NgayLap.Month - 1;
                revenuePerMonth[monthIndex] += invoice.TongTien;
                totalRevenue += invoice.TongTien;
            }

            var revenueData = revenuePerMonth.Select((value, index) =>
            {
                string label;
                decimal formattedValue;

                if (value < 1000000)
                {
                    formattedValue = value / 1000; // Convert to thousands
                    label = $"Tháng {index + 1} ngàn";
                }
                else
                {
                    formattedValue = value / 1000000; // Convert to millions
                    label = $"Tháng {index + 1} triệu";
                }

                return new { Value = formattedValue, Label = label };
            }).ToArray();

            return Ok(new
            {
                Year = year,
                RevenueData = revenueData,
                TotalRevenue = totalRevenue,
                UserCount = _context.BenhNhans.Count()
            });
        }

    }
}
