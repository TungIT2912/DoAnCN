using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebQuanLyNhaKhoa.Data;
namespace WebQuanLyNhaKhoa.Data
{
	public class BenhNhan
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdBenhNhan")]
		public int IdbenhNhan { get; set; }
        [ForeignKey("TaiKhoan")]
        public int? UserId { get; set; }
        [Required]
		[StringLength(100)]
		public string HoTen { get; set; }

		public bool? Gioi { get; set; }

		[StringLength(4)]
		public string? NamSinh { get; set; }

		[Phone]
		public string? Sdt { get; set; }

		[StringLength(200)]
		public string? DiaChi { get; set; }

		public string? TrieuChung { get; set; }
		public string? GhiChu { get; set; }

        
        [ForeignKey("ChanDoan")]
        
        public int? IdChanDoan { get; set; }

        public DateTime? NgayKhamDau { get; set; }
        [EmailAddress]
        public string EmailBn { get; set; }

        // Quan hệ 1-n với DanhSachKham
        [JsonIgnore]
        public virtual ICollection<DanhSachKham> DanhSachKhams { get; set; } = new List<DanhSachKham>();
        public virtual TaiKhoan? TaiKhoan { get; set; }
        public virtual ChanDoan? ChanDoan { get; set; }
        public virtual HoaDon? HoaDon { get; set; }
    }
}
