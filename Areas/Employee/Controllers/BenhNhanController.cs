﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebQuanLyNhaKhoa.Data;
using X.PagedList;

namespace WebQuanLyNhaKhoa.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = SD.Role_Employee)]
    public class BenhNhanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BenhNhanController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: BenhNhan
        
        public async Task<IActionResult> Index(string query = "", string filter = "nothing")
        {
            var curentDay = DateTime.Now;
            if (query == "")
            {
                if (filter == "nothing")
                {
                    List<BenhNhan> bns = await _context.BenhNhans.ToListAsync();
                    return View(bns);
                }
                else if (filter == "Nam")
                {
                    List<BenhNhan> bns = await _context.BenhNhans
                        .Where(n => n.Gioi == false).ToListAsync();
                    return View(bns);
                }
                else if (filter == "Nữ")
                {
                    List<BenhNhan> bns = await _context.BenhNhans
                        .Where(n => n.Gioi == true).ToListAsync();
                    return View(bns);
                }
                else
                {
                    List<BenhNhan> bns = await _context.BenhNhans
                        .Where(n => n.NgayKhamDau.Value.Day == curentDay.Day && n.NgayKhamDau.Value.Month == curentDay.Month && n.NgayKhamDau.Value.Year == curentDay.Year)
                        .ToListAsync();
                    return View(bns);
                }

            }
            else
            {
                if (filter == "nothing")
                {
                    List<BenhNhan> bns = await _context.BenhNhans.Where(n => n.HoTen.Contains(query)).ToListAsync();
                    return View(bns);
                }
                else if (filter == "Nam" && query != "")
                {
                    List<BenhNhan> bns = await _context.BenhNhans
                        .Where(n => n.HoTen.Contains(query))
                        .Where(n => n.Gioi == false).ToListAsync();
                    return View(bns);
                }
                else if (filter == "Nữ" && query != "")
                {
                    List<BenhNhan> bns = await _context.BenhNhans
                        .Where(n => n.HoTen.Contains(query))
                        .Where(n => n.Gioi == true).ToListAsync();
                    return View(bns);
                }
                else
                {
                    List<BenhNhan> bns = await _context.BenhNhans.Where(n => n.HoTen.Contains(query)).Where(n => n.NgayKhamDau.Value.Day == curentDay.Day && n.NgayKhamDau.Value.Month == curentDay.Month && n.NgayKhamDau.Value.Year == curentDay.Year).ToListAsync();
                    return View(bns);
                }

            }
            return View();
        }
        public async Task<IActionResult> Move(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var benhnhan = await _context.BenhNhans.FirstOrDefaultAsync(m => m.IdbenhNhan == id);
            return View(benhnhan);
        }
        [HttpPost]
        public async Task<IActionResult> Move(string id, BenhNhan bn)
        {
            Random random = new Random();
            int randomNumber = random.Next(100, 999);
            DanhSachKham ds = new DanhSachKham
            {
                IdbenhNhan = bn.IdbenhNhan,
                NgayKham = DateTime.Now,
                Idkham = randomNumber
            };
            await _context.DanhSachKhams.AddAsync(ds);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: BenhNhan/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var benhNhan = await _context.BenhNhans
                .FirstOrDefaultAsync(m => m.IdbenhNhan == id);
            if (benhNhan == null)
            {
                return NotFound();
            }

            return View(benhNhan);
        }

        // GET: BenhNhan/Create
        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: BenhNhan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdbenhNhan,HoTen,Gioi,NamSinh,Sdt,DiaChi,NgayKhamDau")] BenhNhan benhNhan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(benhNhan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(benhNhan);
        }

        // GET: BenhNhan/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var benhNhan = await _context.BenhNhans.FindAsync(id);
            if (benhNhan == null)
            {
                return NotFound();
            }
            return View(benhNhan);
        }

        // POST: BenhNhan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdbenhNhan,HoTen,Gioi,NamSinh,Sdt,DiaChi,NgayKhamDau")] BenhNhan benhNhan)
        {
            if (id != benhNhan.IdbenhNhan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(benhNhan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BenhNhanExists(benhNhan.IdbenhNhan))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(benhNhan);
        }

        // GET: BenhNhan/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var benhNhan = await _context.BenhNhans
                .FirstOrDefaultAsync(m => m.IdbenhNhan == id);
            if (benhNhan == null)
            {
                return NotFound();
            }

            return View(benhNhan);
        }

        // POST: BenhNhan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var benhNhan = await _context.BenhNhans.FindAsync(id);
            if (benhNhan != null)
            {
                _context.BenhNhans.Remove(benhNhan);
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool BenhNhanExists(int id)
        {
            return _context.BenhNhans.Any(e => e.IdbenhNhan == id);
        }
    }
}
