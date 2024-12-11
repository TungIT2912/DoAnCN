using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [HttpGet("Create/{id}")]
        public async Task<IActionResult> Create(int id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.Shifts
                        .Include(n => n.NhanVien)
                        .FirstOrDefaultAsync(n => n.MaNv == id);
            if (nhanVien == null)
            {
                return View(new ShiftDTO
                {
                    MaNv = id, 
                    DayOfWeek = null,
                    StartTime = null,
                    EndTime = null
                });
            }

            var nhanVienDTO = new ShiftDTO
            {
                MaNv = id,
                DayOfWeek  = nhanVien.DayOfWeek,
                StartTime = nhanVien.StartTime,
                EndTime = nhanVien.EndTime,
            };
            return View(nhanVienDTO);
        }

        [HttpPut("bulk/{id}")]
        public async Task<IActionResult> CreateMultipleShifts(int id, [FromBody] List<ShiftDTO> shiftDtos)
        {
            if (shiftDtos == null || !shiftDtos.Any())
            {
                return BadRequest("Shift list cannot be empty.");
            }

            var existingShifts = await _context.Shifts
                .Where(s => s.MaNv == id)
                .ToListAsync();

            var newShifts = new List<Shift>();
            var updatedShifts = new List<Shift>();

            foreach (var shiftDto in shiftDtos)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingShift = existingShifts.FirstOrDefault(s => s.DayOfWeek == shiftDto.DayOfWeek);
                if (existingShift != null)
                {
                    existingShift.StartTime = shiftDto.StartTime;
                    existingShift.EndTime = shiftDto.EndTime;
                    updatedShifts.Add(existingShift);
                }
                else
                {
                    var newShift = new Shift
                    {
                        MaNv = id,
                        DayOfWeek = shiftDto.DayOfWeek,
                        StartTime = shiftDto.StartTime,
                        EndTime = shiftDto.EndTime
                    };
                    newShifts.Add(newShift);
                }
            }

            if (newShifts.Any())
            {
                _context.Shifts.AddRange(newShifts);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = $"{newShifts.Count} new shifts created and {updatedShifts.Count} shifts updated successfully.",
                newShifts,
                updatedShifts
            });
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
