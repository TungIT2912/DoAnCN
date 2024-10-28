using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebQuanLyNhaKhoa.Data
{
	public partial class TaiKhoan
	{
		[Key]
		[StringLength(50)]
		public string TenDangNhap { get; set; } = null!;

		[Required]
		[StringLength(100)]
		public string MatKhau { get; set; } = null!;
        public string? MaBN { get; set; } // Change this to string to match IdbenhNhan
        public virtual BenhNhan? BenhNhan { get; set; } // Navigation property for BenhNhan

        public int? MaNV { get; set; }
        public virtual NhanVien? NhanVien { get; set; }
    }
}