using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.Models;

namespace WebQuanLyNhaKhoa.Controllers.HomePageCustomer
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Home
public async Task<IActionResult> Index()
{
    var username = GetLoggedInUsername();
    ViewBag.Username = username;
    int Experienced = 10;

    var qlnhaKhoaContext = _context.NhanViens.Where(n => n.Ten != null);

    int nhanViensCount = await _context.NhanViens.CountAsync();
        int khachHangCount = await _context.HoaDons.CountAsync();


    ViewBag.NhanViensCount = nhanViensCount;
    ViewBag.KhachHangCount = khachHangCount;

    return View(await qlnhaKhoaContext.ToListAsync());
}

        private string GetLoggedInUsername()
        {
            if (Request.Cookies.TryGetValue("jwt_token", out string token))
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtHandler.ReadToken(token) as JwtSecurityToken;

                var usernameClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
                return usernameClaim?.Value;
            }
            return null;
        }
        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images", image.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + image.FileName;
        }
        public IActionResult BookAppointment()
        {
            return View();
        }
        [HttpPost]
        public IActionResult BookAppointment(BenhNhan benhNhan)
        {
            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    _context.BenhNhans.Add(benhNhan);
                    _context.SaveChanges();
                }
            }
            return View(benhNhan);
        }
            public IActionResult HandleError(int statusCode)
        {
            if (statusCode == 404)
            {
                return View("404"); 
            }

            return View("Error"); 
        }
    }
}
