using WebQuanLyNhaKhoa.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KhoVaThongKe.Controllers
{
    public class Statics : Controller
    {
        QlnhaKhoaContext _context;
        public Statics(QlnhaKhoaContext context)
        {
            _context = context;
        }
        public IActionResult Index(string year)
        {

            DateTime startDate;
            DateTime endDate;

            if (DateTime.TryParse(year, out DateTime selectedYear))
            {
                startDate = new DateTime(selectedYear.Year, 1, 1);
                endDate = startDate.AddYears(1).AddDays(-1); // Kết thúc vào cuối năm đã chọn
            }
            else
            {
                // Nếu chuỗi năm không hợp lệ, hiển thị thông báo lỗi và không tiếp tục xử lý
                ViewBag.ErrorMessage = "Năm không hợp lệ.";
                return View();
            }

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

            // Hiển thị biểu đồ
            return View(revenuePerMonth);
        }
    }
}
