using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.DTO;
using X.PagedList;
using X.PagedList.Extensions;


namespace WebQuanLyNhaKhoa.Controllers.HomePageCustomer
{
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Services
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 6;

            // Retrieve paginated DichVu data
            var dichVus = await _context.DichVus
                .OrderBy(dv => dv.IddichVu)  
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Retrieve total count for pagination
            var totalCount = await _context.DichVus.CountAsync();

            // Create a PagedList for the DichVu data
            var pagedList = new StaticPagedList<DichVu>(dichVus, page, pageSize, totalCount);

            // Pass the paged list to the view
            return View(pagedList);
        }

        // Pricing page with pagination
        public ActionResult Pricing(int? page)
        {
            int pageSize = 4;  
            int pageNumber = (page ?? 1);  // Default to page 1 if no page parameter

            // Use AsQueryable() to ensure that ToPagedList can be applied
            var pricingData = _context.DichVus.AsQueryable();  // Convert to IQueryable

            return View(pricingData.ToPagedList(pageNumber, pageSize)); // Use ToPagedList from X.PagedList
        }

        // HoaDonDetails search page
public IActionResult HoaDonDetails(string searchQuery)
{
    // Kiểm tra nếu không có searchQuery, trả về tất cả hóa đơn
    var chiTietHoaDons = string.IsNullOrEmpty(searchQuery)
        ? _context.ChiTietHoaDons
            .Include(c => c.DanhSachKham)  
                .ThenInclude(dsk => dsk.BenhNhan)  // Bao gồm bảng Bệnh Nhân
            .Include(c => c.DieuTris)  
                .ThenInclude(dt => dt.DichVu)  // Bao gồm dịch vụ đã sử dụng
            .Include(c => c.DieuTris)  
                .ThenInclude(dt => dt.Kho)  // Bao gồm thuốc/thiết bị đã sử dụng
            .ToList()
        : _context.ChiTietHoaDons
            .Where(c => c.DanhSachKham.BenhNhan.Sdt.Contains(searchQuery)  // Tìm theo số điện thoại
                     || c.DanhSachKham.BenhNhan.EmailBn.Contains(searchQuery)) // Hoặc email
            .Include(c => c.DanhSachKham)  
                .ThenInclude(dsk => dsk.BenhNhan)  
            .Include(c => c.DieuTris)
                .ThenInclude(dt => dt.DichVu)
            .Include(c => c.DieuTris)
                .ThenInclude(dt => dt.Kho)
            .ToList();

    // Gắn dữ liệu tìm kiếm vào ViewData để hiển thị lại trên giao diện
    ViewData["SearchQuery"] = searchQuery; 

    return View(chiTietHoaDons);
}



        public IActionResult ServicesDetail(int id)
        {
            var service = _context.DichVus.FirstOrDefault(s => s.IddichVu == id);

            if (service == null)
            {
                return NotFound(); 
            }

            var doctors = _context.NhanViens.Include(nv => nv.DichVu)
                .Where(d => d.IddichVu == id)  
                .ToList();

            var viewModel = new ServiceDetailViewModel
            {
                Service = service,
                Doctors = doctors
            };

            return View(viewModel);
        }
        public IActionResult ListEachUser()
        {
         
            return View();
        }
        [HttpPost("api/getList")]
        public async Task<ActionResult<BenhNhan>> PostList([FromBody] RequestDTO request)
        {
            var benhnhan = await _context.DanhSachKhams
                                  .Include(bn => bn.BenhNhan)
                                  .Include(bn => bn.NhanVien)
                                  .Include(bn => bn.DieuTris)
                                  .ThenInclude(dt => dt.DichVu)
                                  .Include(bn => bn.BenhNhan.HoaDon)
                                  .ThenInclude(dv => dv.DieuTri.DichVu)
                                  .Include(bn => bn.DonThuocs)
                                  .ThenInclude(dt => dt.Kho.ThiTruong)
                                  .FirstOrDefaultAsync(n => n.BenhNhan.Sdt == request.Phone  ||  n.BenhNhan.EmailBn == request.Mail);
            if (benhnhan == null)
            {
                return NotFound("Không tìm thấy bệnh nhân.");
            }
            if (benhnhan?.DieuTris != null)
            {
                foreach (var dieuTri in benhnhan.DieuTris)
                {
                    Console.WriteLine($"TenDichVu: {dieuTri.DichVu?.TenDichVu ?? "No Data"}");
                }
            }
            var newDTO = new ListOfEachUserDTO
            {
                IdbenhNhan = benhnhan.IdbenhNhan,
                HoTen = benhnhan.BenhNhan?.HoTen ?? "Unknown",
                Gioi = benhnhan.BenhNhan?.Gioi,
                NamSinh = benhnhan.BenhNhan?.NamSinh ?? "Unknown",
                Sdt = benhnhan.BenhNhan?.Sdt ?? "Unknown",
                EmailBn = benhnhan.BenhNhan?.EmailBn ?? "Unknown",
                NgayKhamDau = benhnhan.NgayKham.ToString("dd/MM/yyyy"),
                TenBacSi = benhnhan.NhanVien?.Ten ?? "Unknown",
                DiaChi = benhnhan.BenhNhan?.DiaChi ?? "Unknown",
                time = benhnhan.time.ToString("HH:mm:ss"),
                DonThuocs = benhnhan.DonThuocs.Select(dt => new DonThuoc1DTO
                {
                    Idkham = dt.Idkham,
                    IddungCu = dt.IddungCu,
                    tenThuoc = dt.Kho.ThiTruong.TenSanPham ?? "Unknown",
                    SoLuong = dt.SoLuong,
                    ThanhGia = dt.ThanhGia,
                    TongTien = dt.TongTien,
                    NgayLapDt = dt.NgayLapDt
                }).ToList(),
                DieuTris = benhnhan.DieuTris.Select(dt => new DieuTriDTO
                {
                    IddichVu = dt.IddichVu,
                    tenDieuTri = dt.DichVu.TenDichVu ?? "Không có dữ liệu dịch vụ",
                    Idkham = dt.Idkham,
                    IddungCu = dt.IddungCu,
                    SoLuong = dt.SoLuong,
                    ThanhTien = (decimal)dt.ThanhTien
                }).ToList(),
                TrieuChung = benhnhan.BenhNhan.TrieuChung ?? "Unknown",
            };

            return Ok(newDTO);

        }

        [HttpGet("History")]
        public async Task<IActionResult> History(string phone, string mail)
        {
            if (string.IsNullOrEmpty(phone) && string.IsNullOrEmpty(mail))
            {
                return BadRequest("Phone hoặc email không hợp lệ.");
            }

            var benhnhan = await _context.DanhSachKhams
                                  .Include(bn => bn.BenhNhan)
                                  .Include(bn => bn.NhanVien)
                                  .Include(bn => bn.DieuTris)
                                  .Include(bn => bn.DonThuocs)
                                  .ThenInclude(dt => dt.Kho.ThiTruong)
                                  .FirstOrDefaultAsync(n => n.BenhNhan.Sdt == phone || n.BenhNhan.EmailBn == mail);

            if (benhnhan == null)
            {
                return NotFound("Không tìm thấy lịch sử khám.");
            }

            var viewModel = new ListOfEachUserDTO
            {
                IdbenhNhan = benhnhan.IdbenhNhan,
                HoTen = benhnhan.BenhNhan.HoTen,
                Gioi = benhnhan.BenhNhan.Gioi,
                NamSinh = benhnhan.BenhNhan.NamSinh,
                Sdt = benhnhan.BenhNhan.Sdt,
                EmailBn = benhnhan.BenhNhan.EmailBn,
                NgayKhamDau = benhnhan.NgayKham.ToString("dd/MM/yyyy"),
                TenBacSi = benhnhan.NhanVien.Ten,
                DiaChi = benhnhan.BenhNhan.DiaChi,
                time = benhnhan.time.ToString("HH:mm:ss"),
                DonThuocs = benhnhan.DonThuocs.Select(dt => new DonThuoc1DTO
                {
                    tenThuoc = dt.Kho.ThiTruong.TenSanPham,
                    SoLuong = dt.SoLuong,
                    ThanhGia = dt.ThanhGia,
                    TongTien = dt.TongTien,
                    NgayLapDt = dt.NgayLapDt
                }).ToList(),
                DieuTris = benhnhan.DieuTris.Select(dt => new DieuTriDTO
                {
                    tenDieuTri = dt.DichVu?.TenDichVu ?? "Không có dữ liệu dịch vụ",
                    SoLuong = dt.SoLuong,
                    ThanhTien = (decimal)dt.ThanhTien
                }).ToList(),
                TrieuChung = benhnhan.BenhNhan.TrieuChung,
            };

            return View(viewModel);
        }

        public class RequestDTO
        {
            public string? Phone { get; set; }
            public string? Mail { get; set; }
        }

    }
}
