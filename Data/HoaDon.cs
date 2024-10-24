using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebQuanLyNhaKhoa.Data
{
	public class HoaDon
	{
		[Key]
		public int IdhoaDon { get; set; }

		public int IddonThuoc { get; set; }

		public int IddieuTri { get; set; }

		public string? Idkham { get; set; }

		public string? PhuongThucThanhToan { get; set; }

		[Range(0, double.MaxValue)]
		public decimal TienThuoc { get; set; }

		[Range(0, double.MaxValue)]
		public decimal TienDieuTri { get; set; }

		[Range(0, double.MaxValue)]
		public decimal TongTien { get; set; }

		public DateTime NgayLap { get; set; }

		[EmailAddress]
		public string? EmailBn { get; set; }

		public virtual DieuTri IddieuTriNavigation { get; set; } = null!;
		public virtual DonThuoc IddonThuocNavigation { get; set; } = null!;
		public virtual DanhSachKham? IdkhamNavigation { get; set; }
	}
}