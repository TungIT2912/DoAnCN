using System;
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
    public class BenhNhansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BenhNhansController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet("api/BenhNhans")]
        public async Task<ActionResult<IEnumerable<BenhNhanDTO>>> GetBenhhNhans()
        {
            var benhNhans = await _context.BenhNhans
                           .Include(nv => nv.TaiKhoan) 
                           .ToListAsync();
            var benhNhanDTOs = benhNhans.Select(nv => new BenhNhanDTO
            {
                IdbenhNhan = nv.IdbenhNhan,
                HoTen = nv.HoTen,
                Gioi = nv.Gioi,
                NamSinh = nv.NamSinh,
                Sdt = nv.Sdt,
                DiaChi = nv.DiaChi,
                NgayKhamDau = nv.NgayKhamDau,
            }).ToList();

            return Ok(benhNhanDTOs);
        }

        [HttpGet("api/BenhNhans/Detail/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var benhNhan = await _context.BenhNhans
                        .Include(n => n.TaiKhoan)
                        .FirstOrDefaultAsync(n => n.IdbenhNhan == id);
            if (benhNhan == null)
            {
                Console.WriteLine($"Bệnh nhân bạn vừa tìm không tồn tại.");
                return NotFound();
            }

            var benhNhanDTO = new BenhNhanDTO
            {
                IdbenhNhan = id,
                HoTen = benhNhan.HoTen,
                Gioi = benhNhan.Gioi,
                NamSinh= benhNhan.NamSinh,
                Sdt = benhNhan.Sdt,
                DiaChi = benhNhan.DiaChi,
                NgayKhamDau = benhNhan.NgayKhamDau
            };
            return View(benhNhanDTO);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var benhNhan = await _context.BenhNhans
                .Include(b => b.TaiKhoan)
                .FirstOrDefaultAsync(m => m.IdbenhNhan == id);
            if (benhNhan == null)
            {
                return NotFound();
            }

            return View(benhNhan);
        }

        // POST: BenhNhans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var benhNhan = await _context.BenhNhans.FindAsync(id);
            if (benhNhan != null)
            {
                _context.BenhNhans.Remove(benhNhan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BenhNhanExists(int id)
        {
            return _context.BenhNhans.Any(e => e.IdbenhNhan == id);
        }
    }
}
