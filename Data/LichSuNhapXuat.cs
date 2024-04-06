using System;
using System.Collections.Generic;

namespace WebQuanLyNhaKhoa.Data;

public partial class LichSuNhapXuat
{
    public int MaLs { get; set; }

    public string NoiDung { get; set; } = null!;

    public string IddungCu { get; set; } = null!;

    public string TenDungCu { get; set; } = null!;

    public string Loai { get; set; } = null!;

    public string DonViTinh { get; set; } = null!;

    public int? SoLuongNhapXuat { get; set; }

    public decimal Don { get; set; }

    public decimal ThanhTien { get; set; }

    public DateTime NgayNhap { get; set; }

    public virtual Kho IddungCuNavigation { get; set; } = null!;
}
