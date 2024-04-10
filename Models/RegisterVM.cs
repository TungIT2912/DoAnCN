using System.ComponentModel.DataAnnotations;

namespace WebQuanLyNhaKhoa.Models
{
    public class RegisterVM
    {
		[Required(ErrorMessage ="Trường này không được bỏ trống!")]
		[MaxLength(20,ErrorMessage = "Không quá 20 ký tự!")]
		public string TenDangNhap { get; set; }

		[Required(ErrorMessage = "Trường này không được bỏ trống!")]
		[MaxLength(20, ErrorMessage = "Không quá 20 ký tự!")]
		[DataType(DataType.Password)]
		public string MatKhau { get; set; }
	}
}
