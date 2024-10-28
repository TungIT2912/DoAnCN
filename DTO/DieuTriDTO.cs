using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebQuanLyNhaKhoa.DTO;

namespace WebQuanLyNhaKhoa.DTO
{
	public class DieuTriDTO
	{
		public string IddichVu { get; set; } = null!;
		public string Idkham { get; set; } = null!;
		public string IddungCu { get; set; } = null!;

		public int SoLuong { get; set; }

		public decimal ThanhTien { get; set; }

	}
}