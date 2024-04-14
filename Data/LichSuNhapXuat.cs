using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebQuanLyNhaKhoa.Data;

public partial class LichSuNhapXuat
{
    public int MaLs { get; set; }

    [DisplayName("Nội dung")]
    public string NoiDung { get; set; } = null!;

    [DisplayName("ID dụng cụ")]
    public string IddungCu { get; set; } = null!;

    [DisplayName("Tên dụng cụ")]
    public string TenDungCu { get; set; } = null!;

    [DisplayName("Loại")]
    public string Loai { get; set; } = null!;

    [DisplayName("Đơn vị tính")]
    public string DonViTinh { get; set; } = null!;

    [DisplayName("Số lượng")]
    public int? SoLuongNhapXuat { get; set; }

    [DisplayName("Đơn giá")]
    public decimal Don { get; set; }

    [DisplayName("Thành tiền")]
    public decimal ThanhTien { get; set; }

    [DisplayName("Ngày tạo")]
    public DateTime NgayNhap { get; set; }

    public virtual Kho IddungCuNavigation { get; set; } = null!;
}
