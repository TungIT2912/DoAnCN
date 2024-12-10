using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebQuanLyNhaKhoa.Data
{
    public class RegisForm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("NhanVien")]
        public int MaNv { get; set; }

        [Required]
        [StringLength(20)]
        public string? DayOfWeek { get; set; } = null!;

        [Required]
        public TimeSpan? StartTime { get; set; }

        [Required]
        public TimeSpan? EndTime { get; set; }
        [Required]
        [StringLength(200)]
        public string ReasonForChange { get; set; } = null!; 

        [Required]
        public ShiftChangeStatus Status { get; set; } = ShiftChangeStatus.Waiting;

        public virtual NhanVien NhanVien { get; set; } = null!;

    }
    public enum ShiftChangeStatus
    {
        Waiting = 1,  
        Accepted = 2, 
        Denied = 3
    }


}

