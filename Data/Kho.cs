using System;
using System.Collections.Generic;

namespace WebQuanLyNhaKhoa.Data;

public partial class Kho
{
    public string IdsanPham { get; set; } = null!;

    public string IddungCu { get; set; } = null!;

    public string TenDungCu { get; set; } = null!;

    public string Loai { get; set; } = null!;

    public string DonViTinh { get; set; } = null!;

    public int? SoLuong { get; set; }

    public virtual ICollection<DieuTri> DieuTris { get; set; } = new List<DieuTri>();

    public virtual ICollection<DonThuoc> DonThuocs { get; set; } = new List<DonThuoc>();

    public virtual ThiTruong IdsanPhamNavigation { get; set; } = null!;

    public virtual ICollection<LichSuNhapXuat> LichSuNhapXuats { get; set; } = new List<LichSuNhapXuat>();
}
