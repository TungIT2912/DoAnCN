using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebQuanLyNhaKhoa.Data
{
	public class ApplicationDbContext : DbContext
    {
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		// DbSets for your entities
		public virtual DbSet<BenhNhan> BenhNhans { get; set; }

		public virtual DbSet<ChanDoan> ChanDoans { get; set; }

		public virtual DbSet<ChucVu> ChucVus { get; set; }

		public virtual DbSet<DanhSachKham> DanhSachKhams { get; set; }

		public virtual DbSet<DichVu> DichVus { get; set; }

		public virtual DbSet<DieuTri> DieuTris { get; set; }

		public virtual DbSet<DonThuoc> DonThuocs { get; set; }

		public virtual DbSet<HoaDon> HoaDons { get; set; }

		public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }

		public virtual DbSet<Kho> Khos { get; set; }

		public virtual DbSet<LichSuNhapXuat> LichSuNhapXuats { get; set; }

		public virtual DbSet<NhanVien> NhanViens { get; set; }

        public DbSet<UnansweredQuestion> UnansweredQuestions { get; set; }

		//public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

		public virtual DbSet<ThiTruong> ThiTruongs { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

        public virtual DbSet<Shift> Shifts { get; set; }

        public virtual DbSet<RegisForm> RegisForms { get; set; }

        public virtual DbSet<ResponseForm> ResponseForms { get; set; }

        public virtual DbSet<NewShift> NewShifts { get; set; }

        //public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("Server=RON;Database=QLNhaKhoaa;Integrated Security=True;Trust Server Certificate=True")
                .EnableSensitiveDataLogging(); // Enable sensitive data logging for detailed exception information

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
           
            modelBuilder.Entity<HoaDon>()
				.HasOne(h => h.DieuTri)
				.WithMany(d => d.HoaDons)
				.HasForeignKey(h => h.IddieuTri)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<HoaDon>()
				.HasOne(h => h.DonThuoc)
				.WithMany(d => d.HoaDons)
				.HasForeignKey(h => h.IddonThuoc)
				.OnDelete(DeleteBehavior.Restrict); // Change cascade to restrict or set null

			modelBuilder.Entity<HoaDon>()
				.HasOne(h => h.DanhSachKham)
				.WithMany(d => d.HoaDons)
				.HasForeignKey(h => h.Idkham)
				.OnDelete(DeleteBehavior.Restrict); // Change cascade to restrict or set null
            modelBuilder.Entity<Kho>()
				.HasOne(k => k.ThiTruong)
				.WithMany()
				.HasForeignKey(k => k.IdsanPham);

            //modelBuilder.Entity<ChiTietHoaDon>()
            //	.HasOne(ct => ct.DonThuoc)
            //	.WithMany()
            //	.HasForeignKey(ct => ct.IddonThuoc)
            //	.OnDelete(DeleteBehavior.Restrict); // Sử dụng Restrict hoặc NoAction

            //modelBuilder.Entity<ChiTietHoaDon>()
            //	.HasOne(ct => ct.DieuTri)
            //	.WithMany()
            //	.HasForeignKey(ct => ct.IddieuTri)
            //	.OnDelete(DeleteBehavior.Restrict); // Sử dụng Restrict hoặc NoAction

        }
	}
}
