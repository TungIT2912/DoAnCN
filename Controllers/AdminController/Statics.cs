﻿using WebQuanLyNhaKhoa.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace WebQuanLyNhaKhoa.Controllers.AdminController
{
    [Route("Admin/[controller]")]
    //[Authorize(Roles = "Admin")]
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
            int initialYear = 2024;
            DateTime startDate = new DateTime(initialYear, 1, 1);
            DateTime endDate = startDate.AddYears(1).AddDays(-1);

            var revenueData = _context.HoaDons
                .Where(h => h.NgayLap >= startDate && h.NgayLap <= endDate)
                .GroupBy(h => h.NgayLap.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalRevenue = g.Sum(h => h.TongTien)
                })
                .ToList();

            var revenuePerMonth = new decimal?[12];
            foreach (var data in revenueData)
            {
                revenuePerMonth[data.Month - 1] = data.TotalRevenue;
            }

            ViewData["InitialRevenueData"] = revenuePerMonth;
            ViewData["TotalRevenue1"] = revenuePerMonth.Sum() ?? 0;
            ViewData["UserCount"] = _context.BenhNhans.Count();
            ViewData["Years"] = initialYear;

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

            decimal?[] revenuePerMonth = new decimal?[12];
            decimal? totalRevenue = 0;
            var invoicesCopy = invoices.ToList();
            Console.WriteLine($"Invoices Count Before Loop: {invoices.Count}");
            foreach (var invoice in invoicesCopy)
            {
                int monthIndex = invoice.NgayLap.Month - 1;

                if (!revenuePerMonth[monthIndex].HasValue)
                {
                    revenuePerMonth[monthIndex] = 0;
                }
                revenuePerMonth[monthIndex] += invoice.TongTien;

                totalRevenue += invoice.TongTien;
            }

            var revenueData = revenuePerMonth.Select((value, index) =>
            {
                string label;
                decimal? formattedValue;
                decimal actualValue = value ?? 0;

                if (actualValue < 1000000)
                {
                    formattedValue = actualValue / 1000; 
                    label = $"Tháng {index + 1} ngàn";
                }
                else
                {
                    formattedValue = actualValue / 1000000; 
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
