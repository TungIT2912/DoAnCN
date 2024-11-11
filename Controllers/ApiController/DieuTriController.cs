//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using WebQuanLyNhaKhoa.Data;
//using System.Threading.Tasks;
//using System.Collections.Generic;

//namespace WebQuanLyNhaKhoa.Controllers
//{
//    [Route("[controller]")]
//    public class DieuTriController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public DieuTriController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        // API Endpoint to get all treatments
//        [HttpGet("api")]
//        public async Task<ActionResult<IEnumerable<DieuTri>>> GetAllDieuTrisApi()
//        {
//            var treatments = await _context.DieuTris
//                .Include(dt => dt.DichVu)
//                .Include(dt => dt.Kho)
//                .Include(dt => dt.DanhSachKham)
//                .ToListAsync();
//            return Ok(treatments);
//        }

//        // View action to list all treatments
//        [HttpGet]
//        public async Task<IActionResult> Index()
//        {
//            var treatments = await _context.DieuTris
//                .Include(dt => dt.DichVu)
//                .Include(dt => dt.Kho)
//                .Include(dt => dt.DanhSachKham)
//                .ToListAsync();
//            return View(treatments);
//        }

//        // API Endpoint to create a new treatment
//        [HttpPost("api")]
//        public async Task<ActionResult<DieuTri>> CreateDieuTriApi([FromBody] DieuTri newDieuTri)
//        {
//            var danhSachKham = await _context.DanhSachKhams.FindAsync(newDieuTri.Idkham);
//            var dichVu = await _context.DichVus.FindAsync(newDieuTri.IddichVu);
//            if (danhSachKham == null || dichVu == null)
//            {
//                return BadRequest("Invalid diagnosis or service.");
//            }

//            newDieuTri.ThanhTien = dichVu.DonGia * newDieuTri.SoLuong;
//            _context.DieuTris.Add(newDieuTri);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction(nameof(GetDieuTriApi), new { id = newDieuTri.IddieuTri }, newDieuTri);
//        }

//        // View action to create a new treatment
//        [HttpGet("Create")]
//        public IActionResult Create()
//        {
//            return View();
//        }

//        [HttpPost("Create")]
//        public async Task<IActionResult> Create(DieuTri newDieuTri)
//        {
//            var danhSachKham = await _context.DanhSachKhams.FindAsync(newDieuTri.Idkham);
//            var dichVu = await _context.DichVus.FindAsync(newDieuTri.IddichVu);
//            if (danhSachKham == null || dichVu == null)
//            {
//                ModelState.AddModelError("", "Invalid diagnosis or service.");
//                return View(newDieuTri);
//            }

//            newDieuTri.ThanhTien = dichVu.DonGia * newDieuTri.SoLuong;
//            _context.DieuTris.Add(newDieuTri);
//            await _context.SaveChangesAsync();

//            return RedirectToAction(nameof(Index));
//        }

//        // API Endpoint to get a treatment by ID
//        [HttpGet("api/{id}")]
//        public async Task<ActionResult<DieuTri>> GetDieuTriApi(int id)
//        {
//            var dieuTri = await _context.DieuTris
//                .Include(dt => dt.DichVu)
//                .Include(dt => dt.Kho)
//                .Include(dt => dt.DanhSachKham)
//                .FirstOrDefaultAsync(dt => dt.IddieuTri == id);

//            if (dieuTri == null)
//            {
//                return NotFound();
//            }

//            return dieuTri;
//        }

//        // View action to display details of a treatment
//        [HttpGet("Details/{id}")]
//        public async Task<IActionResult> Details(int id)
//        {
//            var dieuTri = await _context.DieuTris
//                .Include(dt => dt.DichVu)
//                .Include(dt => dt.Kho)
//                .Include(dt => dt.DanhSachKham)
//                .FirstOrDefaultAsync(dt => dt.IddieuTri == id);

//            if (dieuTri == null)
//            {
//                return NotFound();
//            }

