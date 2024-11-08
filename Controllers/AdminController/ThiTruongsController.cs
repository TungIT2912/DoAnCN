﻿using System;
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
    public class ThiTruongsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ThiTruongsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ThiTruongs
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpGet("api/ThiTruongs")]
        public async Task<ActionResult<IEnumerable<KhoDTO>>> GetThiTruongs()
        {
            var thiTruongs = await _context.ThiTruongs.ToListAsync();
            var thiTruongsDTOs = thiTruongs.Select(nv => new ThiTruongDTO
            {
                TenSanPham = nv.TenSanPham, 
                Loai = nv.Loai,
                DonViTinh = nv.DonViTinh,
                DonGia = nv.DonGia
            }).ToList();

            return Ok(thiTruongsDTOs);
        }
        private bool ThiTruongExists(int id)
        {
            return _context.ThiTruongs.Any(e => e.IdsanPham == id);
        }
    }
}