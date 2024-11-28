using MimeKit.Encodings;

namespace WebQuanLyNhaKhoa.DTO
{
    public class ChiTietHoaDonDTO
    {
        public int? IdchiTiet { get; set; }

        public string? tenBn { get; set; }
        public int? Idkham { get; set; }
        public int? IdhoaDon { get; set; }

        public string? PhuongThucThanhToan { get; set; }

        public string? TenDon { get; set; }
        public string? TenDieuTri { get; set; }
        public string? Description { get; set; }

        public decimal? TienThuoc { get; set; }
        public decimal? TienDieuTri { get; set; }
        public decimal? TongTien { get; set; }

        public DateTime NgayLap { get; set; }
        public string? EmailBn { get; set; }
            public string? Sdt { get; set; }
            
        public List<DonThuoc1DTO> DonThuocs { get; set; } = new List<DonThuoc1DTO>();
        public List<DieuTriDTO> DieuTris { get; set; } = new List<DieuTriDTO>();

       
    }
}
