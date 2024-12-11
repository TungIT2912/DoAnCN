using WebQuanLyNhaKhoa.Data;

namespace WebQuanLyNhaKhoa.DTO
{
    public class RegisFormDTO
    {
        public int MaNv { get; set; }  
        public DateTime CreateDay { get; set; }   
       
        public string ReasonForChange { get; set; }
        public ShiftChangeStatus Status { get; set; }
    }
}
