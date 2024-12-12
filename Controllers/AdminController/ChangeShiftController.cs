using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.DTO;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace WebQuanLyNhaKhoa.Controllers.AdminController
{
    [Route("Admin/[controller]")]
    public class ChangeShiftController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;
        public ChangeShiftController(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }

        [HttpGet("api/ListShift")]
        public async Task<ActionResult> GetChanges(int pageNumber = 1, int pageSize = 10)
        {
            var totalItems = await _context.RegisForms.CountAsync();

            var nhanViens = await _context.RegisForms
                .Include(nv => nv.NhanVien)
                .Include(nv => nv.NhanVien.ChucVu)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var nhanVienDTOs = nhanViens.Select(nv => new RegisFormDTO
            {
                RegisFormId = nv.Id,
                MaNv = nv.MaNv,
                Ten = nv.NhanVien.Ten,
                ChucVu = nv.NhanVien.ChucVu.TenCv,
                CreateDay = nv.CreateDay,
                Status = nv.Status,
            }).ToList();

            return Ok(new { data = nhanVienDTOs, totalItems });
        }
        [HttpGet("api/ChangeShift/Detail/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regisForm = await _context.RegisForms
                        .Include(n => n.NewShifts)
                        .Include(n => n.NhanVien)
                            .ThenInclude(n => n.ChucVu)
                        .Include(n => n.NhanVien.DichVu)
                        .Where(n => n.Id == id)
                        .FirstOrDefaultAsync(r => r.Id == id);
            if (regisForm == null)
            {
                Console.WriteLine($"Nhân viên với mã nhân viên bạn vừa tìm không tồn tại.");
                return NotFound();
            }
            var regisFormDTO = new RegisFormDTO
            {
                RegisFormId = regisForm.Id,
                MaNv = regisForm.MaNv,
                Ten = regisForm.NhanVien.Ten,
                ChucVu = regisForm.NhanVien.ChucVu.TenCv,
                chuyenNganh = regisForm.NhanVien.DichVu.TenDichVu,
                CreateDay = regisForm.CreateDay,
                ReasonForChange = regisForm.ReasonForChange,
                Status = regisForm.Status,
                NewShifts = regisForm.NewShifts.Select(ns => new NewShiftDTO
                {
                    RegisFormId = ns.RegisFormId,
                    DayOfWeek = ns.DayOfWeek,
                    StartTime = ns.StartTime,
                    EndTime = ns.EndTime
                }).ToList()
            };


            return View(regisFormDTO);
        }

        [HttpPost("api/ChangeShift/Accept")]
        public async Task<IActionResult> Accept([FromBody] ResponseFormDTO dto)
        {

            var dayMapping = new Dictionary<string, string>
            {
                { "Thứ 2", "monday" },
                { "Thứ 3", "tuesday" },
                { "Thứ 4", "wednesday" },
                { "Thứ 5", "thursday" },
                { "Thứ 6", "friday" }
            };
            foreach (var change in dto.NewShifts) {
                if (!string.IsNullOrEmpty(change.DayOfWeek) && dayMapping.ContainsKey(change.DayOfWeek))
                {
                    change.DayOfWeek = dayMapping[change.DayOfWeek];
                }
            }
            

            var existForm = await _context.RegisForms.FirstOrDefaultAsync(n => n.Id == dto.RegisFormId);
            existForm.Status = ShiftChangeStatus.Accepted;
            var respone = new ResponseForm
            {
                RegisFormId = dto.RegisFormId,
                ResponseStatus = ShiftChangeStatus.Accepted,
            };
            var shiftList = await _context.Shifts
                  .Where(n => n.MaNv == dto.MaNv)
                  .ToListAsync();

            var patientList = await _context.DanhSachKhams.Include(n => n.BenhNhan).Include(n => n.NhanVien).ThenInclude(n => n.DichVu)
                             .Where(n => n.MaNv == dto.MaNv &&  n.NgayKham > DateTime.Now)
                             .ToListAsync();

            _context.Shifts.RemoveRange(shiftList);
            var newShifts = dto.NewShifts
                             .Select(shiftDto => new Shift
                             {
                                 MaNv = dto.MaNv ?? 1,
                                 DayOfWeek = shiftDto.DayOfWeek,
                                 StartTime = shiftDto.StartTime,
                                 EndTime = shiftDto.EndTime
                             })
                             .ToList();
            await _context.Shifts.AddRangeAsync(newShifts);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var patient in patientList)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                bool isShiftAssigned = false;

                foreach (var t in dto.NewShifts)
                {
                    if (patient.NgayKham.DayOfWeek.ToString() == t.DayOfWeek && patient.NgayKham.Date > DateTime.Now.Date)
                    {
                        var newEmployee = await _context.Shifts
                            .Include(n => n.NhanVien)
                            .ThenInclude(n => n.DichVu)
                            .Where(n => n.NhanVien.DichVu.TenDichVu == patient.NhanVien.DichVu.TenDichVu
                                && n.DayOfWeek == patient.NgayKham.DayOfWeek.ToString())
                            .ToListAsync();

                        foreach (var employee in newEmployee)
                        {
                            if (employee.MaNv == dto.MaNv)
                            {
                                continue;
                            }

                            var availableShifts = _context.Shifts
                                .Where(s => s.MaNv == employee.MaNv
                                    && s.DayOfWeek == patient.NgayKham.DayOfWeek.ToString()
                                    && s.StartTime <= patient.NgayKham.TimeOfDay
                                    && s.EndTime >= patient.NgayKham.TimeOfDay)
                                .ToList();

                            var availableShiftsWithPatientCount = new List<Shift>();

                            foreach (var shift in availableShifts)
                            {
                                var patientCountForShift = _context.DanhSachKhams
                                    .Where(a => a.MaNv == employee.MaNv
                                                && a.NgayKham.Date == patient.NgayKham.Date
                                                && a.time.TimeOfDay >= shift.StartTime
                                                && a.time.TimeOfDay <= shift.EndTime)
                                    .Count();

                                if (patientCountForShift < 1)
                                {
                                    availableShiftsWithPatientCount.Add(shift);
                                }
                            }

                            if (availableShiftsWithPatientCount.Any())
                            {
                                var selectedShift = availableShiftsWithPatientCount.First();

                                patient.MaNv = employee.MaNv;
                                if (selectedShift.StartTime.HasValue)
                                {
                                    patient.NgayKham = patient.NgayKham.Date.Add(selectedShift.StartTime.Value);
                                    patient.time = patient.NgayKham.Date.Add(selectedShift.StartTime.Value);
                                }
                                else
                                {
                                    return BadRequest("Selected shift does not have a valid start time.");
                                }

                                isShiftAssigned = true;
                                _context.SaveChanges();
                                break;
                            }
                            else
                            {
                                var availableEmployee = _context.Shifts
                                    .Where(s => s.DayOfWeek == patient.NgayKham.DayOfWeek.ToString())
                                    .Select(s => s.MaNv)
                                    .FirstOrDefault();

                                if (availableEmployee != default)
                                {
                                    patient.MaNv = availableEmployee;

                                    var nextAvailableShift = _context.Shifts
                                        .Where(s => s.MaNv == availableEmployee
                                                    && s.DayOfWeek == patient.NgayKham.DayOfWeek.ToString()
                                                    && s.StartTime <= patient.NgayKham.TimeOfDay)
                                        .OrderBy(s => s.StartTime)
                                        .FirstOrDefault();

                                    if (nextAvailableShift != null && nextAvailableShift.StartTime.HasValue)
                                    {
                                        var adjustedStartTime = nextAvailableShift.StartTime.Value.Add(new TimeSpan(1, 0, 0));

                                        patient.NgayKham = patient.NgayKham.Date.Add(adjustedStartTime);
                                        patient.time = patient.NgayKham.Date.Add(adjustedStartTime);
                                        isShiftAssigned = true;
                                        _context.SaveChanges();
                                        break;
                                    }
                                    else
                                    {
                                        return BadRequest($"No available shifts for the employee on {patient.NgayKham.DayOfWeek}.");
                                    }
                                }
                                else
                                {
                                    return BadRequest($"No employees are available on {patient.NgayKham.DayOfWeek}.");
                                }
                            }
                        }

                        if (isShiftAssigned)
                        {
                            break;
                        }
                    }
                    
                }
                foreach (var t in dto.NewShifts)
                {
                    if (isShiftAssigned) break; 

                    if (patient.NgayKham.DayOfWeek.ToString() != t.DayOfWeek && patient.NgayKham.Date > DateTime.Now.Date)
                    {
                        var newEmployee = await _context.Shifts.Include(n => n.NhanVien).ThenInclude(n => n.DichVu)
                            .Where(n => n.NhanVien.DichVu.TenDichVu == patient.NhanVien.DichVu.TenDichVu && n.DayOfWeek == patient.NgayKham.DayOfWeek.ToString()).ToListAsync();

                        foreach (var employee in newEmployee)
                        {
                            if (employee.MaNv == dto.MaNv)
                            {
                                continue;
                            }
                          
                            var availableShifts = _context.Shifts
                                .Where(s => s.MaNv == employee.MaNv
                                    && s.DayOfWeek == patient.NgayKham.DayOfWeek.ToString()
                                    && s.StartTime <= patient.NgayKham.TimeOfDay
                                    && s.EndTime >= patient.NgayKham.TimeOfDay)
                                .ToList();

                            var availableShiftsWithPatientCount = new List<Shift>();

                            foreach (var shift in availableShifts)
                            {
                                var patientCountForShift = _context.DanhSachKhams
                                    .Where(a => a.MaNv == employee.MaNv
                                                && a.NgayKham.Date == patient.NgayKham.Date
                                                && a.time.TimeOfDay >= shift.StartTime
                                                && a.time.TimeOfDay <= shift.EndTime)
                                    .Count();

                                if (patientCountForShift < 3)
                                {
                                    availableShiftsWithPatientCount.Add(shift);
                                }
                            }
                            var patientEmail = patient.BenhNhan.EmailBn;
                            var patientShift = patient.NgayKham.ToString("dd/MM/yyyy HH:mm:ss");
                            var patientDoctor = patient.NhanVien.Ten;
                            var patientTime = patient.time.ToString("hh:mm tt");
                            if (availableShiftsWithPatientCount.Any())
                            {
                                var selectedShift = availableShiftsWithPatientCount.First();

                                patient.MaNv = employee.MaNv;
                                if (selectedShift.StartTime.HasValue)
                                {
                                    patient.NgayKham = patient.NgayKham.Date.Add(selectedShift.StartTime.Value);
                                    patient.time = patient.NgayKham.Date.Add(selectedShift.StartTime.Value);
                                }
                                else
                                {
                                    return BadRequest("Selected shift does not have a valid start time.");
                                }

                                _context.SaveChanges();
                                var emailMessage = string.Empty;
                               
                                var newDoctor = employee.NhanVien.Ten;
                                var newShift = patient.time.TimeOfDay.ToString("hh:mm tt");
                                var newDay = patient.NgayKham.ToString("dd/MM/yyyy HH:mm:ss");
                                var emailSubject = "Thông báo thay đổi ca khám";
                                emailMessage = $"<h1>Thông báo</h1<br><br>" +
                                              $"Chúng tôi rất tiếc khi phải thông báo với bạn rằng .<br><br> " +
                                              $"Bác sĩ {patientDoctor} đã bận vào ngày đó nên chúng tôi sẽ thay đôi bác sĩ mới chính là bác {newDoctor}.<br><br>" +
                                              $"Chúng tôi xin cam đoan bác sĩ mới này có tay nghề tương đương với bác {patientDoctor}.<br><br>" +
                                              $"Ca khám mới của bạn sẽ vào ngày {newDay} và vào lúc {newShift}<br><br>" +
                                              $"Chúng tôi xin chân thành xin lỗi bạn vì sự bất tiện này.<br><br>" +
                                              $"Nếu bạn muốn thay đổi chỉ cần đăng kí lại ngày khác.<br><br>";

                                await _emailService.SendEmailAsync(patientEmail, emailSubject, emailMessage);
                                isShiftAssigned = true; 
                                break; 
                            }
                            else
                            {
                                var availableEmployee = _context.Shifts
                                    .Where(s => s.DayOfWeek == patient.NgayKham.DayOfWeek.ToString())
                                    .Select(s => s.MaNv)
                                    .FirstOrDefault();

                                if (availableEmployee != default)
                                {
                                    patient.MaNv = availableEmployee;

                                    var nextAvailableShift = _context.Shifts
                                        .Where(s => s.MaNv == availableEmployee
                                                    && s.DayOfWeek == patient.NgayKham.DayOfWeek.ToString()
                                                    && s.StartTime <= patient.NgayKham.TimeOfDay)
                                        .OrderBy(s => s.StartTime)
                                        .FirstOrDefault();

                                    if (nextAvailableShift != null && nextAvailableShift.StartTime.HasValue)
                                    {
                                        var adjustedStartTime = nextAvailableShift.StartTime.Value.Add(new TimeSpan(1, 0, 0));

                                        patient.NgayKham = patient.NgayKham.Date.Add(adjustedStartTime);
                                        patient.time = patient.NgayKham.Date.Add(adjustedStartTime);

                                        _context.SaveChanges();
                                        var emailMessage = string.Empty;

                                        var newDoctor = employee.NhanVien.Ten;
                                        var newShift = patient.time.TimeOfDay.ToString("hh:mm tt");
                                        var newDay = patient.NgayKham.ToString("dd/MM/yyyy HH:mm:ss");
                                        var emailSubject = "Thông báo thay đổi ca khám";
                                        emailMessage = $"<h1>Thông báo</h1<br><br>" +
                                                      $"Chúng tôi rất tiếc khi phải thông báo với bạn rằng .<br><br> " +
                                                      $"Bác sĩ {patientDoctor} đã bận vào ngày đó nên chúng tôi sẽ thay đôi bác sĩ mới chính là bác {newDoctor}.<br><br>" +
                                                      $"Chúng tôi xin cam đoan bác sĩ mới này có tay nghề tương đương với bác {patientDoctor}.<br><br>" +
                                                      $"Ca khám mới của bạn sẽ vào ngày {newDay} và vào lúc {newShift}<br><br>" +
                                                      $"Chúng tôi xin chân thành xin lỗi bạn vì sự bất tiện này.<br><br>" +
                                                      $"Nếu bạn muốn thay đổi chỉ cần đăng kí lại ngày khác.<br><br>";

                                        await _emailService.SendEmailAsync(patientEmail, emailSubject, emailMessage);
                                        isShiftAssigned = true; 
                                        break; 
                                    }
                                    else
                                    {
                                        return BadRequest($"No available shifts for the employee on {patient.NgayKham.DayOfWeek}.");
                                    }
                                }
                                else
                                {
                                    return BadRequest($"No employees are available on {patient.NgayKham.DayOfWeek}.");
                                }
                            }
                        }
                    }
                }

            }


            _context.ResponseForms.Add(respone);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = $"{respone} new shifts created and.",
               
            });
        }
    }
}

