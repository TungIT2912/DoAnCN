using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                EmailBn = nv.EmailBn,
                NgayKhamDau = nv.NgayKhamDau.ToString(),
            }).ToList();

            return Ok(benhNhanDTOs);
        }
        //[Authorize(Roles = "Admin")]
        [HttpGet("Create")]
        public IActionResult Create(string date, string time)
        {
            ViewBag.DichVuList = new SelectList(_context.DichVus, "IddichVu", "TenDichVu");
            
            if (string.IsNullOrEmpty(date) && string.IsNullOrEmpty(time))
            {
              
                ViewBag.SelectedDate = "null";
                ViewBag.SelectedTime = "null";
            }
            else
            {
             
                ViewBag.SelectedDate = !string.IsNullOrEmpty(date) ? date : DateTime.Now.ToString("yyyy-MM-dd");
                ViewBag.SelectedTime = time ?? "null"; 
            }

            return View();
        }

        [HttpGet("GetNhanViensByDichVu")]
        public IActionResult GetNhanViensByDichVu(int dichVuId)
        {
            var nhanViens = _context.NhanViens
                                   .Where(nv => nv.IddichVu == dichVuId)
                                   .Select(nv => new { Id = nv.MaNv, Ten = nv.Ten })
                                   .ToList();

            return Json(nhanViens);
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



            var newBenhNhan = new BenhNhan
            {
                HoTen = benhNhan.HoTen,
                Gioi = benhNhan.Gioi,
                NamSinh = benhNhan.NamSinh,
                Sdt = benhNhan.Sdt,
                DiaChi = benhNhan.DiaChi,
                TrieuChung = benhNhan.TrieuChung,
                NgayKhamDau = DateTime.Parse(benhNhan.NgayKhamDau),
                EmailBn = benhNhan.EmailBn
            };

            _context.BenhNhans.Add(newBenhNhan);

            //_context.BenhNhans.Add(newBenhNhan); 
            var saveResult = await _context.SaveChangesAsync();

            if (saveResult > 0)
            {
                var newDanhSachKham = new DanhSachKham
                {
                    IdbenhNhan = newBenhNhan.IdbenhNhan,
                    NgayKham = DateTime.Parse(benhNhan.NgayKhamDau), 
                    MaNv = benhNhan.MaNv,
                    time = DateTime.Parse(benhNhan.time),
                };

                _context.DanhSachKhams.Add(newDanhSachKham);
              
                // Lưu lịch khám vào cơ sở dữ liệu
                var saveDanhSachKhamResult = await _context.SaveChangesAsync();
              
               
                await _context.SaveChangesAsync();
                if (saveDanhSachKhamResult > 0)
                {
                    // Trả về thông tin bệnh nhân và lịch khám
                    var createdBenhNhanDTO = new BenhNhanDTO
                    {
                        HoTen = newBenhNhan.HoTen,
                        Gioi = newBenhNhan.Gioi,
                        NamSinh = newBenhNhan.NamSinh,
                        Sdt = newBenhNhan.Sdt,
                        DiaChi = newBenhNhan.DiaChi,
                        EmailBn = newBenhNhan.EmailBn,
                        TrieuChung = newBenhNhan.TrieuChung,
                        NgayKhamDau = newBenhNhan.NgayKhamDau.ToString()
                    };

                    return CreatedAtAction(nameof(GetBenhhNhans), new { id = newBenhNhan.IdbenhNhan }, createdBenhNhanDTO);
                }
                else
                {
                    return BadRequest(new { success = false, message = "Failed to create appointment." });
                }
            }

            return BadRequest(new { success = false, message = "Failed to create appointment." });

        }

        // View method to display patients by date
        //[HttpGet("PatientsByDate")]
        //public IActionResult PatientsByDate()
        //{
        //    return View();
        //}

        //// API method to retrieve patients based on a specific date string
        //[HttpGet("api/GetBenhNhanByDate")]
        //public async Task<ActionResult<IEnumerable<BenhNhanDTO>>> GetBenhNhanByDate(string date)
        //{
        //    if (string.IsNullOrEmpty(date) || !DateTime.TryParse(date, out DateTime parsedDate))
        //    {
        //        return BadRequest(new { success = false, message = "Invalid or missing date." });
        //    }

        //    var benhNhans = await _context.BenhNhans
        //                        .Where(bn => bn.NgayKhamDau.HasValue && bn.NgayKhamDau.Value.Date == parsedDate.Date)
        //                        .Include(bn => bn.TaiKhoan)
        //                        .ToListAsync();

        //    if (benhNhans == null || !benhNhans.Any())
        //    {
        //        return NotFound(new { success = false, message = "No patients found for the specified date." });
        //    }

        //    var benhNhanDTOs = benhNhans.Select(bn => new BenhNhanDTO
        //    {
        //        IdbenhNhan = bn.IdbenhNhan,
        //        HoTen = bn.HoTen,
        //        Gioi = bn.Gioi,
        //        NamSinh = bn.NamSinh,
        //        Sdt = bn.Sdt,
        //        DiaChi = bn.DiaChi,
        //        NgayKhamDau = bn.NgayKhamDau
        //    }).ToList();

        //    return Ok(benhNhanDTOs);
        //}

        [HttpGet("getAvailableSlots")]
        public IActionResult GetAvailableSlots([FromQuery] string ngayKham, [FromQuery] int maNv)
        {
            if (!DateTime.TryParse(ngayKham, out DateTime selectedDate))
            {
                return BadRequest("Invalid date format.");
            }
            if (selectedDate.DayOfWeek == DayOfWeek.Saturday || selectedDate.DayOfWeek == DayOfWeek.Sunday)
            {
                return BadRequest("Dịch vụ chúng tôi không hoạt động vào thứ 7 và chủ nhật.");
            }
            if (selectedDate.Day < DateTime.Now.Day)
            {
                return BadRequest("Xin lỗi hôm nay la ngày "  + DateTime.Now.ToString("dd/MM/yyyy") + " ngày bạn chọn " + selectedDate.ToString("dd/MM/yyyy") + " đã qua ngày đó." );
            }

            var availableSlots = GenerateTimeSlots(selectedDate); 
            var existingAppointments = _context.DanhSachKhams
                .Where(a => a.MaNv == maNv && a.NgayKham.Date == selectedDate.Date  )
                .GroupBy(a => a.time)
                .Select(g => new { Time = g.Key, Count = g.Count() })
                .ToList();

            List<string> disabledSlots = new List<string>();

            foreach (var slot in availableSlots.ToList())
            {
                var timeRange = slot.Split(" - "); 
                if (timeRange.Length != 2)
                {
                    continue;
                }

                var startTime = DateTime.Parse(timeRange[0]);

                if (existingAppointments.Any(a => a.Time.TimeOfDay == startTime.TimeOfDay && a.Count >= 3))
                {
                    disabledSlots.Add(slot);
                }
            }

            return Ok(new
            {
                availableSlots = availableSlots.Except(disabledSlots),
                disabledSlots = disabledSlots
            });
        }


        [HttpPost("checkAvailability")]
        public IActionResult CheckAvailability([FromBody] BenhNhanDTO request)
        {
            DateTime selectedDate = DateTime.Parse(request.NgayKhamDau);
            DateTime selectedTime = DateTime.Parse(request.time);

            var existingAppointments = _context.DanhSachKhams
                .Where(a => a.NgayKham.Date == selectedDate.Date && a.time == selectedTime)
                .ToList();

            if (existingAppointments.Count >= 3)
            {
                return BadRequest("Khung giờ khám này đã kín");
            }

            return Ok("Khung giờ này có thể chọn.");
        }


        private List<string> GenerateTimeSlots(DateTime selectedDate)
        {
            var slots = new List<string>();

            var morningStart = selectedDate.AddHours(8);  
            var morningEnd = selectedDate.AddHours(12); 

            while (morningStart < morningEnd)
            {
                var nextSlot = morningStart.AddHours(1); 
                if (nextSlot > morningEnd) break;

                slots.Add($"{morningStart:hh\\:mm tt} - {nextSlot:hh\\:mm tt}");
                morningStart = nextSlot;
            }

            var afternoonStart = selectedDate.AddHours(13);
            var afternoonEnd = selectedDate.AddHours(17); 

            while (afternoonStart < afternoonEnd)
            {
                var nextSlot = afternoonStart.AddHours(1); 

                if (nextSlot > afternoonEnd) break; 

                slots.Add($"{afternoonStart:hh\\:mm tt} - {nextSlot:hh\\:mm tt}");
                afternoonStart = nextSlot;
            }

            return slots;
        }

    }
}

