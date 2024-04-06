using System;
using System.Collections.Generic;

namespace WebQuanLyNhaKhoa.Data;

public partial class CtdonThuoc
{
    public string Idctdt { get; set; } = null!;

    public int IddonThuoc { get; set; }

    public string TenThuoc { get; set; } = null!;

    public int SoLuong { get; set; }

    public decimal ThanhGia { get; set; }

    public virtual DonThuoc IddonThuocNavigation { get; set; } = null!;
}
