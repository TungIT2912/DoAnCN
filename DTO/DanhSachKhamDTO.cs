using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebQuanLyNhaKhoa.DTO;

namespace WebQuanLyNhaKhoa.DTO
{
	public class DanhSachKhamDTO
	{
        public int Idkham { get; set; }
        public int IdbenhNhan { get; set; }
        public string hoTenBenhNhan { get; set; }
        public string sdtBenhnhan { get; set; }
        public string diaChiBenhnhan { get; set; }

        public string GioKham { get; set; }


        public int? MaNv { get; set; }


		public DateTime NgayKham { get; set; }
	}
}