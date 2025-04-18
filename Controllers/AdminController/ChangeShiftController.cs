﻿using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
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
                bool isMatch = false;

                foreach (var t in dto.NewShifts)
                {
                    if (patient.NgayKham.DayOfWeek.ToString().ToLower() == t.DayOfWeek.ToLower() && patient.NgayKham.Date > DateTime.Now.Date)
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
                        isMatch = true;
                        continue;    
                        }
                    }
                }
                foreach (var t in dto.NewShifts)
                {
                    if (isMatch) break;
                    if (isShiftAssigned) break; 

                    if (patient.NgayKham.DayOfWeek.ToString().ToLower() != t.DayOfWeek.ToLower() && patient.NgayKham.Date > DateTime.Now.Date)
                    {
                        var newEmployee = await _context.Shifts.Include(n => n.NhanVien).ThenInclude(n => n.DichVu)
                            .Where(n => n.NhanVien.DichVu.TenDichVu == patient.NhanVien.DichVu.TenDichVu && n.DayOfWeek.ToLower() == patient.NgayKham.DayOfWeek.ToString().ToLower()).ToListAsync();
                       
                        foreach (var employee in newEmployee)
                        {
                            if (employee.MaNv == dto.MaNv)
                            {
                                continue;
                            }
                          
                            var availableShifts = _context.Shifts
                                .Where(s => s.MaNv == employee.MaNv
                                    && s.DayOfWeek == patient.NgayKham.DayOfWeek.ToString().ToLower()
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
                                var newShift = patient.time.ToString("hh:mm tt");
                                var newDay = patient.NgayKham.ToString("dd/MM/yyyy");
                                var emailSubject = "Thông báo thay đổi ca khám";
                                emailMessage = $@"<!DOCTYPE html>
                                                <html>
                                                <head>
                                                    <style>
                                                        body {{
                                                            font-family: Arial, sans-serif;
                                                            line-height: 1.6;
                                                            margin: 0;
                                                            padding: 0;
                                                            background-color: #f4f4f9;
                                                            color: #333;
                                                        }}
                                                        .email-container {{
                                                            max-width: 600px;
                                                            margin: 20px auto;
                                                            background: #ffffff;
                                                            padding: 20px;
                                                            border-radius: 10px;
                                                            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                                                        }}
                                                        h1 {{
                                                            color: #007bff;
                                                            text-align: center;
                                                        }}
                                                        p {{
                                                            margin: 15px 0;
                                                        }}
                                                        .highlight {{
                                                            font-weight: bold;
                                                            color: #007bff;
                                                        }}
                                                        .button-container {{
                                                            text-align: center;
                                                            margin: 20px 0;
                                                        }}
                                                        a.button {{
                                                            background-color: #007bff;
                                                            color: #ffffff;
                                                            text-decoration: none;
                                                            padding: 10px 20px;
                                                            border-radius: 5px;
                                                            font-size: 16px;
                                                        }}
                                                        a.button:hover {{
                                                            background-color: #0056b3;
                                                        }}
                                                    </style>
                                                </head>
                                                <body>
                                                    <div class='email-container'>
                                                        <h1>Thông Báo</h1>
                                                        <p>Chúng tôi rất tiếc khi phải thông báo với bạn rằng:</p>
                                                        <p>Bác sĩ <span class='highlight'>{patientDoctor}</span> đã bận vào ngày đã đăng ký.</p>
                                                        <p>Chúng tôi sẽ thay thế bằng bác sĩ mới, đó là bác sĩ <span class='highlight'>{newDoctor}</span>.</p>
                                                        <p>Chúng tôi xin cam đoan rằng bác sĩ <span class='highlight'>{newDoctor}</span> có tay nghề tương đương với bác sĩ <span class='highlight'>{patientDoctor}</span>.</p>
                                                        <p>Ca khám mới của bạn sẽ diễn ra vào:</p>
                                                        <ul>
                                                            <li><strong>Ngày:</strong> <span class='highlight'>{newDay}</span></li>
                                                            <li><strong>Giờ:</strong> <span class='highlight'>{newShift}</span></li>
                                                        </ul>
                                                        <p>Chúng tôi xin chân thành xin lỗi vì sự bất tiện này.</p>
                                                        <p>Nếu bạn muốn thay đổi, chỉ cần đăng ký lại ngày khác.</p>
                                                        <p>Trân trọng,<br>Phòng Khám</p>
                                                    </div>
                                                </body>
                                                </html>
                            
                                ";

                                await _emailService.SendEmailAsync(patientEmail, emailSubject, emailMessage);
                                isShiftAssigned = true; 
                                break; 
                            }
                            else
                            {
                                var availableEmployee = _context.Shifts
                                    .Where(s => s.DayOfWeek.ToLower() == patient.NgayKham.DayOfWeek.ToString())
                                    .Select(s => s.MaNv)
                                    .FirstOrDefault();

                                if (availableEmployee != default)
                                {
                                    patient.MaNv = availableEmployee;

                                    var nextAvailableShift = _context.Shifts
                                        .Where(s => s.MaNv == availableEmployee
                                                    && s.DayOfWeek.ToLower() == patient.NgayKham.DayOfWeek.ToString().ToLower()
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
                                        var newShift = patient.time.ToString("hh:mm tt");
                                        var newDay = patient.NgayKham.ToString("dd/MM/yyyy");
                                        var emailSubject = "Thông báo thay đổi ca khám";
                                        emailMessage = $@"
                                                        <html>
                                                        <head>
                                                            <style>
                                                                body {{
                                                                    font-family: Arial, sans-serif;
                                                                    line-height: 1.6;
                                                                    background-color: #f9f9f9;
                                                                    color: #333;
                                                                    margin: 0;
                                                                    padding: 0;
                                                                }}
                                                                .email-container {{
                                                                    max-width: 600px;
                                                                    margin: 20px auto;
                                                                    background: #ffffff;
                                                                    padding: 20px;
                                                                    border-radius: 8px;
                                                                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                                                                }}
                                                                h1 {{
                                                                    color: #007bff;
                                                                    text-align: center;
                                                                    margin-bottom: 20px;
                                                                }}
                                                                p {{
                                                                    margin: 15px 0;
                                                                }}
                                                                .highlight {{
                                                                    font-weight: bold;
                                                                    color: #007bff;
                                                                }}
                                                            </style>
                                                        </head>
                                                        <body>
                                                            <div class='email-container'>
                                                                <h1>Thông Báo</h1>
                                                                <p>Chúng tôi rất tiếc khi phải thông báo với bạn rằng:</p>
                                                                <p>Bác sĩ <span class='highlight'>{patientDoctor}</span> đã bận vào ngày đó. Vì vậy, chúng tôi sẽ thay đổi bác sĩ mới chính là bác sĩ <span class='highlight'>{newDoctor}</span>.</p>
                                                                <p>Chúng tôi cam đoan rằng bác sĩ mới này có tay nghề tương đương với bác sĩ <span class='highlight'>{patientDoctor}</span>.</p>
                                                                <p>Lịch khám mới của bạn sẽ là:</p>
                                                                <ul>
                                                                    <li><strong>Ngày:</strong> <span class='highlight'>{newDay}</span></li>
                                                                    <li><strong>Giờ:</strong> <span class='highlight'>{newShift}</span></li>
                                                                </ul>
                                                                <p>Chúng tôi xin chân thành xin lỗi vì sự bất tiện này.</p>
                                                                <p>Nếu bạn muốn thay đổi, chỉ cần đăng ký lại ngày khác.</p>
                                                                <p>Trân trọng,<br>Phòng Khám</p>
                                                            </div>
                                                        </body>
                                                        </html>";


                                        await _emailService.SendEmailAsync(patientEmail, emailSubject, emailMessage);
                                        isShiftAssigned = true; 
                                        break; 
                                    }
                                    else
                                    {
                                        return BadRequest($"Không có ca nào trống {patient.NgayKham.DayOfWeek}.");
                                    }
                                }
                                else
                                {
                                    return BadRequest($"Không có nhân viên nào rảnh {patient.NgayKham.DayOfWeek}.");
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
        [HttpPost("api/ChangeShift/Deny")]
        public async Task<IActionResult> Deny([FromBody] ResponseFormDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Không nhận được dữ liệu");
            }
            var existForm = await _context.RegisForms.FirstOrDefaultAsync(n => n.Id == dto.RegisFormId);
            existForm.Status = ShiftChangeStatus.Denied;
            var respone = new ResponseForm
            {
                RegisFormId = dto.RegisFormId,
                ResponseStatus = ShiftChangeStatus.Denied,
                Comment = dto.Comment,
            };
            var exitsEmployee = await _context.RegisForms.Include(n => n.NhanVien).FirstOrDefaultAsync(n => n.MaNv == dto.MaNv);
            var emailMessage = string.Empty;
            var employeeEmail = exitsEmployee.NhanVien.Email;
            var newDoctor = dto.Comment;
            var emailSubject = "Thông báo thay vấn đề về việc đổi ca khám";
            emailMessage = $@"
                            <html>
                            <head>
                                <style>
                                    body {{
                                        font-family: Arial, sans-serif;
                                        line-height: 1.6;
                                        background-color: #f9f9f9;
                                        color: #333;
                                        margin: 0;
                                        padding: 0;
                                    }}
                                    .email-container {{
                                        max-width: 600px;
                                        margin: 20px auto;
                                        background: #ffffff;
                                        padding: 20px;
                                        border-radius: 8px;
                                        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                                    }}
                                    h1 {{
                                        color: #007bff;
                                        text-align: center;
                                        margin-bottom: 20px;
                                    }}
                                    p {{
                                        margin: 15px 0;
                                    }}
                                    .highlight {{
                                        font-weight: bold;
                                        color: #007bff;
                                    }}
                                    table {{
                                        width: 100%;
                                        border-collapse: collapse;
                                        margin-top: 20px;
                                    }}
                                    th, td {{
                                        padding: 8px;
                                        text-align: left;
                                        border: 1px solid #ddd;
                                    }}
                                    th {{
                                        background-color: #f2f2f2;
                                    }}
                                </style>
                            </head>
                            <body>
                                <div class='email-container'>
                                    <h1>Thông Báo</h1>
                                    <p>Chúng tôi rất tiếc khi phải thông báo với bạn rằng:</p>
                                    <p>Ca khám mà bạn đăng kí:</p>

                                    <!-- Table to display shifts -->
                                    <table>
                                        <thead>
                                            <tr>
                                                <th>Ca Khám</th>
                                                <th>Giờ bắt đầu</th>
                                                <th>Giờ kết thúc</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {string.Join("", dto.NewShifts.Select(shift => $"<tr>" +
                                            $"<td>{shift.DayOfWeek}</td>" +
                                            $"<td>{shift.StartTime}</td>" +
                                            $"<td>{shift.EndTime}</td>" +
                                            $"</tr>"))}
                                        </tbody>
                                    </table>

                                    <p>Việc thay đổi ca khám của bạn đã bị từ chối.</p>
                                    <p>Lý do: <span class='highlight'>{newDoctor}</span></p>

                                    
                                </div>
                            </body>
                            </html>";


            await _emailService.SendEmailAsync(employeeEmail, emailSubject, emailMessage);

            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = $"{respone} successful to deny this form.",

            });
        }

    }
}

