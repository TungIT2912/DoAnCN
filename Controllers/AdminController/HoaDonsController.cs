using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.DTO;

namespace WebQuanLyNhaKhoa.Controllers.AdminController
{
    [Route("Admin/[controller]")]
    public class HoaDonsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HoaDonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
           return View();
        }
        [HttpGet("api/HoaDons")]
        public async Task<ActionResult<IEnumerable<HoaDonDTO>>> GetHoaDon(int pageNumber = 1, int pageSize = 10)
        {
            var totalItems = await _context.HoaDons.CountAsync();
            var ls = await _context.HoaDons
                    .Include(nv => nv.DanhSachKham)
                    .Include(nv => nv.DanhSachKham.BenhNhan)
                    .Include(nv => nv.DieuTri)
                    .Include(nv => nv.DonThuoc)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize).ToListAsync();
            var hdDTOs = ls.Select(nv => new HoaDonDTO
            {
               Idkham = nv.Idkham,
               tenBn = nv.DanhSachKham.BenhNhan.HoTen,
               PhuongThucThanhToan = nv.PhuongThucThanhToan,
               TienThuoc = nv.TienThuoc,
               TienDieuTri = nv.TienDieuTri,
               TongTien = nv.TongTien,
               NgayLap =   nv.NgayLap,
            }).ToList();

            return Ok(new { data = hdDTOs, totalItems });
        }

        private bool HoaDonExists(int id)
        {
            return _context.HoaDons.Any(e => e.IdhoaDon == id);
        }
    }
}
