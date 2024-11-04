using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebQuanLyNhaKhoa.DTO;

namespace WebQuanLyNhaKhoa.DTO
{
	public class DieuTriDTO
	{
		public int IddichVu { get; set; } 
		public int Idkham { get; set; }
		public int IddungCu { get; set; }

		public int SoLuong { get; set; }

		public decimal ThanhTien { get; set; }

	}
}