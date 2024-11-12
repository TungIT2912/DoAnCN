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
        public async Task<ActionResult<IEnumerable<HoaDonDTO>>> GetHoaDon()
        {
            var ls = await _context.HoaDons
                    .Include(nv => nv.DanhSachKham)
                    .Include(nv => nv.DanhSachKham.BenhNhan)
                    .Include(nv => nv.DieuTri)
                    .Include(nv => nv.DonThuoc)
                    .ToListAsync();
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

            return Ok(hdDTOs);
        }

        private bool HoaDonExists(int id)
        {
            return _context.HoaDons.Any(e => e.IdhoaDon == id);
        }
    }
}
