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
    }
}
