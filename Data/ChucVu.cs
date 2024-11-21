using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebQuanLyNhaKhoa.Data;

namespace WebQuanLyNhaKhoa.Data
{
	public class ChucVu
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("MaCV")]
		public int MaCv { get; set; } 

		[Required]
		[StringLength(50)]
		public string TenCv { get; set; } = null!;

		// Quan hệ 1-n với NhanVien
		public virtual List<NhanVien> NhanViens { get; set; } = new List<NhanVien>();
	}
}
