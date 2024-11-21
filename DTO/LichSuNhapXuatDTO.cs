using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebQuanLyNhaKhoa.DTO
{
	public partial class LichSuNhapXuatDTO
	{
		public string NoiDung { get; set; } = null!;

		public int IdsanPham { get; set; } 

		public string? TenDungCu { get; set; } 

		public string? Loai { get; set; } = null!;

		public string? DonViTinh { get; set; } = null!;

		public int SoLuongNhapXuat { get; set; }

		[Range(0, double.MaxValue)]
		public decimal? Don { get; set; }

		[Range(0, double.MaxValue)]
		public decimal ThanhTien { get; set; }

		public DateTime? NgayNhap { get; set; }
	}
}