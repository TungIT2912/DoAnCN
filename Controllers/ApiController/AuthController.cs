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
            _context.TaiKhoans.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đăng ký thành công.", role = user.Role });
        }


        [HttpPost("api/login")]
        [Produces("application/json")]
        public async Task<IActionResult> LoginApi([FromBody] TaiKhoanDTO loginRequest)
        {
            var user = await _context.TaiKhoans.SingleOrDefaultAsync(u => u.TenDangNhap == loginRequest.TenDangNhap);

            if (user.isLoocked == true)
            {
                return Unauthorized("Tài khoản đã bị vô hiệu hóa");
            }
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.MatKhau, user.MatKhau) )
            {
               
                return Unauthorized("Sai tên đăng nhập hoặc mật khẩu.");
            }

            var token = GenerateJwtToken(user);
            Response.Cookies.Append("jwt_token", token, new CookieOptions { HttpOnly = true, Secure = true, Expires = DateTimeOffset.Now.AddMinutes(10) });
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

            if (user.isLoocked == true)
            {
                return Unauthorized("Tài khoản đã bị vô hiệu hóa");
            }

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.MatKhau, user.MatKhau))
            {
              
                ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu.");
                return View(loginRequest);
            }

            // Set JWT token as a cookie (optional)
            var token = GenerateJwtToken(user);
            Response.Cookies.Append("jwt_token", token, new CookieOptions { HttpOnly = true, Secure = true, Expires = DateTimeOffset.Now.AddMinutes(10) });

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
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("EmployeeId", user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Remove the JWT token from cookies

            Response.Cookies.Delete("jwt_token");
            ViewBag.Message = "Đã đăng xuất thành công!";
            return RedirectToAction("Login");
        }

    }

}

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using WebQuanLyNhaKhoa.Models;
//using WebQuanLyNhaKhoa.Data;
//using Microsoft.EntityFrameworkCore;
//using WebQuanLyNhaKhoa.DTO;

//namespace WebQuanLyNhaKhoa.Controllers.ApiConrtroller
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AuthController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly IConfiguration _configuration;

//        public AuthController(ApplicationDbContext context, IConfiguration configuration)
//        {
//            _context = context;
//            _configuration = configuration;
//        }

//        [HttpPost("register")]
//        public async Task<IActionResult> Register([FromBody] TaiKhoan user)
//        {
//            // Check if username already exists
//            if (_context.TaiKhoans.Any(u => u.TenDangNhap == user.TenDangNhap))
//            {
//                return BadRequest("Tên đăng nhập đã tồn tại.");
//            }

//            // Validate Role (Admin, Customer, Staff)
//            var validRoles = new[] { "Admin", "Customer", "Staff" };
//            if (!validRoles.Contains(user.Role))
//            {
//                return BadRequest("Vai trò không hợp lệ. Vai trò phải là Admin, Customer, hoặc Staff.");
//            }

//            // Hash password before saving
//            user.MatKhau = BCrypt.Net.BCrypt.HashPassword(user.MatKhau);
//            _context.TaiKhoans.Add(user);
//            await _context.SaveChangesAsync();

//            return Ok(new { message = "Đăng ký thành công.", role = user.Role });
//        }

//        [HttpPost("login")]
//        public async Task<IActionResult> Login(TaiKhoanDTO loginRequest)
//        {
//            var user = await _context.TaiKhoans.SingleOrDefaultAsync(u => u.TenDangNhap == loginRequest.TenDangNhap);

//            // Check username and password
//            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.MatKhau, user.MatKhau))
//            {
//                return Unauthorized("Sai tên đăng nhập hoặc mật khẩu.");
//            }

//            var token = GenerateJwtToken(user);
//            return Ok(new { Token = token });
//        }

//        private string GenerateJwtToken(TaiKhoan user)
//        {
//            var jwtSettings = _configuration.GetSection("JwtSettings");
//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//            // Include role claim in the JWT token
//            var claims = new[]
//            {
//                new Claim(JwtRegisteredClaimNames.Sub, user.TenDangNhap),
//                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
//                new Claim(ClaimTypes.Role, user.Role) // Add role claim
//            };

//            var token = new JwtSecurityToken(
//                issuer: jwtSettings["Issuer"],
//                audience: jwtSettings["Audience"],
//                claims: claims,
//                expires: DateTime.Now.AddMinutes(30),
//                signingCredentials: creds
//            );

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }
//    }
//}

/*using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.Models;

namespace WebQuanLyNhaKhoa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AuthController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AuthController> logger,
            IEmailSender emailSender,
            ApplicationDbContext context,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
            _configuration = configuration;
        }

        public class RegisterRequest
        {
            [Required]
            [StringLength(100, ErrorMessage = "Tên phải dài ít nhất 5 kí tự.", MinimumLength = 5)]
            public string FullName { get; set; }

            [Display(Name = "Địa chỉ")]
            public string Address { get; set; }

            public string UserName { get; set; }
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The password must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            public string Password { get; set; }

            public string Role { get; set; }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                FullName = request.FullName,
                Address = request.Address,
                Email = request.Email,
                UserName = request.Email,
                ChucVu = request.Role,
                Role = request.Role
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                var assignedRole = request.Role ?? "Customer";

                // Ensure role exists before assigning
                if (await _roleManager.RoleExistsAsync(assignedRole))
                {
                    await _userManager.AddToRoleAsync(user, assignedRole);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid role specified.");
                    return BadRequest(ModelState);
                }

                var customer = new BenhNhan
                {
                    UserId = user.Id,
                    HoTen = request.FullName
                };

                _context.BenhNhans.Add(customer);
                await _context.SaveChangesAsync();

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action(
                    "ConfirmEmail",
                    "Auth",
                    new { userId = user.Id, code = code },
                    protocol: HttpContext.Request.Scheme);

                await _emailSender.SendEmailAsync(request.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return Ok(new { message = "Registration successful. Please confirm your email to complete the registration." });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        

        private string GenerateJwtToken(ApplicationUser user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Include role claim in the JWT token
            var claims = new[]
            {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Role, user.Role) // Add role claim
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
*/