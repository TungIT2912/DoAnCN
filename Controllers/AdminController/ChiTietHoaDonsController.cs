﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.DTO;

namespace WebQuanLyNhaKhoa.Controllers.AdminController
{
    [Route("Admin/[controller]")]
    //[Authorize(Roles = "Admin")]
    public class ChiTietHoaDonsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChiTietHoaDonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ChiTietHoaDons
        public async Task<IActionResult> Index()
        {

            return View();
        }

       [HttpGet("api/ChiTietHoaDons/Detail/{id}")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cthd = await _context.ChiTietHoaDons
                    .Include(n => n.DonThuocs)
                    .Include(n => n.DieuTris)
                    .Include(n => n.HoaDon)
                    .Include(n => n.DanhSachKham)
                    .Include(n => n.DanhSachKham.BenhNhan)
                    .Include(n => n.HoaDon.DonThuoc.Kho.ThiTruong)
                    .Include(n => n.HoaDon.DieuTri.DichVu)
                    .FirstOrDefaultAsync(n => n.IdchiTiet == id);
        if (cthd == null)
        {
            Console.WriteLine($"Mã chi tiết hóa đơn không tồn tại.");
            return NotFound();
        }

        var cthdDTO = new ChiTietHoaDonDTO
        {
            IdchiTiet = cthd.IdchiTiet,
            tenBn = cthd.DanhSachKham.BenhNhan.HoTen,
            Idkham = cthd.Idkham,
            IdhoaDon = cthd.IdhoaDon,
            PhuongThucThanhToan = cthd.PhuongThucThanhToan,
            TenDon = cthd.TenDon,
            TenDieuTri = cthd.TenDieuTri,
            Description = cthd.Description,
            TienThuoc = cthd.TienThuoc,
            TienDieuTri = cthd.TienDieuTri,
            TongTien = cthd.TongTien,
            NgayLap = cthd.NgayLap,
            EmailBn = cthd.EmailBn,
            Sdt = cthd.DanhSachKham.BenhNhan.Sdt,

            DonThuocs = cthd.DonThuocs.Select(dt => new DonThuoc1DTO
            {
                Idkham = dt.Idkham,
                IddungCu = dt.IddungCu,
                tenThuoc = dt.Kho.ThiTruong.TenSanPham ?? "Không tìm thấy",
                SoLuong = dt.SoLuong,
                ThanhGia = dt.ThanhGia,
                TongTien = dt.TongTien,
                NgayLapDt = dt.NgayLapDt
            }).ToList(),

            DieuTris = cthd.DieuTris.Select(dt => new DieuTriDTO
            {
                IddichVu = dt.IddichVu,
                tenDieuTri = dt.DichVu.TenDichVu,
                Idkham = dt.Idkham,
                IddungCu = dt.IddungCu,
                SoLuong = dt.SoLuong,
                ThanhTien = dt.ThanhTien
            }).ToList()
        };

    return View(cthdDTO); 
}


        private bool ChiTietHoaDonExists(int id)
        {
            return _context.ChiTietHoaDons.Any(e => e.IdchiTiet == id);
        }
    }
}
