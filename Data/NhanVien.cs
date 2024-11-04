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

		[Required]
		[StringLength(100)]
		public string Ten { get; set; } = null!;

		[Phone]
		public string? Sdt { get; set; }

		[ForeignKey("ChucVu")]
		[Required]
		public int MaCv { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; } = null!;
        [StringLength(200)]
		public string? KinhNghiem { get; set; }
        [StringLength(200)]
        public string? Diachi { get; set; }
        [StringLength(200)]
        public string? Email { get; set; }

        [StringLength(100)]
		public string? Hinh { get; set; }

		public virtual ICollection<DanhSachKham> DanhSachKhams { get; set; } = new List<DanhSachKham>();
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;
        public virtual ChucVu ChucVu { get; set; } = null!;
    }
}