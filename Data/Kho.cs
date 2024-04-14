using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebQuanLyNhaKhoa.Data;

public partial class Kho
{
    [DisplayName("ID sản phẩm")]
    public string IdsanPham { get; set; } = null!;

    [DisplayName("ID dụng cụ")]
    public string IddungCu { get; set; } = null!;

    [DisplayName("Tên dụng cụ")]
    public string TenDungCu { get; set; } = null!;

    [DisplayName("Loại")]
    public string Loai { get; set; } = null!;

    [DisplayName("Đơn vị tính")]
    public string DonViTinh { get; set; } = null!;

    [DisplayName("Số lượng")]
    public int? SoLuong { get; set; }

    public virtual ICollection<DieuTri> DieuTris { get; set; } = new List<DieuTri>();

    public virtual ICollection<DonThuoc> DonThuocs { get; set; } = new List<DonThuoc>();

    public virtual ThiTruong IdsanPhamNavigation { get; set; } = null!;

    public virtual ICollection<LichSuNhapXuat> LichSuNhapXuats { get; set; } = new List<LichSuNhapXuat>();
}
