using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.DTO;

namespace WebQuanLyNhaKhoa.Controllers.HomePageCustomer
{
[Route("BookAppointment")]

   public class BookAppointmentController : Controller
   {
   //    private readonly ApplicationDbContext _context;

   //    public BookAppointmentController(ApplicationDbContext context)
   //    {
   //        _context = context;
   //    }
   // //    [Authorize(Roles = "Admin")]
   //    [HttpGet("Index")]
   //    public async Task<IActionResult> Index()
   //    {
   //        return View();
   //    }

   // //    [Authorize(Roles = "Admin")]
   //    [HttpGet("api/GetBenhNhan")]
   //    public async Task<ActionResult<IEnumerable<BenhNhanDTO>>> GetBenhhNhans()
   //    {
   //        var benhNhans = await _context.BenhNhans
   //                       .Include(nv => nv.TaiKhoan)
   //                       .ToListAsync();
   //        var benhNhanDTOs = benhNhans.Select(nv => new BenhNhanDTO
   //        {
   //            IdbenhNhan = nv.IdbenhNhan,
   //            HoTen = nv.HoTen,
   //            Gioi = nv.Gioi,
   //            NamSinh = nv.NamSinh,
   //            Sdt = nv.Sdt,
   //            DiaChi = nv.DiaChi,
   //            NgayKhamDau = nv.NgayKhamDau.ToString(),
   //        }).ToList();

   //        return Ok(benhNhanDTOs);
   //    }
   //    [HttpGet("Create")]
   //    public IActionResult Create(string time)
   //    {
   //             if (!string.IsNullOrEmpty(time))
   //         {
              
   //             var timeParts = time.Split('-');
   //             var startTime = DateTime.Parse(timeParts[0].Trim());
   //             ViewBag.Time = startTime.ToString("yyyy-MM-ddTHH:mm"); 
   //         }
   //         return View();
   //    }
   // //    [Authorize(Roles = "Admin,Staff")]
   //    [HttpPost("api/PostBenhNhan")]
   //    public async Task<ActionResult<BenhNhan>> PostBenhNhan([FromBody] BenhNhan benhNhan)
   //    {
   //        if (!ModelState.IsValid)
   //        {
   //            return BadRequest(ModelState);
   //        }

   //        _context.BenhNhans.Add(benhNhan);
   //        await _context.SaveChangesAsync();

   //        return CreatedAtAction(nameof(GetBenhhNhans), new { id = benhNhan.IdbenhNhan }, benhNhan);
   //    }
   }
}
