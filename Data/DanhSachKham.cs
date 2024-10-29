using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebQuanLyNhaKhoa.Data;

namespace WebQuanLyNhaKhoa.Data
{
    public class DanhSachKham
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdKham")]
        public string Idkham { get; set; } = null!;


        [ForeignKey("BenhNhan")]
        public string IdbenhNhan { get; set; } = null!;

        public int? MaNv { get; set; }


        public DateTime NgayKham { get; set; }

        // Quan hệ 1-n với DieuTri, DonThuoc, HoaDon
        public virtual ICollection<DieuTri> DieuTris { get; set; } = new List<DieuTri>();
        public virtual ICollection<DonThuoc> DonThuocs { get; set; } = new List<DonThuoc>();
        public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

        public virtual BenhNhan IdbenhNhanNavigation { get; set; } = null!;
        public virtual NhanVien? MaNvNavigation { get; set; }
    }
}