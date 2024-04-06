using System;
using System.Collections.Generic;

namespace WebQuanLyNhaKhoa.Data;

public partial class ChucVu
{
    public string MaCv { get; set; } = null!;

    public string TenCv { get; set; } = null!;

    public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();
}
