using System.ComponentModel.DataAnnotations;

namespace WebQuanLyNhaKhoa.DTO
{
    public class ListOfEachUserDTO
    {
        public int? IdbenhNhan { get; set; }
        public string HoTen { get; set; }

        public bool? Gioi { get; set; }
        public string? NamSinh { get; set; }

        [Phone]
        public string? Sdt { get; set; }

        public string? TenBacSi { get; set; }


        public int? MaNv { get; set; }

        [StringLength(200)]
        public string? DiaChi { get; set; }
        public string? TrieuChung { get; set; }
        [EmailAddress]
        public string? EmailBn { get; set; }
     
        public string? time { get; set; }
        public string? NgayKhamDau { get; set; }
        public List<DonThuoc1DTO> DonThuocs { get; set; } = new List<DonThuoc1DTO>();
        public List<DieuTriDTO> DieuTris { get; set; } = new List<DieuTriDTO>();
    }
}
