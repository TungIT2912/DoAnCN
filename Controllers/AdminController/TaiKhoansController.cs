using System;
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
    public class TaiKhoansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaiKhoansController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

       [HttpGet("api/TaiKhoans")]
public async Task<ActionResult<IEnumerable<TaiKhoanDTO>>> GetTaiKhoans(int pageNumber = 1, int pageSize = 10)
{
    try
    {
        var totalItems = await _context.TaiKhoans.CountAsync();
        var taikhoans = await _context.TaiKhoans
                            .Include(nv => nv.NhanVien)
                            .Include(nv => nv.NhanVien.ChucVu)  // Ensure ChucVu is included
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

        var taikhoanDTOs = taikhoans.Select(nv => new TaiKhoanDTO
        {
            Id = nv.Id,
            TenDangNhap = nv.TenDangNhap,
            // Null checks to avoid exceptions if related data is missing
            ChucVu = nv.NhanVien?.ChucVu?.TenCv ?? "Chưa có chức vụ",  // Default value if ChucVu is null
            Role = nv.NhanVien?.TaiKhoan?.Role ?? "Chưa có role",  // Default value if TaiKhoan is null
            isLoocked = nv.isLoocked ?? false  // Default value if isLoocked is null
        }).ToList();

        return Ok(new { data = taikhoanDTOs, totalItems });
    }
    catch (Exception ex)
    {
        // Log the exception details (make sure to use a logger)
        return StatusCode(500, new { message = "Internal Server Error", details = ex.Message });
    }
}


        [HttpPut("api/TaiKhoans/Locked/{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] TaiKhoanDTO dto)
        {
            if (dto == null || !dto.isLoocked.HasValue)
            {
                return BadRequest("Invalid request data.");
            }

            var taikhoan = await _context.TaiKhoans.FindAsync(id);

            if (taikhoan == null)
            {
                return NotFound();
            }

            taikhoan.isLoocked = dto.isLoocked.Value;

            await _context.SaveChangesAsync();

            return Ok(new { message = $"Tài khoản đã được {(dto.isLoocked.Value ? "chặn" : "bỏ chặn")} thành công!" });
        }

        private bool TaiKhoanExists(int id)
        {
            return _context.TaiKhoans.Any(e => e.Id == id);
        }
    }
}
