using Microsoft.AspNetCore.Mvc;
using WebQuanLyNhaKhoa.Models;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.DTO;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;

namespace WebQuanLyNhaKhoa.Controllers
{

    [Route("[controller]")]
    public class AuthMvcController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthMvcController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /AuthMvc/Register
        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /AuthMvc/Register
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

        // GET: /AuthMvc/Login
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /AuthMvc/Login
        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(TaiKhoanDTO loginRequest)
        {
            var user = await _context.TaiKhoans.SingleOrDefaultAsync(u => u.TenDangNhap == loginRequest.TenDangNhap);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.MatKhau, user.MatKhau))
            {
                ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu.");
                return View(loginRequest);
            }

            // Set up session or cookies as needed for logged-in user (optional, for additional functionality)
            // Example: Set user role in session

            ViewBag.Message = "Đăng nhập thành công!";
            return RedirectToAction("Index", "Home"); // Redirect to home or another page on success
        }
    }
}
