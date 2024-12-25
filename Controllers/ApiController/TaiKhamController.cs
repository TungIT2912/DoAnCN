using iText.Forms;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.DTO;
using ZXing.QrCode;
using ZXing;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebQuanLyNhaKhoa.Controllers.ApiController
{
    public class TaiKhamController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaiKhamController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string date, string time)
        {
            ViewBag.DichVuList = new SelectList(_context.DichVus, "IddichVu", "TenDichVu");

            if (string.IsNullOrEmpty(date) && string.IsNullOrEmpty(time))
            {

                ViewBag.SelectedDate = "";
                ViewBag.SelectedTime = "";
            }
            else
            {

                ViewBag.SelectedDate = !string.IsNullOrEmpty(date) ? date : DateTime.Now.ToString("yyyy-MM-dd");
                ViewBag.SelectedTime = time ?? "";
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
        [HttpGet("api/CheckBenhNhan")]
        public async Task<IActionResult> CheckBenhNhanAsync(string sdt)
        {
            var benhNhan = await _context.DanhSachKhams
                                 .Include(bn => bn.BenhNhan)
                                 .Include(bn => bn.NhanVien)
                                 .Include(bn => bn.DieuTris)
                                 .ThenInclude(dt => dt.DichVu)
                                 .Include(bn => bn.BenhNhan.HoaDon)
                                 .ThenInclude(dv => dv.DieuTri.DichVu)
                                 .Include(bn => bn.DonThuocs)
                                 .ThenInclude(dt => dt.Kho.ThiTruong)
                                 .Where(n => n.BenhNhan.Sdt == sdt)
                                 .OrderByDescending(n => n.NgayKham) // Sắp xếp theo ngày khám mới nhất
                                 .FirstOrDefaultAsync();

            if (benhNhan == null)
            {
                return NotFound("Bệnh nhân chưa từng đặt lịch.");
            }

            var hoaDon = await _context.HoaDons.FirstOrDefaultAsync(hd => hd.IdhoaDon == benhNhan.BenhNhan.HoaDon.IdhoaDon);
            if (hoaDon == null)
            {
                return BadRequest("Bệnh nhân chưa từng khám tại nha khoa.");
            }

            var response = new
            {
                IdbenhNhan = benhNhan.IdbenhNhan,
                HoTen = benhNhan.BenhNhan?.HoTen ?? "Unknown",
                Gioi = benhNhan.BenhNhan?.Gioi,
                NamSinh = benhNhan.BenhNhan?.NamSinh ?? "Unknown",
                Sdt = benhNhan.BenhNhan?.Sdt ?? "Unknown",
                EmailBn = benhNhan.BenhNhan?.EmailBn ?? "Unknown",
                DiaChi = benhNhan.BenhNhan?.DiaChi ?? "Unknown",

                IddichVu = benhNhan.NhanVien?.IddichVu, 
                MaNv = benhNhan.NhanVien?.MaNv,         
                NgayKhamDau = benhNhan.NgayKham.ToString("yyyy-MM-dd"), 
                Time = benhNhan.time.ToString("HH:mm") 
            };

            return Ok(response);
        }



        [HttpPost("api/PostTaiKham")]
        public async Task<ActionResult<BenhNhanDTO>> PostTaiKham([FromBody] BenhNhanDTO benhNhan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var benhNhan1 = _context.BenhNhans
                                  .Include(bn => bn.HoaDon)
                                  .FirstOrDefault(bn => bn.Sdt == benhNhan.Sdt);
            
            var newDanhSachKham = new DanhSachKham
            {
                IdbenhNhan = benhNhan1.IdbenhNhan,
                NgayKham = DateTime.Parse(benhNhan.NgayKhamDau),
                MaNv = benhNhan.MaNv,
                time = DateTime.Parse(benhNhan.time),
                isTaiKham = true,
            };

            _context.DanhSachKhams.Add(newDanhSachKham);
            var saveDanhSachKhamResult = await _context.SaveChangesAsync();

            if (saveDanhSachKhamResult > 0)
            {
                var nhanVien = await _context.NhanViens.FindAsync(newDanhSachKham.MaNv);
                var qrData = new
                {
                    HoTen = benhNhan.HoTen,
                    Sdt = benhNhan.Sdt,
                    DiaChi = benhNhan.DiaChi,
                    EmailBn = benhNhan.EmailBn,
                    NgayKham = newDanhSachKham.NgayKham.ToString("dd/MM/yyyy"),
                    GioKham = newDanhSachKham.time.ToString("HH:mm"),
                    MaNv = newDanhSachKham.MaNv,
                    BacSiKham = nhanVien.Ten
                };

                var qrContent = System.Text.Json.JsonSerializer.Serialize(qrData);
                var qrFileName = GenerateQrCode(qrContent);
                var qrCodeDownloadUrl = $"{Request.Scheme}://{Request.Host}/BookAppointment/api/downloadQrCode/{qrFileName}";


                var qrCodeImage = $"{Request.Scheme}://{Request.Host}/qr_codes/{qrFileName}";

                var pdfFileName = GenerateLichKhamPdf(benhNhan, newDanhSachKham, nhanVien.Ten);
                var pdfUrl = $"{Request.Scheme}://{Request.Host}/BookAppointment/api/downloadLichKhamPdf/{pdfFileName}";

                var createdBenhNhanDTO = new BenhNhanDTO
                {
                    HoTen = benhNhan.HoTen,
                    Gioi = benhNhan.Gioi,
                    NamSinh = benhNhan.NamSinh,
                    Sdt = benhNhan.Sdt,
                    DiaChi = benhNhan.DiaChi,
                    EmailBn = benhNhan.EmailBn,
                    NgayKhamDau = benhNhan.NgayKhamDau,
                    QrCodeUrl = qrCodeDownloadUrl,
                    QrCodeImage = qrCodeImage,
                    Pdf = pdfUrl
                };

                return CreatedAtAction(nameof(GetBenhhNhans), new { id = benhNhan.IdbenhNhan }, createdBenhNhanDTO);
            }

            return BadRequest(new { success = false, message = "Failed to create appointment." });
        }


        public string GenerateQrCode(string qrContent)
        {
            var qrWriter = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = 200,
                    Width = 200
                }
            };

            var pixelData = qrWriter.Write(qrContent);
            using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppArgb))
            {
                var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                try
                {
                    Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }

                var fileName = Guid.NewGuid().ToString() + ".png";
                var filePath = Path.Combine("wwwroot", "qr_codes", fileName);
                bitmap.Save(filePath, ImageFormat.Png);

                return fileName;
            }
        }


        public string GenerateLichKhamPdf(BenhNhanDTO benhNhan, DanhSachKham danhSachKham, string tenBacSi)
        {
            try
            {
                // Đường dẫn file PDF mẫu
                var templatePath = Path.Combine("wwwroot", "templates", "phieu_dang_ky_kham.pdf");

                // Đường dẫn để lưu file PDF đã điền
                var fileName = Guid.NewGuid().ToString() + ".pdf";
                var outputPath = Path.Combine("wwwroot", "lich_kham_pdfs", fileName);

                // Kiểm tra sự tồn tại của thư mục đầu ra
                var outputDirectory = Path.GetDirectoryName(outputPath);
                if (!Directory.Exists(outputDirectory))
                {
                    throw new DirectoryNotFoundException($"Thư mục đầu ra {outputDirectory} không tồn tại.");
                }

                // Kiểm tra sự tồn tại của file PDF mẫu
                if (!System.IO.File.Exists(templatePath))
                {
                    throw new FileNotFoundException($"File tại đường dẫn {templatePath} không tồn tại.");
                }

                Console.WriteLine($"Mở file PDF mẫu từ {templatePath}");
                Console.WriteLine($"Lưu file PDF kết quả tại {outputPath}");

                // Mở file PDF mẫu
                using (var pdfReader = new PdfReader(templatePath))
                using (var pdfWriter = new PdfWriter(outputPath))
                using (var pdfDocument = new PdfDocument(pdfReader, pdfWriter))
                {
                    Console.WriteLine("Đã mở file PDF mẫu thành công");

                    // Lấy form từ file PDF
                    var form = PdfAcroForm.GetAcroForm(pdfDocument, true);
                    var fields = form.GetAllFormFields();
                    Console.WriteLine("Đã lấy form thành công");

                    // Kiểm tra và điền dữ liệu vào từng trường
                    if (fields.ContainsKey("HoTen")) fields["HoTen"].SetValue(benhNhan.HoTen);
                    if (fields.ContainsKey("Email")) fields["Email"].SetValue(benhNhan.EmailBn);
                    if (fields.ContainsKey("Sdt")) fields["Sdt"].SetValue(benhNhan.Sdt);
                    if (fields.ContainsKey("NamSinh")) fields["NamSinh"].SetValue(benhNhan.NamSinh);
                    if (fields.ContainsKey("DiaChi")) fields["DiaChi"].SetValue(benhNhan.DiaChi);
                    if (fields.ContainsKey("TrieuChung")) fields["TrieuChung"].SetValue(benhNhan.TrieuChung);
                    if (fields.ContainsKey("NgayKham")) fields["NgayKham"].SetValue(danhSachKham.NgayKham.ToString("dd/MM/yyyy"));
                    if (fields.ContainsKey("GioKham")) fields["GioKham"].SetValue(danhSachKham.time.ToString("HH:mm"));
                    if (fields.ContainsKey("TenBacSi")) fields["TenBacSi"].SetValue(tenBacSi);

                    // Làm phẳng form (khóa form để không chỉnh sửa sau này)
                    form.FlattenFields();
                    Console.WriteLine("Đã điền và làm phẳng các trường thành công");
                }

                Console.WriteLine("Đã đóng file PDF và hoàn tất");
                return fileName;
            }
            catch (Exception ex)
            {
                // Ghi log hoặc xử lý lỗi
                Console.WriteLine($"Error generating PDF: {ex.Message}");
                throw;
            }
        }
        
        [HttpGet("api/downloadQrCode/{fileName}")]
        public IActionResult DownloadQrCode(string fileName)
        {
            var filePath = Path.Combine("wwwroot", "qr_codes", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var mimeType = "image/png";
            return File(fileBytes, mimeType, fileName);
        }

        [HttpGet("api/downloadLichKhamPdf/{fileName}")]
        public IActionResult DownloadLichKhamPdf(string fileName)
        {
            // Đường dẫn tới thư mục lưu file PDF
            var folderPath = Path.Combine("wwwroot", "lich_kham_pdfs");
            var filePath = Path.Combine(folderPath, fileName);

            // Kiểm tra file có tồn tại không
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new { success = false, message = "File not found." });
            }

            // Trả file về client
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var contentType = "application/pdf";

            return File(fileBytes, contentType, fileName);
        }

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
                return BadRequest("Xin lỗi hôm nay la ngày " + DateTime.Now.ToString("dd/MM/yyyy") + " ngày bạn chọn " + selectedDate.ToString("dd/MM/yyyy") + " đã qua ngày đó.");
            }

            var employeeShifts = _context.Shifts
                .Where(s => s.MaNv == maNv && s.DayOfWeek == selectedDate.DayOfWeek.ToString())
                .ToList();
            var employees = _context.Shifts
               .Where(s => s.MaNv == maNv)
               .ToList();
            if (!employeeShifts.Any())
            {
                var workingDays = employees.Select(s => s.DayOfWeek).Distinct();

                string workingDaysStr = string.Join(", ", workingDays.Select(day => day switch
                {
                    "monday" => "Thứ hai",
                    "tuesday" => "Thứ ba",
                    "wednesday" => "Thứ tư",
                    "thursday" => "Thứ năm",
                    "friday" => "Thứ sáu",
                    _ => day
                }));

                return BadRequest("Xin lỗi, bác sĩ bạn muốn không làm vào ngày bạn chọn. Bác sĩ làm vào các ngày sau: " + workingDaysStr);
            }
            var availableSlots = new List<string>();
            foreach (var shift in employeeShifts)
            {
                availableSlots.AddRange(GenerateTimeSlotsForShift(selectedDate, shift.StartTime, shift.EndTime));
            }
            var existingAppointments = _context.DanhSachKhams
                .Where(a => a.MaNv == maNv && a.NgayKham.Date == selectedDate.Date)
                .GroupBy(a => a.time)
                .Select(g => new { Time = g.Key, Count = g.Count() })
                .ToList();

            List<string> disabledSlots = new List<string>();
            foreach (var slot in availableSlots.ToList())
            {
                var timeRange = slot.Split(" - ");
                if (timeRange.Length != 2) continue;

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


        private List<string> GenerateTimeSlotsForShift(DateTime selectedDate, TimeSpan? start, TimeSpan? end, int slotDurationMinutes = 60)
        {
            var slots = new List<string>();

            if (start == null || end == null)
                throw new ArgumentException("Start and end times must not be null.");

            if (start >= end)
                throw new ArgumentException("Start time must be earlier than end time.");

            var startTime = selectedDate.Date.Add(start.Value);
            var endTime = selectedDate.Date.Add(end.Value);

            if (selectedDate.Date == DateTime.Now.Date)
            {
                var now = DateTime.Now;
                if (now > startTime)
                {
                    startTime = now.AddMinutes(slotDurationMinutes - (now.Minute % slotDurationMinutes));
                }
            }

            while (startTime < endTime)
            {
                var nextSlot = startTime.AddMinutes(slotDurationMinutes);
                if (nextSlot > endTime) nextSlot = endTime;

                slots.Add($"{startTime:hh\\:mm tt} - {nextSlot:hh\\:mm tt}");
                startTime = nextSlot;
            }

            return slots;
        }

    }
}
