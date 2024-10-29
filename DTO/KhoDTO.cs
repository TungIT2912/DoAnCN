using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebQuanLyNhaKhoa.DTO;

namespace WebQuanLyNhaKhoa.DTO
{
	public class KhoDTO
	{
		public string IddungCu { get; set; } = null!;

		public string TenDungCu { get; set; } = null!;

		public string Loai { get; set; } = null!;

		public string DonViTinh { get; set; } = null!;

		public int? SoLuong { get; set; }

	}
}