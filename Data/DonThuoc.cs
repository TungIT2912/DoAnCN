using System;
using System.Collections.Generic;

namespace WebQuanLyNhaKhoa.Data;

public partial class DonThuoc
{
    public int IddonThuoc { get; set; }

    public string Idkham { get; set; } = null!;

    public string IddungCu { get; set; } = null!;

    public int SoLuong { get; set; }

    public decimal ThanhGia { get; set; }

    public decimal TongTien { get; set; }

    public DateTime? NgayLapDt { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual Kho IddungCuNavigation { get; set; } = null!;

    public virtual DanhSachKham IdkhamNavigation { get; set; } = null!;
}
