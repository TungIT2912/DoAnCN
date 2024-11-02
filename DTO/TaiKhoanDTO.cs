using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebQuanLyNhaKhoa.DTO
{
    public partial class TaiKhoanDTO
    {
        [EmailAddress]
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
    }
}