using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebQuanLyNhaKhoa.Data;

namespace WebQuanLyNhaKhoa.Data
{
	public class Kho
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdsanPham { get; set; } 

		[ForeignKey("ThiTruong")]
		public int IddungCu { get; set; } 

		[Required]
		[StringLength(100)]
		public string TenDungCu { get; set; } = null!;

		[Required]
		[StringLength(50)]
		public string Loai { get; set; } = null!;

		[Required]
		[StringLength(50)]
		public string DonViTinh { get; set; } = null!;

		public int? SoLuong { get; set; }

		public virtual ICollection<DieuTri> DieuTris { get; set; } = new List<DieuTri>();
		public virtual ICollection<DonThuoc> DonThuocs { get; set; } = new List<DonThuoc>();

		public virtual ThiTruong IdsanPhamNavigation { get; set; } = null!;
	}
}