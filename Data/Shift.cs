using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebQuanLyNhaKhoa.Data
{
    public class Shift
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

        public virtual NhanVien NhanVien { get; set; } = null!;

    }
}
