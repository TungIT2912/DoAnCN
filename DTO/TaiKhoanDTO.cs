using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebQuanLyNhaKhoa.DTO
{
    public partial class TaiKhoanDTO
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        [EmailAddress]

        public string TenDangNhap { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string MatKhau { get; set; }
        public string Role { get; set; }
        public string ChucVu { get; set; }
        public bool? isLoocked {  get; set; }
    }
}