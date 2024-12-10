using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebQuanLyNhaKhoa.Data
{
    public class ResponseForm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("RegisForm")]
        public int IdFormRegis { get; set; }  

        [Required]
        public ShiftChangeStatus ResponseStatus { get; set; } 

        [StringLength(500)]
        public string? Comment { get; set; }  

        public virtual RegisForm RegisForm { get; set; }
    }
}
