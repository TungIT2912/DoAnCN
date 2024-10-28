//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace WebQuanLyNhaKhoa.Data
//{
//	public partial class TaiKhoan
//	{
//		[Key]
//		[StringLength(50)]
//		public string TenDangNhap { get; set; } = null!;

//		[Required]
//		[StringLength(100)]
//		public string MatKhau { get; set; } = null!;

//		public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();
//	}
//}