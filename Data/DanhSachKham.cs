using System;
using System.Collections.Generic;

namespace WebQuanLyNhaKhoa.Data;

public partial class DanhSachKham
{
    public string Idkham { get; set; } = null!;

    public string IdbenhNhan { get; set; } = null!;

    public int? MaNv { get; set; }

    public DateTime NgayKham { get; set; }

    public virtual ICollection<DieuTri> DieuTris { get; set; } = new List<DieuTri>();

    public virtual ICollection<DonThuoc> DonThuocs { get; set; } = new List<DonThuoc>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual BenhNhan IdbenhNhanNavigation { get; set; } = null!;

    public virtual NhanVien? MaNvNavigation { get; set; }
}
