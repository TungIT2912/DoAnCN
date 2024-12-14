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

            public DateTime CreateDay { get; set; }

            [Required]
            [StringLength(200)]
            public string ReasonForChange { get; set; } = null!; 

            [Required]
            public ShiftChangeStatus Status { get; set; } = ShiftChangeStatus.Waiting; 
            public virtual NhanVien NhanVien { get; set; } = null!;
            public virtual ICollection<NewShift> NewShifts { get; set; } = new List<NewShift>(); 

            public virtual ICollection<ResponseForm> Responses { get; set; } = new List<ResponseForm>();
        }

        public enum ShiftChangeStatus
        {
            Waiting = 1,  
            Accepted = 2, 
            Denied = 3   
        }
}

