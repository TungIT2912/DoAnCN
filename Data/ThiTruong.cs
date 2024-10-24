using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebQuanLyNhaKhoa.Data
{
	public partial class ThiTruong
	{
		[Key]
		[StringLength(50)]
		public string IdsanPham { get; set; } = null!;

		[Required]
		[StringLength(100)]
		public string TenSanPham { get; set; } = null!;

		[Required]
		[StringLength(50)]
		public string Loai { get; set; } = null!;

		[Required]
		[StringLength(50)]
		public string DonViTinh { get; set; } = null!;

		[Range(0, double.MaxValue)]
		public decimal DonGia { get; set; }

		public virtual ICollection<Kho> Khos { get; set; } = new List<Kho>();
	}
}