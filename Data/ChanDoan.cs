using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebQuanLyNhaKhoa.Data;

namespace WebQuanLyNhaKhoa.Data
{
	public class ChanDoan
	{
		[Key]
		[Column("IdChanDoan")]
		public string IdchanDoan { get; set; } = null!;

		[Required]
		[StringLength(100)]
		public string TenChanDoan { get; set; } = null!;

		// Quan hệ 1-n với DichVu
		public virtual ICollection<DichVu> DichVus { get; set; } = new List<DichVu>();
	}
}