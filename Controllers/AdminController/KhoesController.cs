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
    public class KhoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KhoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Khoes
        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }
        [HttpGet("api/Khoes")]
        public async Task<ActionResult<IEnumerable<KhoDTO>>> GetKhoes()
        {
            var khoes = await _context.Khos
        .Include(nv => nv.ThiTruong).ToListAsync();
            var khoDTOs = khoes.Select(nv => new KhoDTO
            {
                TenDungCu = nv.ThiTruong.TenSanPham, // Match the correct property name here
                Loai = nv.Loai,
                DonViTinh = nv.DonViTinh,
                SoLuong = nv.SoLuong
            }).ToList();

            return Ok(khoDTOs);
        }
    }
}
