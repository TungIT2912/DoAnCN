using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.DTO;

namespace WebQuanLyNhaKhoa.Controllers.ApiConrtroller
{
    //[Authorize]
    [Route("[controller]")]
    public class BookAppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookAppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }
        //[Authorize(Roles = "Admin")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("api/GetBenhNhan")]
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
        //[Authorize(Roles = "Admin")]
        [HttpGet("Create")]
        public IActionResult Create()
        {

            return View();
        }
        // POST: api/BenhNhan
        //[Authorize(Roles = "Admin,Staff")]
        [HttpPost("api/PostBenhNhan")]
        public async Task<ActionResult<BenhNhan>> PostBenhNhan([FromBody] BenhNhanDTO benhNhan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var test = new BenhNhan
            {
                HoTen = benhNhan.HoTen,
                Gioi = benhNhan.Gioi,
                NamSinh = benhNhan.NamSinh,
                Sdt = benhNhan.Sdt,
                DiaChi = benhNhan.DiaChi,
                NgayKhamDau  = benhNhan.NgayKhamDau
            };
            _context.BenhNhans.Add(test);
            var nhanVienSaveResult= await _context.SaveChangesAsync();
            if (nhanVienSaveResult > 0)
            {
                var createdNhanVienDTO = new BenhNhanDTO
                {
                   HoTen = test.HoTen,
                   Gioi = test.Gioi,
                   NamSinh = test.NamSinh,
                   Sdt= test.Sdt,
                   DiaChi   = test.DiaChi,
                   NgayKhamDau = test.NgayKhamDau,
                };


                return CreatedAtAction(nameof(GetBenhhNhans), new { id = benhNhan.IdbenhNhan }, createdNhanVienDTO);
            }

            // If any save operation fails, return a BadRequest with an error message
            return BadRequest(new { success = false, message = "Failed to create NhanVien." });

        }
    }
}
