using WebQuanLyNhaKhoa.Data;

namespace WebQuanLyNhaKhoa.DTO
{
    public class ResponseFormDTO
    {
        public int RegisFormId { get; set; }  

        public int? MaNv {  get; set; }
        public ShiftChangeStatus? ResponseStatus { get; set; } 
        public string? Comment { get; set; }
        public List<NewShiftDTO> NewShifts { get; set; }
    }
}
