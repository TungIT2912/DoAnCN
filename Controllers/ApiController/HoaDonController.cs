

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.Models;
using WebQuanLyNhaKhoa.ServicesPay;

namespace WebQuanLyNhaKhoa.Controllers.ApiController
{
    
    public class HoaDonController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IVnPayService _vnPayService;
        private readonly EmailService _emailService;

        public HoaDonController(ApplicationDbContext context, IConfiguration configuration, IVnPayService vnPayService, EmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _vnPayService = vnPayService;
            _emailService = emailService;
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
                .Include(h => h.DieuTri.DanhSachKham.BenhNhan)
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

        [HttpPost]
        public async Task<IActionResult> ThanhToanApi(int hoaDonId, string paymentMethod)
        {
            // Kiểm tra tính hợp lệ của `hoaDonId`
            if (hoaDonId <= 0)
            {
                return BadRequest("Invalid HoaDonId");
            }

            // Lấy hóa đơn từ database
            var hoaDon = await _context.HoaDons
                .Include(h => h.DieuTri)
                .ThenInclude(d => d.DichVu)
                .Include(h => h.DieuTri.DanhSachKham)
                .Include(h => h.DieuTri.DanhSachKham.BenhNhan)
                .FirstOrDefaultAsync(h => h.IdhoaDon == hoaDonId);
            if (hoaDon == null)
            {
                return NotFound("Invoice not found.");
            }


            hoaDon.TongTien = hoaDon.TienDieuTri + hoaDon.TienThuoc;

            //    // Cập nhật trạng thái thanh toán là đã thanh toán
            //    hoaDon.DaThanhToan = true;
            //    hoaDon.PhuongThucThanhToan = "COD"; // Cập nhật phương thức thanh toán
            //    hoaDon.TongTien = hoaDon.TienDieuTri + hoaDon.TienThuoc; // Tính lại tổng tiền nếu cần
            //    hoaDon.NgayLap = DateTime.Now;

            //    _context.HoaDons.Update(hoaDon);
            //    await _context.SaveChangesAsync();
            using var transaction = await _context.Database.BeginTransactionAsync();
            if (paymentMethod == "COD")
        {
                hoaDon.DaThanhToan = true; 
                hoaDon.PhuongThucThanhToan = "COD";
                _context.HoaDons.Update(hoaDon);
                var chiTietHoaDon = await _context.ChiTietHoaDons
                    .FirstOrDefaultAsync(ct =>
                        ct.IdhoaDon == hoaDon.IdhoaDon &&
                        (ct.IddieuTri == hoaDon.IddieuTri || ct.IddonThuoc == hoaDon.IddonThuoc)
                    );

                if (chiTietHoaDon != null) { 
                    chiTietHoaDon.PhuongThucThanhToan = "COD"; 
                    _context.ChiTietHoaDons.Update(chiTietHoaDon); 
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                var currentTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                var patientEmail = hoaDon.EmailBn; // Replace with the patient's email
                var emailSubject = "Thanh toán hóa đơn";
                var emailMessage = $"Bạn đã thanh toán thành công tiền viện phí.<br><br> " +
                                    $"Bạn đã thanh toán thành công tiền viện phí vào lúc {currentTime}.<br><br>" +
                                    $"Tổng tiền cần thanh toán là {hoaDon.TongTien.ToString("C", new CultureInfo("vi-VN"))}<br><br>" +
                                    $"Thông tin chi tiết:<br><br>" +
                                    $"Số tiền điều trị: {hoaDon.TienDieuTri.ToString("C", new CultureInfo("vi-VN"))}.<br><br> " +
                                    $"Số tiền thuốc: {hoaDon.TienThuoc.ToString("C", new CultureInfo("vi-VN"))}.<br><br> ";
                await _emailService.SendEmailAsync(patientEmail, emailSubject, emailMessage);
                return Ok("Thanh toán thành công!");
            }
            else if (paymentMethod == "VNPay")
            {

                // Khởi tạo yêu cầu thanh toán VNPay
                var paymentRequest = new VnPaymentRequestModel
                {
                    Amount = (double)hoaDon.TongTien,
                    OrderId = hoaDonId,
                    CreateDate = DateTime.Now
                };
                hoaDon.DaThanhToan = true;
                hoaDon.PhuongThucThanhToan = "VNPay";
                var chiTietHoaDon = await _context.ChiTietHoaDons.FirstOrDefaultAsync(ct => ct.IdhoaDon == hoaDon.IdhoaDon && ct.IddieuTri == hoaDon.IddieuTri);
                if (chiTietHoaDon != null)
                {
                    chiTietHoaDon.PhuongThucThanhToan = "VNPay";
                    _context.ChiTietHoaDons.Update(chiTietHoaDon);
                }
                _context.HoaDons.Update(hoaDon);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                var currentTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                var patientEmail = hoaDon.DieuTri.DanhSachKham.BenhNhan.EmailBn; // Replace with the patient's email
                var emailSubject = "Thanh toán hóa đơn";
                var emailMessage = $"Bạn đã thanh toán thành công tiền viện phí.<br><br> " +
                                    $"Bạn đã thanh toán thành công tiền viện phí vào lúc {currentTime}.<br><br>" +
                                    $"Tổng tiền cần thanh toán là {hoaDon.TongTien.ToString("C", new CultureInfo("vi-VN"))}<br><br>" +
                                    $"Thông tin chi tiết:<br><br>" +
                                    $"Số tiền điều trị: {hoaDon.TienDieuTri.ToString("C", new CultureInfo("vi-VN"))}.<br><br> " +
                                    $"Số tiền thuốc: {hoaDon.TienThuoc.ToString("C", new CultureInfo("vi-VN"))}.<br><br> ";
                await _emailService.SendEmailAsync(patientEmail, emailSubject, emailMessage);

                var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, paymentRequest);
                //return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, paymentRequest));
                return Redirect(paymentUrl);
            }
            else
            {
                return BadRequest("Invalid payment method.");
            }
        }


        

        private static string HmacSHA512(string key, string input)
        {
            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
            }

        [HttpGet]
        public async Task<IActionResult> ThanhToanVNPayReturn()
            {
            var response = _vnPayService.PaymentExcute(Request.Query);

            if (response.Success)
            {
                // Tìm và cập nhật trạng thái thanh toán hóa đơn
                var hoaDon = await _context.HoaDons.FindAsync(int.Parse(response.OrderId));
                if (hoaDon != null)
                {
                    hoaDon.DaThanhToan = true;
                    hoaDon.PhuongThucThanhToan = "VNPay";
                    _context.HoaDons.Update(hoaDon);
                    await _context.SaveChangesAsync();
                }
                return View("ThanhToanThanhCong", response);
            }
            else
            {
                return View("ThanhToanThatBai");
            }
        }

    }
}
