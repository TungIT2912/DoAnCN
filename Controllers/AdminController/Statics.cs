using WebQuanLyNhaKhoa.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace WebQuanLyNhaKhoa.Controllers.AdminController
{
    public class Statics : Controller
    {
        QlnhaKhoaContext _context;
        public Statics(QlnhaKhoaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Hành động này để tạo view
            return View();
        }

        [HttpPost]
        public IActionResult Index(int year)
        {
            // Hành động này để xử lý dữ liệu
            DateTime startDate;
            DateTime endDate;
            DateTime selectedYear;
            if (year < 1900 || year > 2100)
            {
                ViewBag.ErrorMessage = "Năm không hợp lệ.";
                return View();
            }
           
            startDate = new DateTime(year, 1, 1);
            endDate = startDate.AddYears(1).AddDays(-1); // Kết thúc vào cuối năm đã chọn
          
            var invoices = _context.HoaDons
                .Where(h => h.NgayLap >= startDate && h.NgayLap <= endDate)
                .ToList();

            // Tạo mảng để lưu trữ tổng tiền của từng tháng
            decimal[] revenuePerMonth = new decimal[12];

            // Tính toán tổng tiền của các hóa đơn trong từng tháng
            foreach (var invoice in invoices)
            {
                int monthIndex = invoice.NgayLap.Month - 1; // Chỉ số tháng trong mảng (từ 0 đến 11)
                revenuePerMonth[monthIndex] += invoice.TongTien; // Cộng tổng tiền của hóa đơn vào tháng tương ứng
            }

            var revenueData = revenuePerMonth.Select((value, index) =>
            {
                string label;
                decimal formattedValue;

                if (value < 1000000)
                {
                    formattedValue = value / 1000; // Chuyển đổi sang ngàn
                    label = $"Tháng {index + 1} ngàn";
                }
                else
                {
                    formattedValue = value / 1000000; // Chuyển đổi sang triệu
                    label = $"Tháng {index + 1} triệu";
                }

                return new { Value = formattedValue, Label = label };
            }).ToArray();

            ViewBag.RevenueData = revenueData;

            // Hiển thị biểu đồ
            return View();
        }
    }
}
