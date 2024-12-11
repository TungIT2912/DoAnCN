namespace WebQuanLyNhaKhoa.DTO
{
    public class NewShiftDTO
    {
        public int RegisFormId { get; set; }  
        public string DayOfWeek { get; set; } = null!; 
        public TimeSpan StartTime { get; set; }  
        public TimeSpan EndTime { get; set; }
    }
}
