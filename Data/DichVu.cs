﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebQuanLyNhaKhoa.Data;

namespace WebQuanLyNhaKhoa.Data
{
	public class DichVu
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IdDichVu")]
		public int IddichVu { get; set; } 

		[ForeignKey("ChanDoan")]
		public int IdchanDoan { get; set; } 

		[Required]
		[StringLength(100)]
		public string TenDichVu { get; set; } = null!;

		[Required]
		[StringLength(50)]
		public string DonViTinh { get; set; } = null!;

		[Range(0, double.MaxValue)]
		public decimal DonGia { get; set; }
        [StringLength(100)]
        public string? Hinh { get; set; }
     
        public virtual ICollection<DieuTri> DieuTris { get; set; } = new List<DieuTri>();

		[StringLength(500)]
        public string? Description { get; set; } 


		public virtual ChanDoan ChanDoan { get; set; } = null!;


		  [NotMapped]
    public NhanVien? AssignedDoctor { get; set; }
	}
}