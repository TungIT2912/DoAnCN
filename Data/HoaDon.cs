using System;
using System.Collections.Generic;

namespace WebQuanLyNhaKhoa.Data;

public partial class HoaDon
{
    public int IdhoaDon { get; set; }

    public string Idkham { get; set; } = null!;

    public string? PhuongThucThanhToan { get; set; }

    public decimal TienThuoc { get; set; }

    public decimal TienDieuTri { get; set; }

    public decimal TongTien { get; set; }

    public DateTime NgayLap { get; set; }

    public virtual DanhSachKham IdkhamNavigation { get; set; } = null!;
}
