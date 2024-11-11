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
using Microsoft.AspNetCore.Authorization;
namespace WebQuanLyNhaKhoa.Controllers.AdminController
{
    [Route("Admin/[controller]")]
    public class NhanViensController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NhanViensController( ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: NhanViens
        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }
        [HttpGet("api/NhanViens")]
        public async Task<ActionResult> GetNhanViens(int pageNumber = 1, int pageSize = 10)
        {
            var totalItems = await _context.NhanViens.CountAsync();

            var nhanViens = await _context.NhanViens
                .Include(nv => nv.ChucVu)
                .Include(nv => nv.TaiKhoan)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var nhanVienDTOs = nhanViens.Select(nv => new NhanVienDTO
            {
                MaNv = nv.MaNv,
                Ten = nv.Ten,
                Sdt = nv.Sdt,
                MaCv = nv.MaCv,
                TenCv = nv.ChucVu.TenCv,
                KinhNghiem = nv.KinhNghiem,
                Diachi = nv.Diachi,
                Gioi = nv.Gioi,
                Hinh = nv.Hinh,
                Email = nv.Email,
                MatKhau = nv.TaiKhoan.MatKhau,
                Role = nv.TaiKhoan.Role
            }).ToList();

            // Return the paginated data with total items
            return Ok(new { data = nhanVienDTOs, totalItems });
        }
        // GET: NhanViens/Details/5

