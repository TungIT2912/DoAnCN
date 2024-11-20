using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebQuanLyNhaKhoa.DTO;

namespace WebQuanLyNhaKhoa.DTO
{
	public class ChanDoanDTO
	{
		public int IdchanDoan { get; set; }

        public string TenChanDoan { get; set; }
        public int IdBenhNhan { get; set; }
        public string GhiChu { get; set; }
    }
}