using System.ComponentModel.DataAnnotations;

namespace WebQuanLyNhaKhoa.DTO
{
    public class ShiftDTO
    {
        [Required]
        public int MaNv { get; set; } 

        [Required]
        [StringLength(20)]
        public string? DayOfWeek { get; set; } = null!;

        [Required]
        public TimeSpan? StartTime { get; set; }

        [Required]
        public TimeSpan? EndTime { get; set; }
        public int RegisFormId { get; set; }
        public List<ShiftInfo> Shifts { get; set; } = new List<ShiftInfo>();
    }

    public class ShiftInfo
    {
        public string DayOfWeek { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public bool IsSelected { get; set; }
        public bool IsAvailable { get; set; }
    }
    
        public class ShiftUpdateDTO
        {
            public string DayOfWeek { get; set; }
            public TimeSpan? StartTime { get; set; }
            public TimeSpan? EndTime { get; set; }
        }
    


}
