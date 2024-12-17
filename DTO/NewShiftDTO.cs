using Microsoft.AspNetCore.Mvc;
using WebQuanLyNhaKhoa.Models;

namespace WebQuanLyNhaKhoa.DTO
{
    public class NewShiftDTO
    {
        public int RegisFormId { get; set; }  
        public string DayOfWeek { get; set; } = null!;
        [ModelBinder(BinderType = typeof(TimeSpanBinder))] 
        public TimeSpan StartTime { get; set; }
        [ModelBinder(BinderType = typeof(TimeSpanBinder))] 
        public TimeSpan EndTime { get; set; }
    }
}
