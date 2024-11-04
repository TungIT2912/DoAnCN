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
using WebQuanLyNhaKhoa.Areas.Admin.API;
namespace WebQuanLyNhaKhoa.Controllers.HomepageAdmin
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class NhanViensController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<NhanViensController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public NhanViensController(ILogger<NhanViensController> logger, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
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
       .Include(nv => nv.ApplicationUser) // Ensure ApplicationUser is loaded
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
                MatKhau = nv.ApplicationUser.PasswordHash,
                Role = nv.ApplicationUser.ChucVu
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
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { success = false, errors });
            }


            var existingAccount = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (existingAccount != null)
            {
                return BadRequest(new { success = false, message = "Email already exists." });
            }

            var existingChucVu = await _context.ChucVus.FindAsync(dto.MaCv);
            if (existingChucVu == null)
            {
                return NotFound($"ChucVu with MaCv {dto.MaCv} not found.");
            }

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                FullName = dto.Ten,
                Email = dto.Email,
                ChucVu = dto.Role,
                Address = dto.Diachi
            };

            var result = await _userManager.CreateAsync(user, dto.MatKhau);
            if (result.Succeeded)
            {
                var roleName = dto.Role switch
                {
                    "admin" => "Admin",
                    "employee" => "Employee",
                    _ => null
                };

                if (roleName != null)
                {
                    var role = await _roleManager.FindByNameAsync(roleName);
                    if (role == null)
                    {
                        role = new IdentityRole(roleName);
                        await _roleManager.CreateAsync(role);
                    }
                }
                else
                {
                    return BadRequest("Invalid role specified.");
                }

                await _userManager.AddToRoleAsync(user, roleName);

                // Handle image upload
                if (hinh != null)
                {
                    dto.Hinh = await SaveImage(hinh);
                }
                else
                {
                    dto.Hinh = "/images/anonymous.png"; // Set default image if none uploaded
                }

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

                _context.NhanViens.Add(newNhanVien);
                await _context.SaveChangesAsync();

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

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
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
                MatKhau = nhanVien.ApplicationUser.PasswordHash,
                Role = nhanVien.ApplicationUser.ChucVu
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
