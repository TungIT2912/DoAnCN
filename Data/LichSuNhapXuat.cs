using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebQuanLyNhaKhoa.Data
{
	public partial class LichSuNhapXuat
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaLs { get; set; }

		[Required]
		[StringLength(500)]
		public string NoiDung { get; set; } = null!;

		[ForeignKey("ThiTruong")]
		[Required]
		public int IdsanPham { get; set; } 

		[Required]
		[StringLength(50)]
		public string Loai { get; set; } = null!;

		[Required]
		[StringLength(50)]
		public string DonViTinh { get; set; } = null!;

		public int SoLuongNhapXuat { get; set; }

		[Range(0, double.MaxValue)]
		public decimal? Don { get; set; }

		[Range(0, double.MaxValue)]
		public decimal ThanhTien { get; set; }

		public DateTime? NgayNhap { get; set; }

		public virtual ThiTruong ThiTruong { get; set; } = null!;
	}
}