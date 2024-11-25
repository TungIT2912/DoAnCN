using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebQuanLyNhaKhoa.Data
{
    public class UnansweredQuestion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Question { get; set; }

        public DateTime AskedOn { get; set; }
        public string? Answer { get; set; }

        public bool IsAnswered { get; set; } // Trạng thái đã trả lời hay chưa

        [ForeignKey("NhanVien")]
        public int? MaNv { get; set; } // Khóa ngoại đến bảng NhanVien
        public virtual NhanVien NhanVien { get; set; } = null;
    }


}
