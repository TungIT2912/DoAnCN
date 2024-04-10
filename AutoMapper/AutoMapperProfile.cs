using AutoMapper;
using WebQuanLyNhaKhoa.Data;
using WebQuanLyNhaKhoa.Models;

namespace WebQuanLyNhaKhoa.wwwroot.AutoMapper
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile() 
		{
			CreateMap<RegisterVM, TaiKhoan>().ForMember(tk => tk.TenDangNhap,
				option => option.MapFrom(RegisterVM => RegisterVM.TenDangNhap));

			CreateMap<RegisterVM, TaiKhoan>().ForMember(tk => tk.MatKhau,
				option => option.MapFrom(RegisterVM => RegisterVM.MatKhau));
		}
	}
}
