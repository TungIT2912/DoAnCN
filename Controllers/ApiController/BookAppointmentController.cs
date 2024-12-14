using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing;
using ZXing.QrCode;
using System.Text;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.DTO;
using System.Runtime.InteropServices;
using iText.Forms.Fields;
using iText.Forms;
using iText.Kernel.Pdf;
using System.IO;


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
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
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
                    var saveDanhSachKhamResult = await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    if (saveDanhSachKhamResult > 0)
                    {
                        var nhanVien = await _context.NhanViens.FindAsync(newDanhSachKham.MaNv);
                        // Tạo nội dung mã QR
                        var qrData = new
                        {
                            HoTen = newBenhNhan.HoTen,
                            Sdt = newBenhNhan.Sdt,
                            DiaChi = newBenhNhan.DiaChi,
                            EmailBn = newBenhNhan.EmailBn,
                            TrieuChung = newBenhNhan.TrieuChung,
                            NgayKham = newDanhSachKham.NgayKham.ToString("dd/MM/yyyy"),
                            GioKham = newDanhSachKham.time.ToString("HH:mm"),
                            MaNv = newDanhSachKham.MaNv,
                            BacSiKham = nhanVien.Ten


                        };

                        var qrContent = System.Text.Json.JsonSerializer.Serialize(qrData);
                        var qrFileName = GenerateQrCode(qrContent);
                        var qrCodeDownloadUrl = $"{Request.Scheme}://{Request.Host}/BookAppointment/api/downloadQrCode/{qrFileName}";

                        // Tạo hình ảnh lịch khám
                        var imageFileName = GenerateLichKhamImage(benhNhan, newDanhSachKham, nhanVien.Ten);
                        var imageUrl = $"{Request.Scheme}://{Request.Host}/BookAppointment/api/downloadLichKham/{imageFileName}";
                        var qrCodeImage = $"{Request.Scheme}://{Request.Host}/qr_codes/{qrFileName}";

                        // Tạo file PDF lịch khám
                        var pdfFileName = GenerateLichKhamPdf(benhNhan, newDanhSachKham, nhanVien.Ten);
                        var pdfUrl = $"{Request.Scheme}://{Request.Host}/BookAppointment/api/downloadLichKhamPdf/{pdfFileName}";
                        var createdBenhNhanDTO = new BenhNhanDTO
                        {
                            HoTen = newBenhNhan.HoTen,
                            Gioi = newBenhNhan.Gioi,
                            NamSinh = newBenhNhan.NamSinh,
                            Sdt = newBenhNhan.Sdt,
                            DiaChi = newBenhNhan.DiaChi,
                            EmailBn = newBenhNhan.EmailBn,
                            TrieuChung = newBenhNhan.TrieuChung,
                            NgayKhamDau = newBenhNhan.NgayKhamDau.ToString(),
                            QrCodeUrl = qrCodeDownloadUrl,
                            ImageUrl = imageUrl,
                            QrCodeImage = qrCodeImage,
                            Pdf = pdfUrl
                        };

                        return CreatedAtAction(nameof(GetBenhhNhans), new { id = newBenhNhan.IdbenhNhan }, createdBenhNhanDTO);
                    }
                    else
                    {
                        return BadRequest(new { success = false, message = "Lối khi tạo." });
                    }
                }
            }
            catch
            {
                // Rollback in case of an error
                await transaction.RollbackAsync();
                return BadRequest(new { success = false, message = "Đã có một số lỗi xảy ra xin bạn vui lòng đăng kí lại." });
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

    //public string GenerateLichKhamPdf(BenhNhanDTO benhNhan, DanhSachKham danhSachKham, string tenBacSi)
    //{
    //    // Tạo tên file ngẫu nhiên
    //    var fileName = Guid.NewGuid().ToString() + ".pdf";
    //    var outputPath = Path.Combine("wwwroot", "lich_kham_pdfs", fileName);

    //    // Đường dẫn file PDF mẫu
    //    var templatePath = Path.Combine("wwwroot", "templates", "phieu_dang_ky_kham.pdf");

    //    var outputDirectory = Path.GetDirectoryName(outputPath);
    //    if (!Directory.Exists(outputDirectory))
    //    {
    //        throw new DirectoryNotFoundException($"Thư mục đầu ra {outputDirectory} không tồn tại.");
    //    }
    //    if (!System.IO.File.Exists(templatePath))
    //    {
    //        throw new FileNotFoundException($"File tại đường dẫn {templatePath} không tồn tại.");
    //    }
    //    Console.WriteLine($"Mở file PDF mẫu từ {templatePath}"); Console.WriteLine($"Lưu file PDF kết quả tại {outputPath}");
    //    // Điền dữ liệu vào form
    //    using (PdfDocument pdfDoc = new PdfDocument(new PdfReader(templatePath), new PdfWriter(outputPath)))
    //    {
    //        // Lấy form từ PDF
    //        PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
    //        IDictionary<string, PdfFormField> fields = form.GetAllFormFields();

    //        // Điền thông tin vào các trường
    //        fields["HoTen"].SetValue(benhNhan.HoTen);
    //        fields["Email"].SetValue(benhNhan.EmailBn);
    //        fields["Sdt"].SetValue(benhNhan.Sdt);
    //        fields["NamSinh"].SetValue(benhNhan.NamSinh);
    //        fields["DiaChi"].SetValue(benhNhan.DiaChi);

    //        fields["TrieuChung"].SetValue(benhNhan.TrieuChung);
    //        fields["NgayKham"].SetValue(danhSachKham.NgayKham.ToString("dd/MM/yyyy"));
    //        fields["GioKham"].SetValue(danhSachKham.time.ToString("HH:mm"));

    //        fields["TenBacSi"].SetValue(tenBacSi);

    //        // Làm phẳng form (khóa form để không chỉnh sửa sau này)
    //        form.FlattenFields();

    //    }

    //    return fileName;
    //}


    public string GenerateLichKhamImage(BenhNhanDTO benhNhan, DanhSachKham danhSachKham, string tenBacSi)
        {
            var fileName = Guid.NewGuid().ToString() + ".png";
            var filePath = Path.Combine("wwwroot", "lich_kham_images", fileName);

            using (var bitmap = new Bitmap(600, 400))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.White);

                // Vẽ tiêu đề với font lớn và màu xanh
                using (var fontTitle = new Font("Arial", 20, FontStyle.Bold))
                using (var brushTitle = new SolidBrush(Color.Blue))
                {
                    graphics.DrawString("PHIẾU ĐĂNG KÍ KHÁM", fontTitle, brushTitle, new PointF(150, 20));
                }

                // Vẽ thông tin với font thường và màu đen
                using (var font = new Font("Arial", 14))
                using (var brush = new SolidBrush(Color.Black))
                {
                    float yPosition = 70;
                    float xPosition = 50;
                    float lineSpacing = 30;

                    string[] lines = {
                $"Họ Tên: {benhNhan.HoTen}",
                $"Số ĐT: {benhNhan.Sdt}",
                $"Địa chỉ: {benhNhan.DiaChi}",
                $"Email: {benhNhan.EmailBn}",
                $"Triệu chứng: {benhNhan.TrieuChung}",
                $"Ngày Khám: {danhSachKham.NgayKham:dd/MM/yyyy}",
                $"Giờ Khám: {danhSachKham.time:HH:mm}",
                $"Mã bác Sĩ: {danhSachKham.MaNv}",
                $"Tên bác sĩ: {tenBacSi}"
            };

                    foreach (var line in lines)
                    {
                        graphics.DrawString(line, font, brush, new PointF(xPosition, yPosition));
                        yPosition += lineSpacing;
                    }
                }

                // Thêm đường viền xung quanh hình ảnh
                using (var pen = new Pen(Color.Black, 3))
                {
                    graphics.DrawRectangle(pen, 5, 5, bitmap.Width - 10, bitmap.Height - 10);
                }

                bitmap.Save(filePath, ImageFormat.Png);
            }

            return fileName;
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
        [HttpGet("api/downloadLichKham/{fileName}")]
        public IActionResult DownloadLichKhamImage(string fileName)
        {
            var filePath = Path.Combine("wwwroot", "lich_kham_images", fileName);
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
            if (selectedDate.Date < DateTime.Now.Date)
            {
                return BadRequest("Xin lỗi hôm nay là ngày " + DateTime.Now.ToString("dd/MM/yyyy") +
                                  ", ngày bạn chọn " + selectedDate.ToString("dd/MM/yyyy") + " đã qua.");
            }

            var employeeShifts = _context.Shifts
                .Where(s => s.MaNv == maNv && s.DayOfWeek == selectedDate.DayOfWeek.ToString())
                .ToList();
            var employees = _context.Shifts
               .Where(s => s.MaNv == maNv )
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


        private List<string> GenerateTimeSlotsForShift(DateTime selectedDate, TimeSpan? start, TimeSpan? end, int slotDurationMinutes = 60)
        {
            var slots = new List<string>();

            if (start == null || end == null)
                throw new ArgumentException("Start and end times must not be null.");

            if (start >= end)
                throw new ArgumentException("Start time must be earlier than end time.");

            var startTime = selectedDate.Date.Add(start.Value);
            var endTime = selectedDate.Date.Add(end.Value);

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

