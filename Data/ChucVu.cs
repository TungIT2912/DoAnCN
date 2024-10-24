using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebQuanLyNhaKhoa.Data;

namespace WebQuanLyNhaKhoa.Data
{
	public class ChucVu
	{
		[Key]
		[Column("MaCV")]
		public string MaCv { get; set; } = null!;

		[Required]
		[StringLength(50)]
		public string TenCv { get; set; } = null!;

		// Quan hệ 1-n với NhanVien
		public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();
	}
}
