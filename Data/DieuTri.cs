using System;
using System.Collections.Generic;

namespace WebQuanLyNhaKhoa.Data;

public partial class DieuTri
{
    public int IddieuTri { get; set; }

    public string IddichVu { get; set; } = null!;

    public string Idkham { get; set; } = null!;

    public string IddungCu { get; set; } = null!;

    public int SoLuong { get; set; }

    public decimal ThanhTien { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual DichVu IddichVuNavigation { get; set; } = null!;

    public virtual Kho IddungCuNavigation { get; set; } = null!;

    public virtual DanhSachKham IdkhamNavigation { get; set; } = null!;
}
