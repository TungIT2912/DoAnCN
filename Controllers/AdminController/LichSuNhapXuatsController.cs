using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.DTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebQuanLyNhaKhoa.Controllers.AdminController
{
    [Route("Admin/[controller]")]
    public class LichSuNhapXuatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LichSuNhapXuatsController(ApplicationDbContext context)
        {
            _context = context;
        }

      
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var lichSuNhapXuat = await _context.LichSuNhapXuats
        //        .Include(l => l.IddungCuNavigation)
        //        .FirstOrDefaultAsync(m => m.MaLs == id);
        //    if (lichSuNhapXuat == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(lichSuNhapXuat);
        //}

        // GET: LichSuNhapXuats/Create
        public IActionResult Create()
        {
            string[] Contents = { "Nhập", "Xuất" };
            ViewBag.Contents = new SelectList(Contents);

            ViewData["IdsanPham"] = new SelectList(_context.ThiTruongs, "IdsanPham", "TenSanPham");

            return View();
        }

        // POST: LichSuNhapXuats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("api/LichSuNhapXuats")]
        public async Task<IActionResult> Create([FromBody] LichSuNhapXuatDTO dto)
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
          

            var existingItem = await _context.ThiTruongs
                .FirstOrDefaultAsync(x => x.IdsanPham == dto.IdsanPham);

            if (existingItem != null)
            {
                if (dto.SoLuongNhapXuat <= 0)
                {
                    ModelState.AddModelError("SoLuongNhapXuat", "Số lượng phải lớn hơn 0.");
                    string[] Contents = { "Nhập", "Xuất" };
                    ViewBag.Contents = new SelectList(Contents);
                    ViewData["IdsanPham"] = new SelectList(_context.Khos, "IdsanPham", "TenSanPham", dto.IdsanPham);
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return BadRequest(new { success = false, errors });
                }
                var ls = new LichSuNhapXuat
                {
                    NoiDung = dto.NoiDung,
                    IdsanPham = dto.IdsanPham,
                    Loai = dto.Loai,
                    DonViTinh = dto.DonViTinh,
                    SoLuongNhapXuat = dto.SoLuongNhapXuat,
                    Don = dto.Don,
                    ThanhTien = dto.ThanhTien,
                    NgayNhap  = dto.NgayNhap
                };
                var existingStore = await _context.Khos
                .FirstOrDefaultAsync(x => x.IddungCu == dto.IdsanPham);
                if (existingStore != null)
                {
                    if (dto.NoiDung == "Nhập")
                    {
                        // Tăng số lượng sản phẩm trong kho
                        existingStore.SoLuong += dto.SoLuongNhapXuat;
                        _context.Update(existingStore);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        // Giảm số lượng sản phẩm trong kho
                        existingStore.SoLuong -= dto.SoLuongNhapXuat;
                        _context.Update(existingStore);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    if (dto.NoiDung == "Nhập")
                    {
                        var khoDTO = new Kho
                        {
                            IddungCu = dto.IdsanPham,
                            Loai = dto.Loai,
                            DonViTinh = dto.DonViTinh,
                            SoLuong = dto.SoLuongNhapXuat
                        };
                        try
                        {
                            // Attempt to add user to context and save changes
                            await _context.Khos.AddAsync(khoDTO);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateException dbEx)
                        {
                            return BadRequest(new { success = false, message = "Failed to create.", details = dbEx.InnerException?.Message });
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("NoiDung", "Trong kho không tồn tại mặt hàng này để xuất.");
                    }
                }
                _context.LichSuNhapXuats.Add(ls);
                var historySaveResult = await _context.SaveChangesAsync();

                if (historySaveResult > 0)
                {
                    var createdLSunDTO = new LichSuNhapXuatDTO
                    {
                        NoiDung = dto.NoiDung,
                        IdsanPham = dto.IdsanPham,
                        TenDungCu = ls.ThiTruong.TenSanPham,
                        Loai = ls.Loai,
                        DonViTinh = ls.DonViTinh,
                        SoLuongNhapXuat = ls.SoLuongNhapXuat,
                        Don = ls.Don,
                        ThanhTien = ls.ThanhTien,
                        NgayNhap = ls.NgayNhap
                    };

                    return CreatedAtAction(nameof(GetLichhSuById), new { id = ls.IdsanPham }, createdLSunDTO);
                }
            }
            // If any save operation fails, return a BadRequest with an error message
            return BadRequest(new { success = false, message = "Thêm lịch sử thất bại." });
        }

        [HttpGet("api/LichSuNhapXuats/{id}")]
        public async Task<ActionResult<LichSuNhapXuatDTO>> GetLichhSuById(int id)
        {
            var lichsu = await _context.LichSuNhapXuats
                        .Include(n => n.ThiTruong)
                        .FirstOrDefaultAsync(n => n.IdsanPham == id);
            if (lichsu == null)
            {
                Console.WriteLine($"Không có lịch sử khớp vơi bạn vừa tìm.");
                return NotFound();
            }

            var lichsuDTO = new LichSuNhapXuatDTO
            {
                NoiDung = lichsu.NoiDung,
                IdsanPham = lichsu.IdsanPham,
                TenDungCu = lichsu.ThiTruong.TenSanPham,
                Loai = lichsu.Loai,
                DonViTinh = lichsu.DonViTinh,
                SoLuongNhapXuat = lichsu.SoLuongNhapXuat,
                Don = lichsu.Don,
                ThanhTien = lichsu.ThanhTien,
                NgayNhap = lichsu.NgayNhap
            };

            return Ok(lichsuDTO);
        }
    }
}
