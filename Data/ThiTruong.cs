using System;
using System.Collections.Generic;

namespace WebQuanLyNhaKhoa.Data;

public partial class ThiTruong
{
    public string IdsanPham { get; set; } = null!;

    public string TenSanPham { get; set; } = null!;

    public string Loai { get; set; } = null!;

    public string DonViTinh { get; set; } = null!;

    public decimal DonGia { get; set; }

    public virtual ICollection<Kho> Khos { get; set; } = new List<Kho>();
}
