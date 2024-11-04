using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebQuanLyNhaKhoa.Data;
namespace WebQuanLyNhaKhoa.Data
{
	public class BenhNhan
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdBenhNhan")]
		public int IdbenhNhan { get; set; }
        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }
        [Required]
		[StringLength(100)]
		public string HoTen { get; set; } = null!;

		public bool? Gioi { get; set; }

		[StringLength(4)]
		public string? NamSinh { get; set; }

		[Phone]
		public string? Sdt { get; set; }

		[StringLength(200)]
		public string? DiaChi { get; set; }

		public DateTime? NgayKhamDau { get; set; }

		// Quan hệ 1-n với DanhSachKham
		public virtual ICollection<DanhSachKham> DanhSachKhams { get; set; } = new List<DanhSachKham>();
        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
