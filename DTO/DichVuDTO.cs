using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebQuanLyNhaKhoa.DTO;

namespace WebQuanLyNhaKhoa.DTO
{
	public class DichVuDTO
	{

        public int? IddichVu { get; set; }
        public int IdchanDoan { get; set; }
        public string? TenChuanDoan { get; set; }
        public string TenDichVu { get; set; }
        public string DonViTinh { get; set; } 
		public decimal DonGia { get; set; }
        public string? Hinh { get; set; } = "/images/anonymous.png";
    }
}