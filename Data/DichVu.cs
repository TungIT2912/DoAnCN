using System;
using System.Collections.Generic;

namespace WebQuanLyNhaKhoa.Data;

public partial class DichVu
{
    public string IddichVu { get; set; } = null!;

    public string IdchanDoan { get; set; } = null!;

    public string TenDichVu { get; set; } = null!;

    public string DonViTinh { get; set; } = null!;

    public decimal DonGia { get; set; }

    public virtual ICollection<DieuTri> DieuTris { get; set; } = new List<DieuTri>();

    public virtual ChanDoan IdchanDoanNavigation { get; set; } = null!;
}
