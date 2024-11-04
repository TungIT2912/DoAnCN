using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebQuanLyNhaKhoa.Data;

using System.Drawing.Printing;
using WebQuanLyNhaKhoa.Models;
using X.PagedList;
using WebQuanLyNhaKhoa.DTO;
using Microsoft.AspNetCore.Identity;
namespace WebQuanLyNhaKhoa.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class NhanViensController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<NhanViensController> _logger;

        public NhanViensController(ILogger<NhanViensController> logger,
            RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _logger = logger;
            _roleManager = roleManager;
            _context = context;
        }
        // GET: NhanViens
        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }
        [HttpGet("api/NhanViens")]
        public async Task<ActionResult<IEnumerable<NhanVienDTO>>> GetNhanViens()
        {
            var nhanViens = await _context.NhanViens
       .Include(nv => nv.TaiKhoan) // Ensure ApplicationUser is loaded
       .ToListAsync();
            var nhanVienDTOs = nhanViens.Select(nv => new NhanVienDTO
            {
                Ten = nv.Ten,
                Sdt = nv.Sdt,
                MaCv = nv.MaCv,
                KinhNghiem = nv.KinhNghiem,
                Diachi = nv.Diachi,
                Hinh = nv.Hinh,
                Email = nv.Email,
                MatKhau = nv.TaiKhoan.MatKhau,
                Role = nv.TaiKhoan.Role
            }).ToList();

            return Ok(nhanVienDTOs);
        }
        // GET: NhanViens/Details/5
        public async Task<IActionResult> Details(int? id)
        {


            return View();
        }

        // GET: NhanViens/Create
        public IActionResult Create()
        {
            ViewData["MaCv"] = new SelectList(_context.ChucVus, "MaCv", "TenCv");
            return View();
        }
        [HttpPost("api/NhanViens")]
        public async Task<IActionResult> CreateNhanVien([FromBody] NhanVienDTO dto, IFormFile? hinh)
        {
            // Validate the model state
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { success = false, errors });
            }

            // Check if account already exists
            var existingAccount = await _context.TaiKhoans
                .FirstOrDefaultAsync(x => x.TenDangNhap == dto.Email);

            if (existingAccount != null)
            {
                return BadRequest(new { success = false, message = "Email already exists." });
            }

            // Check if ChucVu exists
            var existingChucVu = await _context.ChucVus.FindAsync(dto.MaCv);
            if (existingChucVu == null)
            {
                return NotFound(new { success = false, message = $"ChucVu with MaCv {dto.MaCv} not found." });
            }

            // Validate role
            var validRoles = new[] { "Admin", "Customer", "Staff" };

            // Use Any with a case-insensitive comparison
            if (!validRoles.Any(role => role.Equals(dto.Role, StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest(new { success = false, message = "Invalid role specified." });
            }

            // Create new TaiKhoan entity
            var user = new TaiKhoan
            {
                TenDangNhap = dto.Email,
                MatKhau = BCrypt.Net.BCrypt.HashPassword(dto.MatKhau),
                Role = dto.Role
            };

            try
            {
                // Attempt to add user to context and save changes
                await _context.TaiKhoans.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                // Log the exception (optional)
                // You can use logging here if you have set up a logger

                // Return a BadRequest with error messages
                return BadRequest(new { success = false, message = "Failed to create user.", details = dbEx.InnerException?.Message });
            }
            // Handle image upload
            if (hinh != null)
            {
                dto.Hinh = await SaveImage(hinh);
            }
            else
            {
                dto.Hinh = "/images/anonymous.png"; // Default image if none provided
            }

            // Create new NhanVien entity
            var newNhanVien = new NhanVien
            {
                Ten = dto.Ten,
                UserId = user.Id,
                Sdt = dto.Sdt,
                MaCv = dto.MaCv,
                KinhNghiem = dto.KinhNghiem,
                Hinh = dto.Hinh,
                Email = dto.Email,
            };

            // Add NhanVien to context and save changes
            _context.NhanViens.Add(newNhanVien);
            var nhanVienSaveResult = await _context.SaveChangesAsync();

            // Check if NhanVien was added successfully
            if (nhanVienSaveResult > 0)
            {
                // Return created response
                var createdNhanVienDTO = new NhanVienDTO
                {
                    Ten = newNhanVien.Ten,
                    Sdt = newNhanVien.Sdt,
                    MaCv = newNhanVien.MaCv,
                    KinhNghiem = newNhanVien.KinhNghiem,
                    Hinh = newNhanVien.Hinh,
                    Email = newNhanVien.Email
                };

                return CreatedAtAction(nameof(GetNhanVienById), new { id = newNhanVien.MaNv }, createdNhanVienDTO);
            }

            // If any save operation fails, return a BadRequest with an error message
            return BadRequest(new { success = false, message = "Failed to create NhanVien." });
        }



        private async Task<string> SaveImage(IFormFile hinh)
        {
            var savePath = Path.Combine("wwwroot/images", hinh.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await hinh.CopyToAsync(fileStream);
            }
            return "/images/" + hinh.FileName; // Return relative path
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<NhanVienDTO>> GetNhanVienById(int id)
        {
            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            var nhanVienDTO = new NhanVienDTO
            {
                Ten = nhanVien.Ten,
                Sdt = nhanVien.Sdt,
                MaCv = nhanVien.MaCv,
                KinhNghiem = nhanVien.KinhNghiem,
                Diachi = nhanVien.Diachi,
                Hinh = nhanVien.Hinh,
                Email = nhanVien.Email,
                MatKhau = nhanVien.TaiKhoan.MatKhau,
                Role = nhanVien.TaiKhoan.Role
            };
            return nhanVienDTO;
        }

        // GET: NhanViens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            ViewData["MaCv"] = new SelectList(_context.ChucVus, "MaCv", "TenCv", nhanVien.MaCv);
            //ViewData["TenDangNhap"] = new SelectList(_context.TaiKhoans, "TenDangNhap", "TenDangNhap", nhanVien.TenDangNhap);
            return View(nhanVien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaNv,Ten,Sdt,MaCv,KinhNghiem,Hinh")] NhanVien nhanVien, IFormFile Hinh)
        {
            ModelState.Remove("Hinh"); // Loại bỏ xác thực ModelState cho ImageUrl
            if (id != nhanVien.MaNv)
            {
                return NotFound();
            }

            var existingNV = _context.NhanViens.FirstOrDefault(n => n.MaNv == id); // Giả định có phương thức GetByIdAsync

            // Giữ nguyên thông tin hình ảnh nếu không có hình mới được tải lên
            if (Hinh == null)
            {
                existingNV.Hinh = existingNV.Hinh;
            }
            else
            {
                // Lưu hình ảnh mới
                existingNV.Hinh = await SaveImage(Hinh);
            }
            // Cập nhật các thông tin khác của sản phẩm
            //existingNV.TenDangNhap = nhanVien.TenDangNhap;
            existingNV.Ten = nhanVien.Ten;
            existingNV.Sdt = nhanVien.Sdt;
            existingNV.MaCv = nhanVien.MaCv;
            existingNV.KinhNghiem = nhanVien.KinhNghiem;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        // POST: NhanViens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien != null)
            {
                _context.NhanViens.Remove(nhanVien);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhanVienExists(int id)
        {
            return _context.NhanViens.Any(e => e.MaNv == id);
        }

    }
}
