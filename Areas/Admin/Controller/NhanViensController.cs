//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Threading.Tasks;
//using AutoMapper;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using WebQuanLyNhaKhoa.Data;
//using WebQuanLyNhaKhoa.DTO;

//namespace WebQuanLyNhaKhoa.Areas.Admin.Controller
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class NhanViensController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly RoleManager<IdentityRole> _roleManager;
//        private readonly ILogger<NhanViensController> _logger;
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly IMapper _mapper;

//        public NhanViensController(ILogger<NhanViensController> logger,UserManager<ApplicationUser> userManager,
//            RoleManager<IdentityRole> roleManager, ApplicationDbContext context, IMapper mapper)
//        {
//            _logger = logger;
//            _roleManager = roleManager;
//            _userManager = userManager;
//            _context = context;
//            _mapper = mapper;
//        }

//        // GET: api/NhanViens
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<NhanVienDTO>>> GetNhanViens()
//        {
//            var nhanViens = await _context.NhanViens.ToListAsync();
//            var nhanVienDTOs = _mapper.Map<List<NhanVienDTO>>(nhanViens);

//            return Ok(nhanVienDTOs);
//        }

//        // Create a new employee
//        [HttpPost]
//        public async Task<IActionResult> CreateNhanVien([FromBody] NhanVienDTO dto)
//        {

//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }
//            var existingAccount = await _context.Users
//      .FirstOrDefaultAsync(x => x.Email == dto.Email);

//            if (existingAccount != null)
//            {
//                // Return error response if the email already exists
//                return BadRequest(new { success = false, message = "Email already exists." });
//            }
//            var chucVu = await _context.ChucVus.FindAsync(dto.MaCv);
//            if (chucVu == null)
//            {
//                return NotFound($"ChucVu with MaCv {dto.MaCv} not found.");
//            }
//            var user = new ApplicationUser
//            {
//                UserName = dto.Email,
//                FullName = dto.Ten,
//                Email = dto.Email,
//                ChucVu = dto.MaCv,
//                Address = dto.Diachi
//            };
//            var result = await _userManager.CreateAsync(user, dto.MatKhau);
//            // Manual mapping from NhanVienDTO to NhanVien
         
//            if (result.Succeeded)
//            {
//                var roleName = dto.Role switch
//                {
//                    "admin" => "Admin",
//                    "employee" => "Employee",
//                    _ => null
//                };

//                if (roleName != null)
//                {
//                    var role = await _roleManager.FindByNameAsync(roleName);
//                    if (role == null)
//                    {
//                        role = new IdentityRole(roleName);
//                        await _roleManager.CreateAsync(role);
//                    }
//                }
//                else
//                {
//                    return BadRequest("Invalid role specified."); // Handle invalid roles
//                }

//                await _userManager.AddToRoleAsync(user, roleName);

//                var newNhanVien = new NhanVien
//                {
//                    Ten = dto.Ten,
//                    UserId = user.Id,
//                    Sdt = dto.Sdt,
//                    MaCv = dto.MaCv,
//                    KinhNghiem = dto.KinhNghiem,
//                    Hinh = dto.Hinh,
//                    Email = dto.Email,

//                };

//                // Add the new employee to the context
//                _context.NhanViens.Add(newNhanVien);
//                await _context.SaveChangesAsync();

//                // Create a response DTO manually
//                var createdNhanVienDTO = new NhanVienDTO
//                { // Assuming you want to return the newly created ID
//                    Ten = newNhanVien.Ten,
//                    Sdt = newNhanVien.Sdt,
//                    MaCv = newNhanVien.MaCv,
//                    KinhNghiem = newNhanVien.KinhNghiem,
//                    Hinh = newNhanVien.Hinh,
//                    Email = newNhanVien.Email
//                };
//                return CreatedAtAction(nameof(GetNhanVienById), new { id = newNhanVien.MaNv }, new { success = true, message = "Employee created successfully." });
//            }
//            AddErrors(result);

//            // Return a response indicating success
            

//            // If user creation failed, log and return the errors
//            foreach (var error in result.Errors)
//            {
//                ModelState.AddModelError(string.Empty, error.Description);
//                _logger.LogWarning("User creation error: {Error}", error.Description);
//            }

//            // Return the model state errors if user creation was unsuccessful
//            return BadRequest(ModelState);
//        }
//        private void AddErrors(IdentityResult result)
//        {
//            foreach (var error in result.Errors)
//            {
//                ModelState.AddModelError(string.Empty, error.Description);
//            }
//        }

//        // Get a specific employee by ID
//        [HttpGet("{id}")]
//        public async Task<ActionResult<NhanVienDTO>> GetNhanVienById(int id)
//        {
//            var nhanVien = await _context.NhanViens.FindAsync(id);
//            if (nhanVien == null)
//            {
//                return NotFound();
//            }

//            var nhanVienDTO = _mapper.Map<NhanVienDTO>(nhanVien);
//            return nhanVienDTO;
//        }
//        // DELETE: api/NhanViens/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteNhanVien(int id)
//        {
//            var nhanVien = await _context.NhanViens.FindAsync(id);
//            if (nhanVien == null)
//            {
//                return NotFound();
//            }

//            _context.NhanViens.Remove(nhanVien);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool NhanVienExists(int id)
//        {
//            return _context.NhanViens.Any(e => e.MaNv == id);
//        }
//    }
//}
