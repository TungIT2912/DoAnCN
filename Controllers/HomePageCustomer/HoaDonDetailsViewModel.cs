public class HoaDonDetailsViewModel
{
    public int? IdHoaDon { get; set; }
    public string? HoTenBenhNhan { get; set; }
    public string? EmailBenhNhan { get; set; }
    public string? SoDienThoai { get; set; }
    public string? PhuongThucThanhToan { get; set; }
    public DateTime? NgayLap { get; set; }
    public decimal TongTien { get; set; }

    public int? IdDonThuoc { get; set; }
    public string? TenDon { get; set; }
    public string? TenDieuTri { get; set; }
    public string? MoTa { get; set; }

    public List<DonThuocDTO>? DonThuocs { get; set; } = new();
    public List<DichVuDTO>? DichVus { get; set; } = new();

    public class DonThuocDTO
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
