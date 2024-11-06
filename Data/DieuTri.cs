using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebQuanLyNhaKhoa.Data;

namespace WebQuanLyNhaKhoa.Data
{
	public class DieuTri
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IddieuTri { get; set; }

		[ForeignKey("DichVu")]
		public int IddichVu { get; set; }

		[ForeignKey("DanhSachKham")]
		public int Idkham { get; set; }

		[ForeignKey("Kho")]
		public int IddungCu { get; set; } 

		public int SoLuong { get; set; }

		[Range(0, double.MaxValue)]
		public decimal ThanhTien { get; set; }

		public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

		public virtual DichVu DichVu { get; set; } = null!;
		public virtual Kho Kho { get; set; } = null!;
		public virtual DanhSachKham DanhSachKham { get; set; } = null!;
	}
}