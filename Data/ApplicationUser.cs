using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebQuanLyNhaKhoa.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        public string? Address { get; set; }
        //public string? Age { get; set; }
        //public bool Sex { get; set; }
        //[Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        //[RegularExpression(@"^(0|\+84)[3|5|7|8|9]\d{8}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        //public string PhoneNumber { get; set; }
    }
}
