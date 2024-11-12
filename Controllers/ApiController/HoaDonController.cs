using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQuanLyNhaKhoa.Data;

namespace WebQuanLyNhaKhoa.Controllers.ApiController
{
    
    public class HoaDonController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HoaDonController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> ThanhToan(int hoaDonId)
        {
            // Kiểm tra `hoaDonId` có hợp lệ không
            if (hoaDonId <= 0)
            {
                return BadRequest("Invalid HoaDonId");
            }

            // Lấy hóa đơn từ database
            var hoaDon = await _context.HoaDons
        .Include(h => h.DieuTri)
        .ThenInclude(d => d.DichVu)
        .Include(h => h.DieuTri.DanhSachKham)
        .Include(h=> h.DieuTri.DanhSachKham.BenhNhan)
        .FirstOrDefaultAsync(h => h.IdhoaDon == hoaDonId);

            if (hoaDon == null)
            {
                return NotFound("Invoice not found.");
            }
            ViewBag.TenDichVu = hoaDon.DieuTri?.DichVu?.TenDichVu;
            ViewBag.TenBenhNhan = hoaDon.DieuTri?.DanhSachKham?.BenhNhan?.HoTen;
            ViewBag.SoDienThoaiBenhNhan = hoaDon.DieuTri?.DanhSachKham?.BenhNhan?.Sdt;
            return View(hoaDon);
        }

        //[HttpPost("api/PostThanhToan")]
        //public async Task<ActionResult> ThanhToanHoaDon(int hoaDonId)
        //{
        //    var hoaDon = await _context.HoaDons.Include(h => h.DieuTri).FirstOrDefaultAsync(h => h.IdhoaDon == hoaDonId);

        //    if (hoaDon == null)
        //    {
        //        return NotFound("Invoice not found.");
        //    }

        //    // Cập nhật trạng thái thanh toán là đã thanh toán
        //    hoaDon.DaThanhToan = true;
        //    hoaDon.PhuongThucThanhToan = "COD"; // Cập nhật phương thức thanh toán
        //    hoaDon.TongTien = hoaDon.TienDieuTri + hoaDon.TienThuoc; // Tính lại tổng tiền nếu cần
        //    hoaDon.NgayLap = DateTime.Now;

        //    _context.HoaDons.Update(hoaDon);
        //    await _context.SaveChangesAsync();

        //    return Ok("Payment successful. Invoice has been paid.");
        //}
        [HttpPost]
        public async Task<IActionResult> ThanhToanHoaDon(int hoaDonId)
        {
            var hoaDon = await _context.HoaDons.FindAsync(hoaDonId);
            if (hoaDon == null)
            {
                return NotFound(new { message = "Invoice not found" });
            }

            // Giả sử bạn đã thêm trường trạng thái thanh toán trong lớp HoaDon
            hoaDon.DaThanhToan = true; // Cập nhật trạng thái thanh toán thành "Paid"
            hoaDon.PhuongThucThanhToan = "COD"; // Cập nhật phương thức thanh toán
            hoaDon.TongTien = hoaDon.TienDieuTri + hoaDon.TienThuoc; // Tính lại tổng tiền nếu cần
            hoaDon.NgayLap = DateTime.Now;
            _context.HoaDons.Update(hoaDon);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Payment successful", hoaDonId = hoaDon.IdhoaDon });
        }
        [HttpPost]
        public async Task<IActionResult> ThanhToanApi(int hoaDonId)
        {
            // Kiểm tra tính hợp lệ của `hoaDonId`
            if (hoaDonId <= 0)
            {
                return BadRequest("Invalid HoaDonId");
            }

            // Lấy hóa đơn từ database
            var hoaDon = await _context.HoaDons.FindAsync(hoaDonId);
            if (hoaDon == null)
            {
                return NotFound("Invoice not found.");
            }

            // Giả sử bạn cập nhật trạng thái thanh toán (ví dụ: Paid)
            hoaDon.DaThanhToan = true; // Cập nhật trạng thái thanh toán thành "Paid"
            hoaDon.PhuongThucThanhToan = "COD"; // Cập nhật phương thức thanh toán
            hoaDon.TongTien = hoaDon.TienDieuTri + hoaDon.TienThuoc; // Tính lại tổng tiền nếu cần
            hoaDon.NgayLap = DateTime.Now;
            _context.HoaDons.Update(hoaDon);
            await _context.SaveChangesAsync();

            return Ok("Thanh toán thành công!");
        }

    }
}
