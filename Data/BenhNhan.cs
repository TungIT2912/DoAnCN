using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebQuanLyNhaKhoa.Data;

public partial class BenhNhan
{
    public string IdbenhNhan { get; set; } = null!;

    [DisplayName("Tên bệnh nhân")]
    public string HoTen { get; set; } = null!;

    public bool? Gioi { get; set; }

    public string? NamSinh { get; set; }

    public string? Sdt { get; set; }

    public string? DiaChi { get; set; }

    public DateTime? NgayKhamDau { get; set; }

    public virtual ICollection<DanhSachKham> DanhSachKhams { get; set; } = new List<DanhSachKham>();
}
