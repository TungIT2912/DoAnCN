using WebQuanLyNhaKhoa.Data;

namespace WebQuanLyNhaKhoa.DTO
{
    public class RegisFormDTO
    {
        public int RegisFormId { get; set; }
        public int MaNv { get; set; }  
        public string? Ten { get; set; }
        public string? ChucVu { get; set; }

        public string? chuyenNganh { get; set; }

        public DateTime CreateDay { get; set; }   
       
        public string ReasonForChange { get; set; }
        public ShiftChangeStatus Status { get; set; }
        public List<NewShiftDTO> NewShifts { get; set; }
    }
}
