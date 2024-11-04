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

		public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

		public virtual Kho IddungCuNavigation { get; set; } = null!;
		public virtual DanhSachKham IdkhamNavigation { get; set; } = null!;
	}
}