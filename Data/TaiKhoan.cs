using System;
using System.Collections.Generic;

namespace WebQuanLyNhaKhoa.Data;

public partial class TaiKhoan
{
    public string TenDangNhap { get; set; }
    public string MatKhau { get; set; }
    public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();
}
