﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebQuanLyNhaKhoa.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace WebQuanLyNhaKhoa.Controllers.ApiConrtroller
{
    [Route("[controller]")]
    public class DonThuocController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonThuocController(ApplicationDbContext context)
        {
            _context = context;
        }

      
        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View(); // Renders Create.cshtml view for input form
        }
        [HttpGet("api/GetDonThuoc")]
        public async Task<ActionResult<IEnumerable<DonThuoc1DTO>>> GetAllDonThuoc()
        {
            try
            {
                var donThuocs = await _context.DonThuocs
                    .Include(d => d.Kho)
                    .ThenInclude(k => k.ThiTruong)
                    .Include(d => d.ChiTietHoaDon)
                    .Select(d => new DonThuoc1DTO
                    {
                        IdhoaDon = (int)(d.ChiTietHoaDon != null ? d.ChiTietHoaDon.IdhoaDon : 0),
                        Idkham = d.Idkham,
                        tenThuoc = d.Kho.ThiTruong.TenSanPham ?? "Không có tên thuốc",
                        ThanhGia = d.ThanhGia,
                        TongTien = d.TongTien,
                        IddungCu = d.IddungCu,
                        SoLuong = d.SoLuong,
                        NgayLapDt = d.NgayLapDt
                    })
                    .ToListAsync();

                return Ok(donThuocs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi tải dữ liệu: {ex.Message}");
            }
        }



        [HttpGet("api/GetDanhSachKhamOptions")]
        public async Task<ActionResult<IEnumerable<OptionDTO>>> GetDanhSachKhamOptions()
        {
            var options = await _context.DanhSachKhams
                .Select(ds => new OptionDTO { Id = ds.Idkham, Name = ds.BenhNhan.HoTen })
                .ToListAsync();
            return Ok(options);
        }

        // Fetch tools
        [HttpGet("api/GetDungCuOptions")]
        public async Task<ActionResult<IEnumerable<OptionDTO>>> GetDungCuOptions()
        {
            var options = await _context.Khos
                .Select(dc => new OptionDTO { Id = dc.IddungCu, Name = dc.ThiTruong.TenSanPham })
                .ToListAsync();
            return Ok(options);
        }
        [HttpGet("Create")]
        public IActionResult Create()
        {
                     
            return View(); 
        }

        [HttpPost("api/PostDonThuoc")]
        public async Task<ActionResult<DonThuocDTO>> CreateDonThuocAndUpdateInvoice([FromBody] DonThuocDTO newDonThuocDto)
        {
            var existingHoaDon = await _context.HoaDons
                    .FirstOrDefaultAsync(h => h.Idkham == newDonThuocDto.Idkham && h.DaThanhToan == false);
            //var existingCTHD = await _context.ChiTietHoaDons    
            //       .FirstOrDefaultAsync(h => h.Idkham == newDonThuocDto.Idkham);
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Kiểm tra thông tin bệnh nhân
                var danhSachKham = await _context.DanhSachKhams.FindAsync(newDonThuocDto.Idkham);
                var sdt = _context.BenhNhans
                    .Where(bn => bn.IdbenhNhan == newDonThuocDto.Idkham)
                    .Select(bn => bn.Sdt)
                    .FirstOrDefault();
                var phuongThucThanhToan = _context.HoaDons
                    .Where(bn => bn.IddonThuoc == newDonThuocDto.IddonThuoc)
                    .Select(bn => bn.PhuongThucThanhToan)
                    .FirstOrDefault();
                var email = _context.BenhNhans
                    .Where(bn => bn.IdbenhNhan == newDonThuocDto.Idkham)
                    .Select(bn => bn.EmailBn)
                    .FirstOrDefault();
                if (danhSachKham == null)
                {
                    return BadRequest("Bệnh nhân không hợp lệ.");
                }

                decimal totalMedicationCost = 0;  
                for (int i = 0; i < newDonThuocDto.IddungCu.Count; i++)
                {
                    var dungCu = await _context.Khos
                        .Include(k => k.ThiTruong) // Load dữ liệu từ ThiTruong liên quan
                        .FirstOrDefaultAsync(k => k.IddungCu == newDonThuocDto.IddungCu[i]);
                    if (dungCu == null)
                    {
                        return BadRequest($"Dụng cụ với ID {newDonThuocDto.IddungCu[i]} không hợp lệ.");
                    }

                    // Kiểm tra số lượng dụng cụ trong kho
                    int soLuong = newDonThuocDto.SoLuong[i];
                    if (dungCu.SoLuong < soLuong)
                    {
                        return BadRequest($"Không đủ số lượng dụng cụ {dungCu.ThiTruong.TenSanPham} trong kho.");
                    }

                    // Tính toán chi phí thuốc cho từng dụng cụ
                    decimal medicationCost = dungCu.ThiTruong.DonGia * soLuong;
                    totalMedicationCost += medicationCost;

                    // Tạo đơn thuốc cho từng dụng cụ
                    var newDonThuoc = new DonThuoc
                    {
                        Idkham = newDonThuocDto.Idkham,
                        IddungCu = newDonThuocDto.IddungCu[i],
                        SoLuong = soLuong,
                        ThanhGia = dungCu.ThiTruong.DonGia,
                        TongTien = medicationCost,
                        NgayLapDt = DateTime.Now,
                    };

                    // Thêm đơn thuốc vào cơ sở dữ liệu
                    _context.DonThuocs.Add(newDonThuoc);
                    await _context.SaveChangesAsync();

                    // Cập nhật số lượng dụng cụ trong kho
                    dungCu.SoLuong -= soLuong;
                    _context.Khos.Update(dungCu);
                    await _context.SaveChangesAsync();



                    int hoaDonId;
                    if (existingHoaDon != null)
                    {
                        existingHoaDon.IddonThuoc = newDonThuoc.IddonThuoc;
                        existingHoaDon.TienThuoc += totalMedicationCost;
                        existingHoaDon.TongTien += totalMedicationCost;
                        _context.HoaDons.Update(existingHoaDon);
                        await _context.SaveChangesAsync();
                        hoaDonId = existingHoaDon.IdhoaDon;
                    }
                    else
                    {
                        var newHoaDon = new HoaDon
                        {
                            Idkham = newDonThuoc.Idkham,
                            IddonThuoc = newDonThuoc.IddonThuoc,
                            PhuongThucThanhToan = "Chưa có",
                            TienDieuTri = 0,
                            TienThuoc = medicationCost,
                            TongTien = medicationCost,
                            NgayLap = DateTime.Now,
                            EmailBn = email
                        };
                        _context.HoaDons.Add(newHoaDon);
                        await _context.SaveChangesAsync();
                        hoaDonId = newHoaDon.IdhoaDon;
                        var benhNhan = await _context.BenhNhans.FirstOrDefaultAsync(bn => bn.EmailBn == newHoaDon.EmailBn);
                        if (benhNhan != null)
                        {
                            // Cập nhật IdHoaDon trong BenhNhan
                            benhNhan.HoaDon = newHoaDon;

                            // Lưu thay đổi
                            await _context.SaveChangesAsync();
                        }
                    }


                    var existingChiTietHoaDon = await _context.ChiTietHoaDons
                    .FirstOrDefaultAsync(c => c.Idkham == newDonThuocDto.Idkham);
                    if (existingChiTietHoaDon != null)
                    {
                        // Cập nhật ChiTietHoaDon hiện có
                        existingChiTietHoaDon.TienThuoc += totalMedicationCost;
                        existingChiTietHoaDon.TongTien += totalMedicationCost;
                        existingChiTietHoaDon.IddonThuoc = newDonThuoc.IddonThuoc;
                        existingChiTietHoaDon.IdhoaDon = hoaDonId;
                        existingChiTietHoaDon.NgayLap = DateTime.Now;
                        _context.ChiTietHoaDons.Update(existingChiTietHoaDon);
                        await _context.SaveChangesAsync();
                        // Cập nhật ChiTietHoaDonId cho DieuTri
                        newDonThuoc.ChiTietHoaDonId = existingChiTietHoaDon.IdchiTiet;
                        _context.DonThuocs.Update(newDonThuoc);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        // Create new ChiTietHoaDon if it doesn't exist
                        var newChiTietHoaDon = new ChiTietHoaDon
                        {
                            IdhoaDon = hoaDonId,
                            IddonThuoc = newDonThuoc.IddonThuoc,
                            Idkham = newDonThuoc.Idkham,
                            PhuongThucThanhToan = "Chưa có",
                            TenDon = "Khám Nha Khoa",  // Assign as necessary, or fetch dynamically
                             // Assuming this is the name you want
                            Description = "Description here", // Assign based on your needs
                            TienThuoc = totalMedicationCost,
                            TongTien = totalMedicationCost,
                            NgayLap = DateTime.Now,
                            EmailBn = email, // Assign as necessary, or leave null if not needed
                            Sdt = sdt
                        };
                        _context.ChiTietHoaDons.Add(newChiTietHoaDon);
                        await _context.SaveChangesAsync();
                        newDonThuoc.ChiTietHoaDonId = newChiTietHoaDon.IdchiTiet;
                        _context.DonThuocs.Update(newDonThuoc);
                        await _context.SaveChangesAsync();

                    }

                    totalMedicationCost = 0;
                    newDonThuocDto.hoaDonId = hoaDonId;
                }

                // Cập nhật thông tin hóa đơn (nếu có)
                

                // Commit transaction
                await transaction.CommitAsync();

                //newDonThuocDto.hoaDonId = newHoaDon.IdhoaDon;
                
                return CreatedAtAction(nameof(CreateDonThuocAndUpdateInvoice), new { id = newDonThuocDto.Idkham }, newDonThuocDto);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Có lỗi xảy ra khi tạo đơn thuốc: {ex.Message}");
            }
        }


        // Add this method in DonThuocController
        [HttpDelete("api/DeleteDonThuoc/{id}")]
        public async Task<IActionResult> DeleteDonThuoc(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Fetch the DonThuoc by id
                var donThuoc = await _context.DonThuocs.Include(d => d.Kho).FirstOrDefaultAsync(d => d.IddonThuoc == id);

                if (donThuoc == null)
                {
                    return NotFound("Prescription not found.");
                }

                // Step 1: Update stock quantity in Kho before deletion
                var dungCu = donThuoc.Kho;
                dungCu.SoLuong += donThuoc.SoLuong; // Add the quantity back to the stock
                _context.Khos.Update(dungCu);
                await _context.SaveChangesAsync();

                // Step 2: Delete the DonThuoc record
                _context.DonThuocs.Remove(donThuoc);
                await _context.SaveChangesAsync();

                // Step 3: Check if any HoaDon exists with this DonThuoc, and remove or update if necessary
                var hoaDon = await _context.HoaDons.FirstOrDefaultAsync(h => h.IddonThuoc == id);
                if (hoaDon != null)
                {
                    hoaDon.IddonThuoc = null; // Remove the reference to DonThuoc
                    hoaDon.TienThuoc = 0; // Reset the medication cost
                    hoaDon.TongTien -= donThuoc.TongTien; // Update total
                    _context.HoaDons.Update(hoaDon);
                    await _context.SaveChangesAsync();
                }

                // Commit transaction
                await transaction.CommitAsync();

                return Ok("Prescription deleted successfully and stock updated.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"An error occurred while deleting the prescription: {ex.Message}");
            }
        }

    }
}
