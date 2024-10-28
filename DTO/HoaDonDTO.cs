using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebQuanLyNhaKhoa.DTO
{
	public class HoaDonDTO
	{

		public int IddonThuoc { get; set; }

		public int IddieuTri { get; set; }

		public string? Idkham { get; set; }

		public string? PhuongThucThanhToan { get; set; }
		public decimal TienThuoc { get; set; }
		public decimal TienDieuTri { get; set; }
		public decimal TongTien { get; set; }

		public DateTime NgayLap { get; set; }
		public string? EmailBn { get; set; }
	}
}