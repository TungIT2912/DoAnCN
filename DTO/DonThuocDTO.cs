using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebQuanLyNhaKhoa.DTO;

namespace WebQuanLyNhaKhoa.DTO
{
	public class DonThuocDTO
	{
		public int Idkham { get; set; }
		public int IddungCu { get; set; }

		public int SoLuong { get; set; }
		public decimal ThanhGia { get; set; }
		public decimal TongTien { get; set; }

		public DateTime? NgayLapDt { get; set; }
	}
}