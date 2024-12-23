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
            _context.Shifts.RemoveRange(existingShifts);
            var newShifts = new List<Shift>();

            foreach (var shiftDto in shiftDtos)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

               
                    var newShift = new Shift
                    {
                        MaNv = id,
                        DayOfWeek = shiftDto.DayOfWeek,
                        StartTime = shiftDto.StartTime,
                        EndTime = shiftDto.EndTime
                    };
                    newShifts.Add(newShift);
            }

            if (newShifts.Any())
            {
                _context.Shifts.AddRange(newShifts);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = $"{newShifts.Count} new shifts created and  shifts updated successfully.",
                newShifts
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

        [HttpGet("CreateRegisForm/{id}")]
        public IActionResult CreateRegisForm(int id)
        {
            var model = new RegisFormDTO { MaNv = id };
            return View(model);
        }

        [HttpPost("CreateRegisForm")]
        public IActionResult CreateRegisForm(int maNv, string reasonForChange)
        {
            var regisForm = new RegisForm
            {
                MaNv = maNv,
                CreateDay = DateTime.Now,
                ReasonForChange = reasonForChange,
                Status = ShiftChangeStatus.Waiting
            };
            _context.RegisForms.Add(regisForm);
            _context.SaveChanges();

            
            return RedirectToAction("EditShift", new { regisFormId = regisForm.Id });
        }

        [HttpGet("EditShift/{regisFormId}")]
        public async Task<IActionResult> EditShift(int regisFormId)
        {
            var regisForm = await _context.RegisForms
                                          .Include(r => r.NewShifts)
                                          .FirstOrDefaultAsync(r => r.Id == regisFormId);

            if (regisForm == null)
            {
                return NotFound();
            }

           
            var allDays = new List<string> { "monday", "tuesday", "wednesday", "thursday", "friday" };

            
            var existingShifts = await _context.Shifts
                                               .Where(s => s.MaNv == regisForm.MaNv)
                                               .ToListAsync();

            var shiftDto = new ShiftDTO
            {
                MaNv = regisForm.MaNv,
                RegisFormId = regisFormId,
                Shifts = allDays.SelectMany(day => new List<ShiftInfo>
                {
                    new ShiftInfo
                    {
                        DayOfWeek = day,
                        StartTime = TimeSpan.Parse("08:00:00"),
                        EndTime = TimeSpan.Parse("12:00:00"),
                        IsSelected = existingShifts.Any(s => s.DayOfWeek == day && s.StartTime == TimeSpan.Parse("08:00:00"))
                    },
                    new ShiftInfo
                    {
                        DayOfWeek = day,
                        StartTime = TimeSpan.Parse("13:00:00"),
                        EndTime = TimeSpan.Parse("17:00:00"),
                        IsSelected = existingShifts.Any(s => s.DayOfWeek == day && s.StartTime == TimeSpan.Parse("13:00:00"))
                    }
                }).ToList()
            };

            return View(shiftDto);
        }


        [HttpPost("CreateNewShifts/{regisFormId}")]
        public async Task<IActionResult> CreateNewShifts(int regisFormId, [FromBody] List<ShiftUpdateDTO> newShifts)
        {
            var regisForm = await _context.RegisForms
                                          .Include(r => r.NewShifts)
                                          .FirstOrDefaultAsync(r => r.Id == regisFormId);

            if (regisForm == null)
            {
                return BadRequest("Không tồn tại regisFormId.");
            }

            foreach (var shift in newShifts)
            {
                regisForm.NewShifts.Add(new NewShift
                {
                    RegisFormId = regisFormId,
                    DayOfWeek = shift.DayOfWeek,
                    StartTime = (TimeSpan)shift.StartTime,
                    EndTime = (TimeSpan)shift.EndTime
                });
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
