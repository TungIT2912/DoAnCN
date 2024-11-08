using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.DTO;

namespace WebQuanLyNhaKhoa.Controllers.ApiController
{
    [Route("[controller]")]
    public class ChanDoanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChanDoanController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            return View();
        }


        // API: Lấy danh sách tất cả các chẩn đoán
        [HttpGet("api/GetChanDoan")]
        public async Task<ActionResult<IEnumerable<ChanDoanDTO>>> GetChanDoans()
        {
            var chanDoans = await _context.ChanDoans.ToListAsync();
            if (chanDoans.Count == 0)
            {
                return NotFound("Không có chẩn đoán nào trong cơ sở dữ liệu.");
            }
            var chanDoanDTOs = chanDoans.Select(nv => new ChanDoanDTO
            {
                IdchanDoan = nv.IdchanDoan,
                TenChanDoan = nv.TenChanDoan

            }).ToList();
            return Ok(chanDoans);
        }

        // API: Lấy chẩn đoán theo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ChanDoan>> GetChanDoan(int id)
        {
            var chanDoan = await _context.ChanDoans.FindAsync(id);

            if (chanDoan == null)
            {
                return NotFound("Chẩn đoán không tồn tại.");
            }

            return Ok(chanDoan);
        }


        [HttpGet("Create")]
        public IActionResult Create()
        {

            return View();
        }
        // API: Thêm mới chẩn đoán
        [HttpPost("api/PostChanDoan")]
        public async Task<ActionResult<ChanDoan>> PostChanDoan([FromBody] ChanDoanDTO chanDoan)
        {
            if (string.IsNullOrWhiteSpace(chanDoan.TenChanDoan))
            {
                return BadRequest("Tên chẩn đoán không được để trống.");
            }
            var newChanDoan = new ChanDoan
            {
                IdchanDoan = chanDoan.IdchanDoan,
                TenChanDoan = chanDoan.TenChanDoan,
                
               
            };


            _context.ChanDoans.Add(newChanDoan);
            var chanDoanResult = await _context.SaveChangesAsync();
            if(chanDoanResult > 0)
            {
                var chanDoanDto = new ChanDoanDTO
                {
                    IdchanDoan = chanDoan.IdchanDoan,
                    TenChanDoan = chanDoan.TenChanDoan
                };
                return CreatedAtAction(nameof(GetChanDoan), new { id = chanDoan.IdchanDoan }, chanDoanDto);
            }
            return BadRequest(new { success = false, message = "Failed to create ChanDoan." });
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var chandoan = await _context.ChanDoans.FindAsync(id); // Retrieve BenhNhan by ID

            if (chandoan == null)
            {
                return NotFound();
            }

            // Map BenhNhan to BenhNhanDTO
            var chanDoanDto = new ChanDoanDTO
            {
                IdchanDoan = chandoan.IdchanDoan,
                TenChanDoan = chandoan.TenChanDoan
                
            };

            // Pass BenhNhanDTO to the view

            return View(chanDoanDto);
        }
        // API: Cập nhật chẩn đoán
        [HttpPut("/api/PutChanDoan/{id}")]
        public async Task<IActionResult> PutChanDoan(int id, [FromBody] ChanDoanDTO chanDoan)
        {
            if (id != chanDoan.IdchanDoan)
            {
                return BadRequest("ID không hợp lệ.");
            }

            var existingChanDoan = await _context.ChanDoans.FindAsync(id);
            if (existingChanDoan == null)
            {
                return NotFound("Chẩn đoán không tồn tại.");
            }

            existingChanDoan.TenChanDoan = chanDoan.TenChanDoan;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Lỗi khi cập nhật dữ liệu.");
            }

            return NoContent();
        }

        // API: Xóa chẩn đoán
        [HttpDelete("api/DeleteChanDoan/{id}")]
        public async Task<IActionResult> DeleteChanDoan(int id)
        {
            var chanDoan = await _context.ChanDoans.FindAsync(id);
            if (chanDoan == null)
            {
                return NotFound("Chẩn đoán không tồn tại.");
            }

            _context.ChanDoans.Remove(chanDoan);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("ChanDoanBenh")]
        public IActionResult ChanDoanBenh()
        {

            return View();
        }

        //[HttpPost("api/ChanDoanBenh")]
        //public async Task<IActionResult> ChanDoanBenh(int idBenhNhan, int idChanDoan, string ghiChu)
        //{
        //    var benhNhan = await _context.BenhNhans.FindAsync(idBenhNhan);

        //    if (benhNhan == null)
        //    {
        //        return NotFound(new { success = false, message = "Bệnh nhân không tồn tại." });
        //    }

        //    var chanDoan = await _context.ChanDoans.FindAsync(idChanDoan);
        //    if (chanDoan == null)
        //    {
        //        return NotFound(new { success = false, message = "Chẩn đoán không tồn tại." });
        //    }

        //    // Cập nhật thông tin chẩn đoán cho bệnh nhân
        //    benhNhan.IdChanDoan = idChanDoan; 
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        return StatusCode(500, new { success = false, message = "Không thể lưu chẩn đoán." });
        //    }

        //    return Ok(new { success = true, message = "Chẩn đoán bệnh thành công!" });
        //}
        [HttpPost("api/ChanDoanBenh")]
        public async Task<IActionResult> ChanDoanBenh([FromBody] ChanDoanDTO chanDoanDTO)
        {
            if (chanDoanDTO == null || chanDoanDTO.IdBenhNhan == 0 || chanDoanDTO.IdchanDoan == 0)
            {
                return BadRequest(new { success = false, message = "Thông tin chẩn đoán không hợp lệ." });
            }

            var benhNhan = await _context.BenhNhans.FindAsync(chanDoanDTO.IdBenhNhan);
            if (benhNhan == null)
            {
                return NotFound(new { success = false, message = "Bệnh nhân không tồn tại." });
            }

            benhNhan.IdChanDoan = chanDoanDTO.IdchanDoan;
            benhNhan.GhiChu = chanDoanDTO.GhiChu;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Chẩn đoán bệnh thành công!" });
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new { success = false, message = "Không thể lưu chẩn đoán." });
            }
        }


        [HttpGet("api/GetBenhNhanByDate")]
        public async Task<ActionResult<IEnumerable<BenhNhanDTO>>> GetBenhNhanByDate(string date)
        {
            if (string.IsNullOrEmpty(date) || !DateTime.TryParse(date, out DateTime parsedDate))
            {
                return BadRequest(new { success = false, message = "Invalid or missing date." });
            }

            var benhNhans = await _context.BenhNhans
                                .Where(bn => bn.NgayKhamDau.HasValue && bn.NgayKhamDau.Value.Date == parsedDate.Date)
                                .Include(bn => bn.TaiKhoan)
                                .ToListAsync();

            if (benhNhans == null || !benhNhans.Any())
            {
                return NotFound(new { success = false, message = "No patients found for the specified date." });
            }

            var benhNhanDTOs = benhNhans.Select(bn => new BenhNhanDTO
            {
                IdbenhNhan = bn.IdbenhNhan,
                HoTen = bn.HoTen,
                Gioi = bn.Gioi,
                NamSinh = bn.NamSinh,
                Sdt = bn.Sdt,
                DiaChi = bn.DiaChi,
                TrieuChung = bn.TrieuChung,
                NgayKhamDau = bn.NgayKhamDau
            }).ToList();

            return Ok(benhNhanDTOs);
        }
    }
}
