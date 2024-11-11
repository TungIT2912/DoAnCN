using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebQuanLyNhaKhoa.DTO
{
	public class HoaDonDTO
	{
		public int? Idkham { get; set; }

		public string tenBn { get; set; }

		public string? PhuongThucThanhToan { get; set; }
		public decimal? TienThuoc { get; set; }
		public decimal? TienDieuTri { get; set; }
		public decimal? TongTien { get; set; }

		public DateTime? NgayLap { get; set; }
		public string? EmailBn { get; set; }
	}
}