using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebQuanLyNhaKhoa.DTO;

namespace WebQuanLyNhaKhoa.DTO
{
	public class DonThuocDTO
	{
        public int IddonThuoc { get; set; }
        public int Idkham { get; set; }
		
        public string? tenThuoc { get; set; }
        public int hoaDonId { get; set; }
        public decimal ThanhGia { get; set; }
		public decimal TongTien { get; set; }
        public List<int> IddungCu { get; set; } // Mảng chứa các ID dụng cụ
        public List<int> SoLuong { get; set; }

        public DateTime? NgayLapDt { get; set; }
	}
}
//public int IddungCu { get; set; }
//      public int SoLuong { get; set; }