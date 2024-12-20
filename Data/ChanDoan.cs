﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebQuanLyNhaKhoa.Data;

namespace WebQuanLyNhaKhoa.Data
{
	public class ChanDoan
	{
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
		[Column("IdChanDoan")]
		public int IdchanDoan { get; set; }

		[Required]
		[StringLength(100)]
		public string TenChanDoan { get; set; } = null!;

		// Quan hệ 1-n với DichVu
		public virtual ICollection<DichVu> DichVus { get; set; } = new List<DichVu>();
	}
}