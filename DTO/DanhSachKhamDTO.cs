using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebQuanLyNhaKhoa.DTO;

namespace WebQuanLyNhaKhoa.DTO
{
	public class DanhSachKhamDTO
	{
		public int IdbenhNhan { get; set; } 

		public int? MaNv { get; set; }


		public DateTime NgayKham { get; set; }
	}
}