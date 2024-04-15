using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebQuanLyNhaKhoa.Data;

public partial class DanhSachKham
{
    [DisplayName("ID khám")]
    public string Idkham { get; set; } = null!;

    [DisplayName("Tên bệnh nhân")]
    public string IdbenhNhan { get; set; } = null!;

    [DisplayName("Tên nhân viên")]
    public int? MaNv { get; set; }

    [DisplayName("Ngày khám")]
    public DateTime NgayKham { get; set; }

    public virtual ICollection<DieuTri> DieuTris { get; set; } = new List<DieuTri>();

    public virtual ICollection<DonThuoc> DonThuocs { get; set; } = new List<DonThuoc>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    [DisplayName("Tên bệnh nhân")]
    public virtual BenhNhan IdbenhNhanNavigation { get; set; } = null!;

    [DisplayName("Tên nhân viên")]
    public virtual NhanVien? MaNvNavigation { get; set; }
}
