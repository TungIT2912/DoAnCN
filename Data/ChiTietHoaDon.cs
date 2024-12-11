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
        public int? IddonThuoc { get; set; }

        [ForeignKey("HoaDon")]
        public int? IdhoaDon { get; set; }
        
        [ForeignKey("DieuTri")]
        public int? IddieuTri { get; set; }

        [ForeignKey("DanhSachKham")]
        public int? Idkham { get; set; }


        public string? PhuongThucThanhToan { get; set; }

        public bool? DaThanhToan { get; set; }
    
        public string? TenDon { get; set; }
        public string? TenDieuTri { get; set; }
        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? TienThuoc { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? TienDieuTri { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? TongTien { get; set; }

        public DateTime? NgayLap { get; set; }

        [EmailAddress]
        public string? EmailBn { get; set; }
        public string? Sdt { get; set; }

        public virtual ICollection<DonThuoc> DonThuocs { get; set; } = new List<DonThuoc>();
        public virtual ICollection<DieuTri> DieuTris { get; set; } = new List<DieuTri>();
        public virtual DanhSachKham? DanhSachKham { get; set; }
        public virtual HoaDon? HoaDon { get; set; }

        
    }




    
}
