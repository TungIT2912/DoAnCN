﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using WebQuanLyNhaKhoa.Data;

//namespace WebQuanLyNhaKhoa.Controllers.UserController
//{
//    [Area("Admin")]
//    [Authorize(Roles = SD.Role_Admin)]
//    public class DanhSachKhamsController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public DanhSachKhamsController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        // GET: DanhSachKhams
//        public async Task<IActionResult> Index()
//        {
//            var qlnhaKhoaContext = _context.DanhSachKhams.Include(d => d.IdbenhNhanNavigation).Include(d => d.MaNvNavigation);
//            return View(await qlnhaKhoaContext.ToListAsync());
//        }

//        // GET: DanhSachKhams/Create
//        public IActionResult Create()
//        {
//            ViewData["IdbenhNhan"] = new SelectList(_context.BenhNhans, "IdbenhNhan", "HoTen");
//            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "Ten");
//            return View();
//        }

//        // POST: DanhSachKhams/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Idkham,IdbenhNhan,MaNv,NgayKham")] DanhSachKham danhSachKham)
//        {

//            _context.Add(danhSachKham);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//            ViewData["IdbenhNhan"] = new SelectList(_context.BenhNhans, "IdbenhNhan", "HoTen", danhSachKham.IdbenhNhan);
//            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "Ten", danhSachKham.MaNv);
//            return View(danhSachKham);
//        }

//        private bool DanhSachKhamExists(int id)
//        {
//            return _context.DanhSachKhams.Any(e => e.Idkham == id);
//        }
//    }
//}
