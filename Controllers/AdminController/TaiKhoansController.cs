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
        public async Task<ActionResult<IEnumerable<TaiKhoanDTO>>> GetTaiKhoans()
        {
            var taikhoans = await _context.TaiKhoans
                            .Include(nv => nv.NhanVien)
                            .Include(nv=>nv.NhanVien.ChucVu).ToListAsync();
            var taikhoanDTOs = taikhoans.Select(nv => new TaiKhoanDTO
            {
                Id = nv.Id,
                TenDangNhap = nv.TenDangNhap,
                ChucVu = nv.NhanVien.ChucVu.TenCv,
                Role = nv.NhanVien.TaiKhoan.Role,
                isLoocked = nv.isLoocked,
            }).ToList();

            return Ok(taikhoanDTOs);
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
