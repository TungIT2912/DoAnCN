using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebQuanLyNhaKhoa.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace WebQuanLyNhaKhoa.Controllers.UserController
{
    public class HoaDonsController : Controller
    {
        private readonly QlnhaKhoaContext _context;
        private readonly EmailService _emailService;


        public HoaDonsController(QlnhaKhoaContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: HoaDons
        public async Task<IActionResult> Index()
        {
            var qlnhaKhoaContext = _context.HoaDons.Include(h => h.IddonThuocNavigation).Include(h => h.IddieuTriNavigation);
            return View(await qlnhaKhoaContext.ToListAsync());
        }

        // GET: HoaDons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons
                .Include(h => h.IddonThuocNavigation)
                .Include(h => h.IddieuTriNavigation)
                .FirstOrDefaultAsync(m => m.IdhoaDon == id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            return View(hoaDon);
        }

        // GET: HoaDons/Create
        public IActionResult Create()
        {
            ViewData["Idkham"] = new SelectList(_context.DanhSachKhams, "Idkham", "Idkham");
            return View();
        }

        // POST: LichSuNhapXuats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdhoaDon,IddonThuoc,IddieuTri,IDKham,PhuongThucThanhToan,TienThuoc,TienDieuTri,TongTien,NgayLap,EmailBn")] HoaDon hoaDon, DanhSachKham ds,String Id)
        {
          
            if (hoaDon.NgayLap < DateTime.Today)
            {
                ModelState.AddModelError("NgayLapDt", "Ngày không thể là một ngày trong quá khứ.");
            }

            // Kiểm tra xem ngày có nằm trong phạm vi cho phép của SQL Server không
            if (hoaDon.NgayLap < new DateTime(1753, 1, 1) || hoaDon.NgayLap > new DateTime(9999, 12, 31))
            {
                ModelState.AddModelError("NgayLapDt", "Ngày không hợp lệ.");
            }
            var DsExists = await _context.DanhSachKhams.AnyAsync(k => k.Idkham == Id);
            if (DsExists)
            {
                decimal s = 0;
                var donThuocs = await _context.DonThuocs
                                .Where(d => d.Idkham==Id) 
                                .ToListAsync();
                foreach(var item in donThuocs)
                {
                    s += item.TongTien;
                }
                var IdDieutri= await _context.DieuTris.Where(k => k.Idkham == Id).Select(k => k.IddieuTri).FirstOrDefaultAsync();
                var IdDonThuoc = await _context.DonThuocs.Where(k => k.Idkham == Id).Select(k => k.IddonThuoc).FirstOrDefaultAsync();
                ViewData["Idkham"] = new SelectList(_context.DanhSachKhams, "Idkham", "Idkham", hoaDon.Idkham);
                hoaDon.Idkham = (Request.Form["Idkham"]).ToString();
                hoaDon.IddieuTri = IdDieutri;
                hoaDon.IddonThuoc= IdDonThuoc;
                var thuocExist = await _context.DonThuocs.AnyAsync(k => k.IddonThuoc == hoaDon.IddonThuoc);
                var dieuTriAmount = await _context.DieuTris.Where(k => k.IddieuTri == hoaDon.IddieuTri).Select(k => k.ThanhTien).FirstOrDefaultAsync();
                hoaDon.TienThuoc =s;
                hoaDon.TienDieuTri = dieuTriAmount;
                hoaDon.TongTien = s + dieuTriAmount;
                hoaDon.Idkham = Id;
                _context.HoaDons.Add(hoaDon);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(hoaDon);
        }

        // GET: HoaDons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon == null)
            {
                return NotFound();
            }
            ViewData["IddonThuoc"] = new SelectList(_context.DonThuocs, "IddonThuoc", "IddonThuoc", hoaDon.IddonThuoc);
            return View(hoaDon);
        }

        // POST: HoaDons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdhoaDon,IddonThuoc,IddieuTri,IDKham,PhuongThucThanhToan,TienThuoc,TienDieuTri,TongTien,NgayLap,EmailBn")] HoaDon hoaDon)
        {
            if (id != hoaDon.IdhoaDon)
            {
                return NotFound();
            }

            try
            {
               
                var existingHoadon = await _context.HoaDons
                                        .Include(p => p.IddieuTriNavigation)
                                        .FirstOrDefaultAsync(p => p.IdhoaDon == id);
                hoaDon.NgayLap = DateTime.Today;
                hoaDon.Idkham = existingHoadon.Idkham;
                hoaDon.IddonThuoc = existingHoadon.IddonThuoc;
                hoaDon.IddieuTri = existingHoadon.IddieuTri;
                hoaDon.TienThuoc = existingHoadon.TienThuoc;
                hoaDon.TienDieuTri = existingHoadon.TienDieuTri;
                hoaDon.TongTien = existingHoadon.TienDieuTri + existingHoadon.TienThuoc;
                decimal Tien = hoaDon.TongTien;
                decimal TienDT = hoaDon.TienDieuTri;
                decimal TienDonT = hoaDon.TienThuoc;
                var currentTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                //_context.Update(hoaDon);
                _context.Entry(existingHoadon).CurrentValues.SetValues(hoaDon);
                await _context.SaveChangesAsync();
                var patientEmail = hoaDon.EmailBn; // Replace with the patient's email
                var emailSubject = "Thanh toán hóa đơn";
                var emailMessage = $"Bạn đã thanh toán thành công tiền viện phí.<br><br> " +
                                    $"Bạn đã thanh toán thành công tiền viện phí vào lúc {currentTime}.<br><br>" +
                                    $"Tổng tiền cần thanh toán là {Tien.ToString("C", new CultureInfo("vi-VN"))}<br><br>" +
                                    $"Thông tin chi tiết:<br><br>" +
                                    $"Số tiền điều trị: {TienDT.ToString("C", new CultureInfo("vi-VN"))}.<br><br> " +
                                    $"Số tiền thuốc: {TienDonT.ToString("C", new CultureInfo("vi-VN"))}.<br><br> ";
                await _emailService.SendEmailAsync(patientEmail, emailSubject, emailMessage);

            }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoaDonExists(hoaDon.IdhoaDon))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                 return View("SuccessPayment",hoaDon.Idkham);

            ViewData["IddonThuoc"] = new SelectList(_context.DonThuocs, "IddonThuoc", "IddonThuoc", hoaDon.IddonThuoc);
            return View(hoaDon);
        }

        // GET: HoaDons/Delete/5
       

        private bool HoaDonExists(int id)
        {
            return _context.HoaDons.Any(e => e.IdhoaDon == id);
        }
    }
}
