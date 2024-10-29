using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebQuanLyNhaKhoa.Data;

namespace WebQuanLyNhaKhoa.Data
{
	public partial class NhanVien
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaNv { get; set; }

		//[ForeignKey("TaiKhoan")]
		//[Required]
		//[StringLength(50)]
		//public string TenDangNhap { get; set; } = null!;

		[Required]
		[StringLength(100)]
		public string Ten { get; set; } = null!;

		[Phone]
		public string? Sdt { get; set; }

		[ForeignKey("ChucVu")]
		[Required]
		public string MaCv { get; set; } = null!;

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        [StringLength(200)]
		public string? KinhNghiem { get; set; }
        [StringLength(200)]
        public string? Diachi { get; set; }
        [StringLength(200)]
        public string? Email { get; set; }

        [StringLength(100)]
		public string? Hinh { get; set; }

		public virtual ICollection<DanhSachKham> DanhSachKhams { get; set; } = new List<DanhSachKham>();

		public virtual ChucVu MaCvNavigation { get; set; } = null!;
    }
}