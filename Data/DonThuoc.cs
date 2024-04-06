using System;
using System.Collections.Generic;

namespace WebQuanLyNhaKhoa.Data;

public partial class DonThuoc
{
    public int IddonThuoc { get; set; }

    public string Idkham { get; set; } = null!;

    public decimal TongTien { get; set; }

    public DateTime? NgayLapDt { get; set; }

    public virtual ICollection<CtdonThuoc> CtdonThuocs { get; set; } = new List<CtdonThuoc>();

    public virtual DanhSachKham IdkhamNavigation { get; set; } = null!;
}
