using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.DTO;

namespace WebQuanLyNhaKhoa.Controllers.AdminController
{
    [Route("Admin/[controller]")]
    public class ChangeShiftController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ChangeShiftController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }

        [HttpGet("api/ListShift")]
        public async Task<ActionResult> GetChanges(int pageNumber = 1, int pageSize = 10)
        {
            var totalItems = await _context.RegisForms.CountAsync();

            var nhanViens = await _context.RegisForms
                .Include(nv => nv.NhanVien)
                .Include(nv => nv.NhanVien.ChucVu)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var nhanVienDTOs = nhanViens.Select(nv => new RegisFormDTO
            {
                MaNv = nv.MaNv,
                Ten = nv.NhanVien.Ten,
                ChucVu = nv.NhanVien.ChucVu.TenCv,
                CreateDay = nv.CreateDay,
                Status = nv.Status,
            }).ToList();

            return Ok(new { data = nhanVienDTOs, totalItems });
        }
    }
}
