using System;
using System.Collections.Generic;

namespace WebQuanLyNhaKhoa.Data;

public partial class HoaDon
{
    public int IdhoaDon { get; set; }

    public int IddonThuoc { get; set; }

    public int IddieuTri { get; set; }

    public string? Idkham { get; set; }

    public string? PhuongThucThanhToan { get; set; }

    public decimal TienThuoc { get; set; }

    public decimal TienDieuTri { get; set; }

    public decimal TongTien { get; set; }

    public DateTime NgayLap { get; set; }

    public string? EmailBn { get; set; }

    public virtual DieuTri IddieuTriNavigation { get; set; } = null!;

    public virtual DonThuoc IddonThuocNavigation { get; set; } = null!;

    public virtual DanhSachKham? IdkhamNavigation { get; set; }
}
