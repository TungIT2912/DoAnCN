using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebQuanLyNhaKhoa.DTO;

namespace WebQuanLyNhaKhoa.DTO
{
	public class KhoDTO
	{
		public int IddungCu { get; set; } 

		public string? TenDungCu { get; set; } 

		public string Loai { get; set; } = null!;

		public string DonViTinh { get; set; } = null!;

		public int? SoLuong { get; set; }

	}
}