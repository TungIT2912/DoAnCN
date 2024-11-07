using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebQuanLyNhaKhoa.Data
{
    public class ChiTietHoaDon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdchiTiet { get; set; }

        [ForeignKey("DonThuoc")]
        public int IddonThuoc { get; set; }

        [ForeignKey("HoaDon")]
        public int IdhoaDon { get; set; }
        
        [ForeignKey("DieuTri")]
        public int IddieuTri { get; set; }

        [ForeignKey("DanhSachKham")]
        public int? Idkham { get; set; }

        public string? PhuongThucThanhToan { get; set; }

        public string TenDon { get; set; }
        public string TenDieuTri { get; set; }
        public string Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal TienThuoc { get; set; }

        [Range(0, double.MaxValue)]
        public decimal TienDieuTri { get; set; }

        [Range(0, double.MaxValue)]
        public decimal TongTien { get; set; }

        public DateTime NgayLap { get; set; }

        [EmailAddress]
        public string? EmailBn { get; set; }

        public virtual DieuTri DieuTri { get; set; } = null!;
        public virtual DonThuoc DonThuoc { get; set; } = null!;
        public virtual DanhSachKham? DanhSachKham { get; set; }

        // Thêm các trường mới để lưu danh sách các đơn thuốc và điều trị
        public ICollection<string> DanhSachDonThuoc { get; set; } = new List<string>();
        public ICollection<string> DanhSachDieuTri { get; set; } = new List<string>();
    }
}