//            return View(dieuTri);
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebQuanLyNhaKhoa.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebQuanLyNhaKhoa.Controllers
{
    [Route("[controller]")]
    public class DieuTriController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DieuTriController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View(); // Renders Create.cshtml view for input form
        }
        // Fetch treatments for API
        [HttpGet("api/GetDieuTri")]
        public async Task<ActionResult<IEnumerable<DieuTriDTO>>> GetAllDieuTri()
        {
            var treatments = await _context.DieuTris
                .Include(dt => dt.DichVu)
                .Include(dt => dt.DanhSachKham)
                .Select(dt => new DieuTriDTO
                {
                    IddieuTri = dt.IddieuTri,
                    IddichVu = dt.IddichVu,
                    Idkham = dt.Idkham,
                    SoLuong = dt.SoLuong,
                    ThanhTien = dt.ThanhTien
                })
                .ToListAsync();
            return Ok(treatments); // Returns list of treatments as JSON
        }

        // Render the form to create a new treatment
        [HttpGet("api/GetDieuTriOptions")]
        public async Task<IActionResult> GetDieuTriOptions()
        {
            try
            {
                var availableDichVus = await _context.DichVus.ToListAsync();
                var availableBenhNhans = await _context.BenhNhans.ToListAsync();
                var availableDungCus = await _context.Khos.ToListAsync();

                var options = new
                {
                    DichVus = availableDichVus.Select(dv => new { dv.IddichVu, dv.TenDichVu }).ToList(),
                    BenhNhans = availableBenhNhans.Select(bn => new { bn.IdbenhNhan, bn.HoTen }).ToList(),
                    DungCus = availableDungCus.Select(dc => new { dc.IddungCu, dc.Loai }).ToList(),
                };

                return Ok(options);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching options: " + ex.Message);
            }
        }





        // Fetch services
        [HttpGet("api/GetDichVuOptions")]
        public async Task<ActionResult<IEnumerable<OptionDTO>>> GetDichVuOptions()
        {
            var options = await _context.DichVus
                .Select(dv => new OptionDTO { Id = dv.IddichVu, Name = dv.TenDichVu })
                .ToListAsync();
            return Ok(options);
        }

        // Fetch patients
        [HttpGet("api/GetDanhSachKhamOptions")]
        public async Task<ActionResult<IEnumerable<OptionDTO>>> GetDanhSachKhamOptions()
        {
            var options = await _context.DanhSachKhams
                .Select(ds => new OptionDTO { Id = ds.Idkham, Name = ds.BenhNhan.HoTen })
                .ToListAsync();
            return Ok(options);
        }

        // Fetch tools
        [HttpGet("api/GetDungCuOptions")]
        public async Task<ActionResult<IEnumerable<OptionDTO>>> GetDungCuOptions()
        {
            var options = await _context.Khos
                .Select(dc => new OptionDTO { Id = dc.IddungCu, Name = dc.ThiTruong.TenSanPham })
                .ToListAsync();
            return Ok(options);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View(); // Just return the view without populating ViewBag or a ViewModel
        }


        // Create a new treatment via API and also generate HoaDon and ChiTietHoaDon
        [HttpPost("api/PostDieuTri")]
        public async Task<ActionResult<DieuTriDTO>> CreateDieuTriWithInvoices([FromBody] DieuTriDTO newDieuTriDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
               
                // Validate related entities
                var dichVu = await _context.DichVus.FindAsync(newDieuTriDto.IddichVu);
                var danhSachKham = await _context.DanhSachKhams.FindAsync(newDieuTriDto.Idkham);
                var dungCu = await _context.Khos.FindAsync(newDieuTriDto.IddungCu);
                if (dichVu == null || danhSachKham == null || dungCu == null)
                {
                    return BadRequest("Invalid service or diagnosis ID.");
                }

                if (dungCu.SoLuong < newDieuTriDto.SoLuong)
                {
                    return BadRequest("Not enough tool quantity in stock.");
                }

                decimal treatmentCost = dichVu.DonGia * newDieuTriDto.SoLuong;

                var newDieuTri = new DieuTri
                {
                    IddichVu = newDieuTriDto.IddichVu,
                    Idkham = newDieuTriDto.Idkham,
                    IddungCu = newDieuTriDto.IddungCu,
                    SoLuong = newDieuTriDto.SoLuong,
                    ThanhTien = treatmentCost
                };
                _context.DieuTris.Add(newDieuTri);
                await _context.SaveChangesAsync();

                dungCu.SoLuong -= newDieuTriDto.SoLuong;
                _context.Khos.Update(dungCu);
                await _context.SaveChangesAsync();

                var newHoaDon = new HoaDon
                {
                    Idkham = newDieuTri.Idkham,
                    IddieuTri = newDieuTri.IddieuTri,
                    PhuongThucThanhToan = "Cash",  // Or assign as needed
                    TienDieuTri = treatmentCost,
                    TienThuoc = 0,  // Initially no medication cost
                    TongTien = treatmentCost,
                    NgayLap = DateTime.Now,
                   
                };
                _context.HoaDons.Add(newHoaDon);
                await _context.SaveChangesAsync();
                // Commit transaction
                await transaction.CommitAsync();

                // Update the DTO with the new treatment ID and return it
                newDieuTriDto.IddieuTri = newDieuTri.IddieuTri;
                newDieuTriDto.ThanhTien = treatmentCost;

                return CreatedAtAction(nameof(CreateDieuTriWithInvoices), new { id = newDieuTriDto.IddieuTri }, newDieuTriDto);
            }
            catch
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "An error occurred while creating the treatment and associated invoices.");
            }
        }

        // Update treatment via API
        [HttpPut("api/PutDieuTri/{id}")]
        public async Task<IActionResult> UpdateDieuTri(int id, [FromBody] DieuTriDTO updatedDieuTriDto)
        {
            var treatment = await _context.DieuTris.FindAsync(id);
            if (treatment == null) return NotFound();

            treatment.IddichVu = updatedDieuTriDto.IddichVu;
            treatment.Idkham = updatedDieuTriDto.Idkham;
            treatment.SoLuong = updatedDieuTriDto.SoLuong;
            treatment.ThanhTien = updatedDieuTriDto.ThanhTien;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Delete treatment via API
        [HttpDelete("api/DeleteDieuTri/{id}")]
        public async Task<IActionResult> DeleteDieuTri(int id)
        {
            var treatment = await _context.DieuTris.FindAsync(id);
            if (treatment == null) return NotFound();

            _context.DieuTris.Remove(treatment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // View details of a treatment
        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var treatment = await _context.DieuTris
                .Include(dt => dt.DichVu)
                .Include(dt => dt.DanhSachKham)
                .FirstOrDefaultAsync(dt => dt.IddieuTri == id);

            if (treatment == null) return NotFound();

            return View(treatment); // Renders Details.cshtml with treatment data
        }
    }
}
