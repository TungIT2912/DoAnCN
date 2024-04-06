using System;
using System.Collections.Generic;

namespace WebQuanLyNhaKhoa.Data;

public partial class BenhNhan
{
    public string IdbenhNhan { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public bool? Gioi { get; set; }

    public string? NamSinh { get; set; }

    public string? Sdt { get; set; }

    public string? DiaChi { get; set; }

    public DateTime? NgayKhamDau { get; set; }

    public virtual ICollection<DanhSachKham> DanhSachKhams { get; set; } = new List<DanhSachKham>();
}
