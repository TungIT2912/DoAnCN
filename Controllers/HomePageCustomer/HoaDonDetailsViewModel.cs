public class HoaDonDetailsViewModel
{
    public int? IdHoaDon { get; set; }
    public string? HoTenBenhNhan { get; set; }
    public string? EmailBenhNhan { get; set; }
    public string? SoDienThoai { get; set; }
    public string? PhuongThucThanhToan { get; set; }
    public DateTime? NgayLap { get; set; }
    public decimal TongTien { get; set; }

    public List<DonThuocDTO>? DonThuocs { get; set; }
    public List<DichVuDTO>? DichVus { get; set; }

    public class DonThuocDTO
    {
        public int IdhoaDon { get; set; }
        public int Idkham { get; set; }

        public string? tenThuoc { get; set; }

        public decimal ThanhGia { get; set; }
        public decimal TongTien { get; set; }
        public int IddungCu { get; set; }
        public int SoLuong { get; set; }

        public DateTime? NgayLapDt { get; set; }
    }
    public class DonThuoc1DTO
    {
        public string? TenThuoc { get; set; }
        public int SoLuong { get; set; }
        public decimal TongTien { get; set; }
    }
    public class DichVuDTO
    {
        public string? TenDichVu { get; set; }
        public decimal ThanhTien { get; set; }
    }
}
