using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebQuanLyNhaKhoa.Data;

public partial class HoaDon
{
    [DisplayName("ID hóa đơn")]
    public int IdhoaDon { get; set; }

    [DisplayName("ID đơn thuốc")]
    public int IddonThuoc { get; set; }

    [DisplayName("ID điều trị")]
    public int IddieuTri { get; set; }

    [DisplayName("ID khám")]
    public string? Idkham { get; set; }

    [DisplayName("Phương thức thanh toán")]
    public string? PhuongThucThanhToan { get; set; }

    [DisplayName("Tổng tiền thuốc")]
    public decimal TienThuoc { get; set; }

    [DisplayName("Tổng tiền điều trị")]
    public decimal TienDieuTri { get; set; }

    [DisplayName("Thành tiền")]
    public decimal TongTien { get; set; }

    [DisplayName("Ngày tạo")]
    public DateTime NgayLap { get; set; }

    [DisplayName("Email bệnh nhân")]
    public string? EmailBn { get; set; }

    public virtual DieuTri IddieuTriNavigation { get; set; } = null!;

    public virtual DonThuoc IddonThuocNavigation { get; set; } = null!;

    public virtual DanhSachKham? IdkhamNavigation { get; set; }
}
