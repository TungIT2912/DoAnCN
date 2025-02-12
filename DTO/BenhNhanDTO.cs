﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebQuanLyNhaKhoa.DTO;
namespace WebQuanLyNhaKhoa.DTO

{
    public class BenhNhanDTO
    {
        public int? IdbenhNhan { get; set; }
        public string HoTen { get; set; }

        public bool? Gioi { get; set; }
        public string? NamSinh { get; set; }

        [Phone]
        public string? Sdt { get; set; }

        public int? IddichVu { get; set; }

        public int? MaNv { get; set; }

        [StringLength(200)]
        public string? DiaChi { get; set; }
        public string? TrieuChung { get; set; }
        [EmailAddress]
        public string? EmailBn { get; set; }
        public int? IdChanDoan { get; set; }
        public string time { get; set; }
        public string NgayKhamDau { get; set; }
        public string? TenDangNhap { get; set; }
        public string? MatKhau { get; set; }
        public string? QrCodeUrl { get; set; }
        public string? ImageUrl { get; set; }
        public string? QrCodeImage { get; set; }
        public string? Pdf { get; set; }
    }
}