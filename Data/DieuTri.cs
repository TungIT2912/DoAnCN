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
		public string IddichVu { get; set; } = null!;

		[ForeignKey("DanhSachKham")]
		public string Idkham { get; set; } = null!;

		[ForeignKey("Kho")]
		public string IddungCu { get; set; } = null!;

		public int SoLuong { get; set; }

		[Range(0, double.MaxValue)]
		public decimal ThanhTien { get; set; }

		public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

		public virtual DichVu IddichVuNavigation { get; set; } = null!;
		public virtual Kho IddungCuNavigation { get; set; } = null!;
		public virtual DanhSachKham IdkhamNavigation { get; set; } = null!;
	}
}