using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebQuanLyNhaKhoa.Data;

public partial class DonThuoc
{
    [DisplayName("ID đơn thuốc")]
    public int IddonThuoc { get; set; }

    [DisplayName("ID khám")]
    public string Idkham { get; set; } = null!;

    [DisplayName("ID thuốc")]
    public string IddungCu { get; set; } = null!;

    [DisplayName("Số lượng")]
    public int SoLuong { get; set; }

    [DisplayName("Đơn giá")]
    public decimal ThanhGia { get; set; }

    [DisplayName("Thành tiền")]
    public decimal TongTien { get; set; }

    [DisplayName("Ngày lập")]
    public DateTime? NgayLapDt { get; set; }


    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();


    [DisplayName("Tên thuốc")]
    public virtual Kho IddungCuNavigation { get; set; } = null!;


    [DisplayName("Tên bệnh nhân")]
    public virtual DanhSachKham IdkhamNavigation { get; set; } = null!;
}
