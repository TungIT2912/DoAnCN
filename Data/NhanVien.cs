using System;
using System.Collections.Generic;

namespace WebQuanLyNhaKhoa.Data;

public partial class NhanVien
{
    public int MaNv { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public string Ten { get; set; } = null!;

    public string? Sdt { get; set; }

    public string MaCv { get; set; } = null!;

    public string? KinhNghiem { get; set; }

    public string? Hinh { get; set; }

    public virtual ICollection<DanhSachKham> DanhSachKhams { get; set; } = new List<DanhSachKham>();

    public virtual ChucVu MaCvNavigation { get; set; } = null!;

    public virtual TaiKhoan TenDangNhapNavigation { get; set; } = null!;
}
