using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebQuanLyNhaKhoa.DTO
{
	public partial class ThiTruongDTO
	{
		public string TenSanPham { get; set; } = null!;

		public string Loai { get; set; } = null!;

		public string DonViTinh { get; set; } = null!;

		[Range(0, double.MaxValue)]
		public decimal DonGia { get; set; }

		public virtual ICollection<KhoDTO> Khos { get; set; } = new List<KhoDTO>();
	}
}