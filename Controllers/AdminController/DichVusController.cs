    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.DTO;

namespace WebQuanLyNhaKhoa.Controllers.AdminController
{
    [Route("Admin/[controller]")]
    //[Authorize(Roles = "Admin")]
    public class DichVusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DichVusController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
          
            return View();
        }

        [HttpGet("api/DichVus")]
        public async Task<ActionResult<IEnumerable<DichVuDTO>>> GetDichVus(int pageNumber = 1, int pageSize = 10)
        {
            var totalItems = await _context.DichVus.CountAsync();
            var dichVus = await _context.DichVus
            .Include(nv => nv.ChanDoan)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
            var dichVuDTOs = dichVus.Select(nv => new DichVuDTO
            {
                IddichVu = nv.IddichVu,
                IdchanDoan = nv.IdchanDoan, // Match the correct property name here
                TenChuanDoan = nv.ChanDoan.TenChanDoan,
                TenDichVu = nv.TenDichVu,
                DonViTinh = nv.DonViTinh,
                DonGia = nv.DonGia
            }).ToList();

            return Ok(new { data = dichVuDTOs, totalItems });
        }
        [HttpGet("api/DichVus/Create")]
        public IActionResult Create()
        {
            string[] DVT = { "Lần", "Cái" };
            ViewBag.DVT = new SelectList(DVT);
            ViewBag.SanPhams = new SelectList(_context.ChanDoans, "IdchanDoan", "TenChanDoan");
            return View();
        }

        [HttpPost("api/DichVus")]
        public async Task<IActionResult> Create([FromForm] DichVuDTO dto, IFormFile? hinh)
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

            if (hinh != null && hinh.Length > 0)
            {

                dto.Hinh = await SaveImage(hinh);
            }
            else
            {
                dto.Hinh = "/images/anonymous.png";
            }

            var existingItem = await _context.ChanDoans
                .FirstOrDefaultAsync(x => x.IdchanDoan == dto.IdchanDoan);

            if (existingItem != null)
            {
                var dv = new DichVu
                {
                    IdchanDoan = dto.IdchanDoan,
                    TenDichVu = dto.TenDichVu,
                    DonViTinh = dto.DonViTinh,
                    DonGia  = dto.DonGia,
                    Hinh = dto.Hinh,
                };
                _context.DichVus.Add(dv);
                var historySaveResult = await _context.SaveChangesAsync();

                if (historySaveResult > 0)
                {
                    var createdLSunDTO = new DichVuDTO
                    {
                        IddichVu = dv.IddichVu,
                        IdchanDoan = dv.IdchanDoan,
                        TenChuanDoan = dv.ChanDoan.TenChanDoan,
                        TenDichVu = dv.TenDichVu,
                        DonViTinh = dv.DonViTinh,
                        DonGia = dv.DonGia,
                        Hinh = dv.Hinh,
                    };

                    return CreatedAtAction(nameof(GetDichVuById), new { id = dv.IddichVu }, createdLSunDTO);
                }
            }
            return BadRequest(new { success = false, message = "Thêm dich vu thất bại." });
        }

        private async Task<string> SaveImage(IFormFile hinh)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(hinh.FileName)}"; 
            var savePath = Path.Combine("wwwroot/images", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);

            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await hinh.CopyToAsync(fileStream);
            }

            return "/images/" + fileName; 
        }


        [HttpGet("api/DichVus/{id}")]
        public async Task<ActionResult<DichVuDTO>> GetDichVuById(int id)
        {
            var lichsu = await _context.DichVus
                        .Include(n => n.ChanDoan)
                        .FirstOrDefaultAsync(n => n.IddichVu == id);
            if (lichsu == null)
            {
                Console.WriteLine($"Không có dịch vụ khớp vơi bạn vừa tìm.");
                return NotFound();
            }

            var createdLSunDTO = new DichVuDTO
            {
                IdchanDoan = lichsu.IdchanDoan,
                TenChuanDoan = lichsu.ChanDoan.TenChanDoan,
                TenDichVu = lichsu.TenDichVu,
                DonViTinh = lichsu.DonViTinh,
                DonGia = lichsu.DonGia,
            };

            return Ok(createdLSunDTO);
        }

        [HttpGet("api/DichVus/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.DichVus
                        .Include(n => n.ChanDoan)
                        .FirstOrDefaultAsync(n => n.IddichVu == id);
            if (nhanVien == null)
            {
                Console.WriteLine($"Dịch vụ với mã bạn vừa tìm không tồn tại.");
                return NotFound();
            }

            var nhanVienDTO = new DichVuDTO
            {
                IddichVu  = nhanVien.IddichVu,
                IdchanDoan = nhanVien.IdchanDoan,
                TenChuanDoan = nhanVien.ChanDoan.TenChanDoan,
                TenDichVu = nhanVien.TenDichVu,
                DonViTinh = nhanVien.DonViTinh,
                DonGia = nhanVien.DonGia
            };
            string[] DVT = { "Lần", "Cái" };
            ViewBag.DVT = new SelectList(DVT);
            ViewBag.SanPhams = new SelectList(_context.ChanDoans, "IdchanDoan", "TenChanDoan"); ;
            return View(nhanVienDTO);
        }
        [HttpPut("api/DichVus/Update/{id}")]
        public async Task<IActionResult> Edit(int id, [FromForm] DichVuDTO dto, IFormFile? Hinh)
        {
          
            var dichvu = await _context.DichVus
                        .Include(n => n.ChanDoan)
                        .FirstOrDefaultAsync(n => n.IddichVu == id);
            if (dichvu == null)
            {
                Console.WriteLine($"Dịch vụ với mã bạn vừa tìm không tồn tại.");
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { success = false, errors });
            }
            if (Hinh != null && Hinh.Length > 0)
            {
                dto.Hinh = await SaveImage(Hinh);
            }
            else
            {
                dto.Hinh = dichvu.Hinh;
            }
            dichvu.IdchanDoan = dto.IdchanDoan;
            dichvu.TenDichVu = dto.TenDichVu;
            dichvu.DonViTinh = dto.DonViTinh;
            dichvu.DonGia = dto.DonGia;
            dichvu.Hinh = dto.Hinh;
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "dichvu updated successfully." });
            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(new { success = false, message = "Failed to update dichvu.", details = dbEx.InnerException?.Message });
            }
        }

        [HttpGet("api/DichVus/Detail/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.DichVus
                        .Include(n => n.ChanDoan)
                        .FirstOrDefaultAsync(n => n.IddichVu == id);
            if (nhanVien == null)
            {
                Console.WriteLine($"Dịch vụ với mã bạn vừa tìm không tồn tại.");
                return NotFound();
            }

            var nhanVienDTO = new DichVuDTO
            {
                IddichVu = nhanVien.IddichVu,
                IdchanDoan = nhanVien.IdchanDoan,
                TenChuanDoan = nhanVien.ChanDoan.TenChanDoan,
                TenDichVu = nhanVien.TenDichVu,
                DonViTinh = nhanVien.DonViTinh,
                DonGia = nhanVien.DonGia,
                Hinh  = nhanVien.Hinh,
            };
            return View(nhanVienDTO);
        }

        // GET: DichVus/Delete/5
        [HttpDelete("api/DichVus/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            
            var dichVu = await _context.DichVus.FindAsync(id);

            if (dichVu == null)
            {
                return NotFound();
            }

            _context.DichVus.Remove(dichVu);

            await _context.SaveChangesAsync();

            return Ok();
        }
        private bool DichVuExists(int id)
        {
            return _context.DichVus.Any(e => e.IddichVu == id);
        }
    }
}
