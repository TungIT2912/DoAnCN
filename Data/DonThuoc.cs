using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebQuanLyNhaKhoa.Data;

namespace WebQuanLyNhaKhoa.Data
{
	public class DonThuoc
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IddonThuoc { get; set; }

		[ForeignKey("DanhSachKham")]
		public int Idkham { get; set; } 

		[ForeignKey("Kho")]
		public int IddungCu { get; set; }
        public int SoLuong { get; set; }

		[Range(0, double.MaxValue)]
		public decimal ThanhGia { get; set; }

		[Range(0, double.MaxValue)]
		public decimal TongTien { get; set; }

		public DateTime? NgayLapDt { get; set; }
        [ForeignKey("ChiTietHoaDon")]
        public int? ChiTietHoaDonId { get; set; } 

        public virtual ChiTietHoaDon ChiTietHoaDon { get; set; }

        public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

		public virtual Kho Kho { get; set; } = null!;
		public virtual DanhSachKham DanhSachKham { get; set; } = null!;
	}
}