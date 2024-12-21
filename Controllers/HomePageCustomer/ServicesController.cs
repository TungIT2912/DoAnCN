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

         
            var dichVus = await _context.DichVus
                .OrderBy(dv => dv.IddichVu)  
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

          
            var totalCount = await _context.DichVus.CountAsync();

           
            var pagedList = new StaticPagedList<DichVu>(dichVus, page, pageSize, totalCount);

          
            return View(pagedList);
        }

        // Pricing page with pagination
        public ActionResult Pricing(int? page)
        {
            int pageSize = 4;  
            int pageNumber = (page ?? 1);  

         
            var pricingData = _context.DichVus.AsQueryable();  

            return View(pricingData.ToPagedList(pageNumber, pageSize)); 
        }

   

public IActionResult HoaDonDetails(string searchQuery)
{
    // Truy vấn từ ChiTietHoaDon, bao gồm các bảng liên quan
    var query = _context.ChiTietHoaDons
        .Include(cthd => cthd.DanhSachKham)
            .ThenInclude(dsk => dsk.BenhNhan)
        .Include(cthd => cthd.DonThuocs)
            .ThenInclude(dt => dt.Kho)
        .Include(cthd => cthd.DieuTris)
            .ThenInclude(dt => dt.DichVu);

    // Áp dụng điều kiện tìm kiếm (nếu có)
    if (!string.IsNullOrEmpty(searchQuery))
    {
        query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<ChiTietHoaDon, DichVu>)query.Where(cthd =>
            cthd.DanhSachKham.BenhNhan.Sdt.Contains(searchQuery) ||
            cthd.DanhSachKham.BenhNhan.EmailBn.Contains(searchQuery));
    }

    var chiTietHoaDons = query.ToList();

    // Map dữ liệu sang ViewModel
    var viewModel = chiTietHoaDons.Select(cthd => new HoaDonDetailsViewModel
    {
        IdHoaDon = cthd.IdhoaDon,
        HoTenBenhNhan = cthd.DanhSachKham?.BenhNhan?.HoTen,
        EmailBenhNhan = cthd.DanhSachKham?.BenhNhan?.EmailBn,
        SoDienThoai = cthd.DanhSachKham?.BenhNhan?.Sdt,
        PhuongThucThanhToan = cthd.PhuongThucThanhToan,
        NgayLap = cthd.NgayLap,
        TongTien = cthd.TongTien ?? 0,
        DonThuocs = cthd.DonThuocs.Select(dt => new HoaDonDetailsViewModel.DonThuocDTO
        {
            tenThuoc = dt.Kho?.ThiTruong?.TenSanPham,
            SoLuong = dt.SoLuong,
            TongTien = dt.ThanhGia
        }).ToList(),
        DichVus = cthd.DieuTris.Select(dt => new HoaDonDetailsViewModel.DichVuDTO
        {
            TenDichVu = dt.DichVu?.TenDichVu,
            ThanhTien = dt.ThanhTien
        }).ToList()
    }).ToList();

    // Gán search query vào ViewData để hiển thị lại trên giao diện
    ViewData["SearchQuery"] = searchQuery;

    return View(viewModel);
}




        public IActionResult ServicesDetail(int id)
        {
            var service = _context.DichVus.FirstOrDefault(s => s.IddichVu == id);

            if (service == null)
            {
                return NotFound(); 
            }

            var doctors = _context.NhanViens
                .Where(d => d.ChucVu.TenCv == "Bác sĩ")  
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
