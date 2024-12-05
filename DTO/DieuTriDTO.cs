using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebQuanLyNhaKhoa.DTO;

namespace WebQuanLyNhaKhoa.DTO
{
	public class DieuTriDTO
	{
        public int IddieuTri { get; set; }
        public int IddichVu { get; set; } 
		public string? tenDieuTri { get; set; }

        public int Idkham { get; set; }
		public int IddungCu { get; set; }
		public int IddonThuoc { get; set; }
        public int hoaDonId { get; set; }
        public int chiTietId { get; set; }

        public int SoLuong { get; set; }

		public decimal ThanhTien { get; set; }
        public string? TenDichVu { get; set; }
        public string TenBenhNhan { get; set; } = string.Empty;
        public string DonViTinh { get; set; } = string.Empty;

    }
}