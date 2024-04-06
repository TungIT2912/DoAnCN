using System;
using System.Collections.Generic;

namespace WebQuanLyNhaKhoa.Data;

public partial class TaiKhoan
{
    public string TenDangNhap { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();
}
