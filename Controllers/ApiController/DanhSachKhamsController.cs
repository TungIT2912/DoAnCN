﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.DTO;

namespace WebQuanLyNhaKhoa.Controllers.ApiController
{
    [Route("[controller]")]
    public class DanhSachKhamsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DanhSachKhamsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("PatientsByDate")]
        public IActionResult PatientsByDate()
        {
            return View();
        }

        // API method to retrieve patients based on a specific date string
        [HttpGet("api/GetBenhNhanByDate")]
        public async Task<ActionResult<IEnumerable<BenhNhanDTO>>> GetBenhNhanByDate(string date)
        {
            if (string.IsNullOrEmpty(date) || !DateTime.TryParse(date, out DateTime parsedDate))
            {
                return BadRequest(new { success = false, message = "Invalid or missing date." });
            }

            var benhNhans = await _context.BenhNhans
                                .Where(bn => bn.NgayKhamDau.HasValue && bn.NgayKhamDau.Value.Date == parsedDate.Date)
                                .Include(bn => bn.TaiKhoan)
                                .ToListAsync();

            if (benhNhans == null || !benhNhans.Any())
            {
                return NotFound(new { success = false, message = "No patients found for the specified date." });
            }

            var benhNhanDTOs = benhNhans.Select(bn => new BenhNhanDTO
            {
                IdbenhNhan = bn.IdbenhNhan,
                HoTen = bn.HoTen,
                Gioi = bn.Gioi,
                NamSinh = bn.NamSinh,
                Sdt = bn.Sdt,
                DiaChi = bn.DiaChi,
                TrieuChung = bn.TrieuChung,
                NgayKhamDau = bn.NgayKhamDau.ToString()
            }).ToList();

            return Ok(benhNhanDTOs);
        }



        [HttpGet("AppointmentsByPhone")]
        public IActionResult AppointmentsByPhone()
        {
            return View();
        }
        [HttpGet("api/GetAppointmentsByPhone")]
        public async Task<ActionResult<IEnumerable<DanhSachKhamDTO>>> GetAppointmentsByPhone([FromQuery] string sdt)
        {
            if (string.IsNullOrEmpty(sdt))
            {
                return BadRequest(new { success = false, message = "Số điện thoại không được để trống." });
            }

            // Tìm các bệnh nhân có SDT trùng khớp
            var benhNhans = await _context.BenhNhans
                .Where(bn => bn.Sdt == sdt)
                .ToListAsync();

            // Kiểm tra nếu không có bệnh nhân nào có SDT trùng khớp
            if (benhNhans == null || benhNhans.Count == 0)
            {
                return NotFound(new { success = false, message = "Không tìm thấy bệnh nhân với số điện thoại này." });
            }

            var benhNhanIds = benhNhans.Select(bn => bn.IdbenhNhan).ToList();
            var danhSachKhams = await _context.DanhSachKhams
                .Where(ds => ds.BenhNhan.Sdt == sdt)
                .Include(ds => ds.BenhNhan)
                .ToListAsync();

            var danhSachKhamsDTO = danhSachKhams.Select(ds => new DanhSachKhamDTO
            {
                IdbenhNhan = ds.IdbenhNhan,
                Idkham = ds.Idkham,
                NgayKham = ds.NgayKham,
                hoTenBenhNhan = ds.BenhNhan.HoTen,
                sdtBenhnhan = ds.BenhNhan.Sdt,
                diaChiBenhnhan = ds.BenhNhan.DiaChi

            }).ToList();

            return Ok(danhSachKhamsDTO);
        }

        [HttpGet("PhoneAndDate")]
        public IActionResult PhoneAndDate()
        {
            return View();
        }

        [HttpGet("api/GetBenhNhan")]
        public async Task<ActionResult<IEnumerable<BenhNhanDTO>>> GetBenhNhans(string query = "", string filter = "nothing")
        {
            var currentDay = DateTime.Now;
            var queryable = _context.BenhNhans.AsQueryable();
            var danhSachBenhNhan = await queryable.Select(bn => bn.IdbenhNhan).ToListAsync();
            var time = await _context.DanhSachKhams
            .Where(ds => danhSachBenhNhan.Contains(ds.BenhNhan.IdbenhNhan))
            .Select(ds => ds.time)
            .FirstOrDefaultAsync();
            if (!string.IsNullOrEmpty(query))
            {
                queryable = queryable.Where(n => n.HoTen.Contains(query));
            }

            switch (filter)
            {
                case "Nam":
                    queryable = queryable.Where(n => n.Gioi == false);
                    break;
                case "Nữ":
                    queryable = queryable.Where(n => n.Gioi == true);
                    break;
                case "today":
                    queryable = queryable.Where(n => n.NgayKhamDau.HasValue &&
                        n.NgayKhamDau.Value.Date == currentDay.Date);
                    break;
            }

            var result = await queryable
                .Select(n => new BenhNhanDTO
                {
                    IdbenhNhan = n.IdbenhNhan,
                    HoTen = n.HoTen,
                    Gioi = n.Gioi,
                    Sdt = n.Sdt,
                    NamSinh = n.NamSinh,
                    DiaChi = n.DiaChi,
                    NgayKhamDau = n.NgayKhamDau.Value.ToString("dd/MM/yyyy"),
                    time = time.ToString("HH:mm") ?? "Chưa có thời gian khám"
                })
                .ToListAsync();

            return Ok(result);
        }
    }
}