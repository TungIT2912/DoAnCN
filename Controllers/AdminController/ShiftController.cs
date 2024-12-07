using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.DTO;

namespace WebQuanLyNhaKhoa.Controllers.AdminController
{

    [Route("[controller]")]
    public class ShiftController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShiftController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> CreateMultipleShifts([FromBody] List<ShiftDTO> shiftDtos)
        {
            
            if (shiftDtos == null || !shiftDtos.Any())
            {
                return BadRequest("Shift list cannot be empty.");
            }

            var s = new List<Shift>();

            foreach (var shiftDto in shiftDtos)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var shift = new Shift
                {
                    MaNv = shiftDto.MaNv,
                    DayOfWeek = shiftDto.DayOfWeek,
                    StartTime = shiftDto.StartTime,
                    EndTime = shiftDto.EndTime
                };

                s.Add(shift);
            }
            _context.Shifts.AddRange(s);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"{s.Count} shifts created successfully.", s });
        }
    
        [HttpGet("get/shift/{MaNv}")]
        public async Task<ActionResult<ShiftDTO>> GetShiftOfStaff(int MaNv)
        {

            if (MaNv == null )
            {
                return BadRequest("Không tồn tại nhân viên này.");
            }

            var shifts = await _context.Shifts
            .Where(s => s.MaNv == MaNv)
            .ToListAsync();

            if (!shifts.Any())
            {
                return NotFound($"No shifts found for Employee ID {MaNv}.");
            }

            return Ok(shifts);
        }
    }
}
