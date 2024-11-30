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
        public IActionResult Create()
        {
            ViewBag.DichVuList = new SelectList(_context.DichVus, "IddichVu", "TenDichVu");
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
                };

                _context.DanhSachKhams.Add(newDanhSachKham);

                // Lưu lịch khám vào cơ sở dữ liệu
                var saveDanhSachKhamResult = await _context.SaveChangesAsync();

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

        [HttpPost("getAvailableSlots")]
        public IActionResult GetAvailableSlots([FromBody] BenhNhanDTO request)
        {
            DateTime selectedDate = DateTime.Parse(request.NgayKhamDau);
            var availableSlots = GenerateTimeSlots(selectedDate);
            var doctorId = request.MaNv;
            var existingAppointments = _context.DanhSachKhams
                                       .Where(a => a.MaNv == doctorId && a.NgayKham.Date == selectedDate.Date)
                                       .GroupBy(a => a.time)
                                       .Select(g => new { Time = g.Key, Count = g.Count() })
                                       .ToList();

            List<string> disabledSlots = new List<string>();

            foreach (var slot in availableSlots.ToList())
            {
                var slotString = slot.ToString("HH:mm"); 

             
                if (existingAppointments.Any(a => a.Time.ToString("HH:mm") == slotString && a.Count >= 3))
                {
                 
                    disabledSlots.Add(slotString);
                }
            }

            return Ok(new
            {
                availableSlots = availableSlots
                    .Where(slot => !disabledSlots.Contains(slot.ToString("HH:mm"))) 
                    .Select(slot => slot.ToString("HH:mm")),
                 disabledSlots = disabledSlots
            });
        }


        [HttpPost("checkAvailability")]
        public IActionResult CheckAvailability([FromBody] BenhNhanDTO request)
        {
            DateTime selectedDate = DateTime.Parse(request.NgayKhamDau);
            DateTime selectedTime = DateTime.Parse(request.NgayKhamDau + " " + request.time);

            var existingAppointments = _context.DanhSachKhams
                .Where(a => a.NgayKham.Date == selectedDate.Date && a.time == selectedTime)
                .ToList();

            if (existingAppointments.Count >= 3)
            {
                return BadRequest("Khung giờ khám này đã kín");
            }

            return Ok("Khung giờ này có thể chọn.");
        }

      
        private List<DateTime> GenerateTimeSlots(DateTime selectedDate)
        {
            var slots = new List<DateTime>();
            var start = selectedDate.AddHours(8);  
            var end = selectedDate.AddHours(18);   

            while (start < end)
            {
                slots.Add(start);
                start = start.AddHours(1); 
            }

            return slots;
        }
    }
}

