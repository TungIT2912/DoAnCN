using MimeKit.Cryptography;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebQuanLyNhaKhoa.DTO;

namespace WebQuanLyNhaKhoa.DTO
{
	public partial class NhanVienDTO
	{
        public int? MaNv { get; set; }
        public string Ten { get; set; }
		public string? Sdt { get; set; }
		public int MaCv { get; set; }
        public string? TenCv { get; set; }
        public string? KinhNghiem { get; set; }
        public int IddichVu { get; set; }
        public string? TenChuyenNghanh { get; set; }
        public string? Diachi { get; set; }
        public bool? Gioi { get; set; }
        public string? Hinh { get; set; } = "/images/anonymous.png";
        public string Email { get; set; }
        public string MatKhau { get; set; }
        public string Role { get; set; }
        public string? Mota { get; set; }
        public List<ShiftDTO> sa { get; set; } = new List<ShiftDTO>();

    }
}