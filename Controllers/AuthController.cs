using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebQuanLyNhaKhoa.Models;
using WebQuanLyNhaKhoa.Data;
using Microsoft.EntityFrameworkCore;
using WebQuanLyNhaKhoa.DTO;

namespace WebQuanLyNhaKhoa.Controllers.ApiConrtroller
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // API đăng ký (trả về JSON)
        [HttpPost("api/register")]
        [Produces("application/json")]
        public async Task<IActionResult> RegisterApi([FromBody] TaiKhoan user)
        {
            if (_context.TaiKhoans.Any(u => u.TenDangNhap == user.TenDangNhap))
            {
                return BadRequest("Tên đăng nhập đã tồn tại.");
            }

            var validRoles = new[] { "Admin", "Customer", "Staff" };
            if (!validRoles.Contains(user.Role))
            {
                return BadRequest("Vai trò không hợp lệ. Vai trò phải là Admin, Customer, hoặc Staff.");
            }

            user.MatKhau = BCrypt.Net.BCrypt.HashPassword(user.MatKhau);
            user.isLoocked = false;
            _context.TaiKhoans.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đăng ký thành công.", role = user.Role });
        }


        [HttpPost("api/login")]
        [Produces("application/json")]
        public async Task<IActionResult> LoginApi([FromBody] TaiKhoanDTO loginRequest)
        {
            var user = await _context.TaiKhoans.SingleOrDefaultAsync(u => u.TenDangNhap == loginRequest.TenDangNhap);

            // Check username and password
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.MatKhau, user.MatKhau)   ||  user.isLoocked == true)
            {
                return Unauthorized("Sai tên đăng nhập hoặc mật khẩu. Hoặc tài khoản đã bị vô hiệu hóa");
            }

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }


        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(TaiKhoan user)
        {
            if (_context.TaiKhoans.Any(u => u.TenDangNhap == user.TenDangNhap))
            {
                ModelState.AddModelError("", "Tên đăng nhập đã tồn tại.");
                return View(user);
            }

            var validRoles = new[] { "Admin", "Customer", "Staff" };
            if (!validRoles.Contains(user.Role))
            {
                ModelState.AddModelError("", "Vai trò không hợp lệ. Vai trò phải là Admin, Customer, hoặc Staff.");
                return View(user);
            }

            user.MatKhau = BCrypt.Net.BCrypt.HashPassword(user.MatKhau);
            _context.TaiKhoans.Add(user);
            await _context.SaveChangesAsync();

            ViewBag.Message = "Đăng ký thành công!";
            return RedirectToAction("Login");
        }

        // View đăng nhập
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(TaiKhoanDTO loginRequest)
        {
            var user = await _context.TaiKhoans.SingleOrDefaultAsync(u => u.TenDangNhap == loginRequest.TenDangNhap);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.MatKhau, user.MatKhau) || user.isLoocked == true)
            {
                ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu. Hoặc tài khoản đã bị vô hiệu hóa");
                return View(loginRequest);
            }

            // Set JWT token as a cookie (optional)
            var token = GenerateJwtToken(user);
            Response.Cookies.Append("jwt_token", token, new CookieOptions { HttpOnly = true, Secure = true });

            ViewBag.Message = "Đăng nhập thành công!";
            return RedirectToAction("Index", "Home");
        }

        private string GenerateJwtToken(TaiKhoan user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.TenDangNhap),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}