        [HttpGet("nhanviens")]
        public IActionResult Create()
        {
            ViewBag.ChucVuList = new SelectList(_context.ChucVus, "MaCv", "TenCv");
            ViewBag.Roles = new SelectList(new[] { "Admin", "Staff" });
            return View();
        }
        [HttpPost("api/NhanViens")]
        public async Task<IActionResult> CreateNhanVien([FromForm] NhanVienDTO dto, IFormFile? hinh)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { success = false, errors });
            }
            if (hinh != null && hinh.Length > 0)
            {
                
                dto.Hinh = await SaveImage(hinh);
            }
            else
            {
                dto.Hinh = "/images/anonymous.png"; 
            }

            var existingAccount = await _context.TaiKhoans
                .FirstOrDefaultAsync(x => x.TenDangNhap == dto.Email);

            if (existingAccount != null)
            {
                return BadRequest(new { success = false, message = "Email đã tồn tại." });
            }

            var existingChucVu = await _context.ChucVus.FindAsync(dto.MaCv);
            if (existingChucVu == null)
            {
                return NotFound(new { success = false, message = $"ChucVu với MaCv {dto.MaCv} không tồn tại." });
            }

            var validRoles = new[] { "Admin", "Customer", "Staff" };

            if (!validRoles.Any(role => role.Equals(dto.Role, StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest(new { success = false, message = "Invalid role specified." });
            }
            if (dto.Role.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                dto.Role = "Admin"; // Set to "Admin"
            }
            else if (dto.Role.Equals("staff", StringComparison.OrdinalIgnoreCase))
            {
                dto.Role = "Staff"; // Set to "Admin"
            }
            else
            {
                dto.Role = "Customer";
            }
            // Create new TaiKhoan entity
            var user = new TaiKhoan
            {
                TenDangNhap = dto.Email,
                MatKhau = BCrypt.Net.BCrypt.HashPassword(dto.MatKhau),
                Role = dto.Role,
                isLoocked = false,
            };

            try
            {
                // Attempt to add user to context and save changes
                await _context.TaiKhoans.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(new { success = false, message = "Failed to create user.", details = dbEx.InnerException?.Message });
            }
            var newNhanVien = new NhanVien
            {
                Ten = dto.Ten,
                UserId = user.Id,
                Sdt = dto.Sdt,
                MaCv = dto.MaCv,
                KinhNghiem = dto.KinhNghiem,
                Diachi = dto.Diachi,
                Gioi = dto.Gioi,
                Hinh = dto.Hinh,
                Email = dto.Email,
                TaiKhoan = user
            };

            _context.NhanViens.Add(newNhanVien);
            var nhanVienSaveResult = await _context.SaveChangesAsync();
           
            if (nhanVienSaveResult > 0)
            {
                var createdNhanVienDTO = new NhanVienDTO
                {
                    MaNv = newNhanVien.MaNv,
                    Ten = newNhanVien.Ten,
                    Sdt = newNhanVien.Sdt,
                    MaCv = newNhanVien.MaCv,
                    
                    KinhNghiem = newNhanVien.KinhNghiem,
                    Diachi = newNhanVien.Diachi,
                    Gioi = newNhanVien.Gioi,
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
            // Set the path for saving the image
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(hinh.FileName)}"; // Keep the original extension
            var savePath = Path.Combine("wwwroot/images", fileName);

            // Create the directory if it does not exist
            Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);

            // Save the image to the specified path
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await hinh.CopyToAsync(fileStream);
            }

            return "/images/" + fileName; // Return the relative path as a string
        }



        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        [HttpGet("api/NhanViens/{id}")] 
        public async Task<ActionResult<NhanVienDTO>> GetNhanVienById(int id)
        {
            var nhanVien = await _context.NhanViens
                        .Include(n => n.TaiKhoan)
                        .FirstOrDefaultAsync(n => n.MaNv == id);
            if (nhanVien == null)
            {
                Console.WriteLine($"Nhân viên với mã nhân viên bạn vừa tìm không tồn tại.");
                return NotFound(); 
            }

            var nhanVienDTO = new NhanVienDTO
            {
                Ten = nhanVien.Ten,
                Sdt = nhanVien.Sdt,
                MaCv = nhanVien.MaCv,
                KinhNghiem = nhanVien.KinhNghiem,
                Diachi = nhanVien.Diachi,
                Gioi= nhanVien.Gioi,
                Hinh = nhanVien.Hinh,
                Email = nhanVien.Email,
                MatKhau = nhanVien.TaiKhoan.MatKhau,
                Role = nhanVien.TaiKhoan.Role
            };

            return Ok(nhanVienDTO); 
        }



        [HttpGet("api/NhanViens/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens
                        .Include(n => n.TaiKhoan)
                        .FirstOrDefaultAsync(n => n.MaNv == id);
            if (nhanVien == null)
            {
                Console.WriteLine($"Nhân viên với mã nhân viên bạn vừa tìm không tồn tại.");
                return NotFound();
            }

            var nhanVienDTO = new NhanVienDTO
            {
                MaNv=id,
                Ten = nhanVien.Ten,
                Sdt = nhanVien.Sdt,
                MaCv = nhanVien.MaCv,
                KinhNghiem = nhanVien.KinhNghiem,
                Diachi = nhanVien.Diachi,
                Gioi = nhanVien.Gioi,
                Hinh = nhanVien.Hinh,
                Email = nhanVien.Email,
                MatKhau = nhanVien.TaiKhoan.MatKhau,
                Role = nhanVien.TaiKhoan.Role
            };
            ViewBag.ChucVuList = new SelectList(_context.ChucVus, "MaCv", "TenCv");
            ViewBag.Roles = new SelectList(new[] { "Admin", "Customer" });
            return View(nhanVienDTO);
        }
        [HttpPut("api/NhanViens/Update/{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] NhanVienDTO dto, IFormFile? Hinh)
        {
            ModelState.Remove("Hinh");
            var nhanVien = await _context.NhanViens
                        .Include(n => n.TaiKhoan)
                        .FirstOrDefaultAsync(n => n.MaNv == id);
            if (nhanVien == null)
            {
                Console.WriteLine($"Nhân viên với mã nhân viên bạn vừa tìm không tồn tại.");
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { success = false, errors });
            }

            if (Hinh != null && Hinh.Length > 0)
            {
                dto.Hinh = await SaveImage(Hinh);
            }
            else
            {
                dto.Hinh = nhanVien.Hinh;
            }
            
            nhanVien.Ten = dto.Ten;
            nhanVien.Sdt = dto.Sdt;
            nhanVien.MaCv = dto.MaCv;
            nhanVien.KinhNghiem = dto.KinhNghiem;
            nhanVien.Diachi = dto.Diachi;
            nhanVien.Gioi = dto.Gioi;
            nhanVien.Hinh = dto.Hinh;
            nhanVien.Email = dto.Email;
            var taikhoan = nhanVien.TaiKhoan; 
            bool accountUpdated = false;

            if (taikhoan.TenDangNhap != dto.Email)
            {
                taikhoan.TenDangNhap = dto.Email; 
                accountUpdated = true;
            }
            if (!string.IsNullOrWhiteSpace(dto.MatKhau)) 
            {
                taikhoan.MatKhau = BCrypt.Net.BCrypt.HashPassword(dto.MatKhau);
                accountUpdated = true;
            }
            else
            {
                taikhoan.MatKhau = nhanVien.TaiKhoan.MatKhau;
            }
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "NhanVien updated successfully." });
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(new { success = false, message = "Failed to update NhanVien.", details = dbEx.InnerException?.Message });
            }
        }

        [HttpGet("api/NhanViens/Detail/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens
                        .Include(n => n.ChucVu)
                        .Include(n => n.TaiKhoan)
                        .FirstOrDefaultAsync(n => n.MaNv == id);
            if (nhanVien == null)
            {
                Console.WriteLine($"Nhân viên với mã nhân viên bạn vừa tìm không tồn tại.");
                return NotFound();
            }

            var nhanVienDTO = new NhanVienDTO
            {
                MaNv = id,
                Ten = nhanVien.Ten,
                Sdt = nhanVien.Sdt,
                TenCv = nhanVien.ChucVu.TenCv,
                KinhNghiem = nhanVien.KinhNghiem,
                Diachi = nhanVien.Diachi,
                Gioi = nhanVien.Gioi,
                Hinh = nhanVien.Hinh,
                Email = nhanVien.Email,
                MatKhau = nhanVien.TaiKhoan.MatKhau,
                Role = nhanVien.TaiKhoan.Role
            };
            ViewBag.ChucVuList = new SelectList(_context.ChucVus, "MaCv", "TenCv");
            ViewBag.Roles = new SelectList(new[] { "Admin", "Customer" });
            return View(nhanVienDTO);
        }

        private bool NhanVienExists(int id)
        {
            return _context.NhanViens.Any(e => e.MaNv == id);
        }

    }
}
