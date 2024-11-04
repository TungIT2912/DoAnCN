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
		public string Ten { get; set; }
		public string? Sdt { get; set; }
		public int MaCv { get; set; }
		public string? KinhNghiem { get; set; }
        public string? Diachi { get; set; }
        public string? Hinh { get; set; } = "/images/anonymous.png";
        public string Email { get; set; }
        public string MatKhau { get; set; }
        public string Role { get; set; }
    }
}