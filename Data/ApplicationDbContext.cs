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

		public virtual DbSet<Kho> Khos { get; set; }

		public virtual DbSet<LichSuNhapXuat> LichSuNhapXuats { get; set; }

		public virtual DbSet<NhanVien> NhanViens { get; set; }

		public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

		public virtual DbSet<ThiTruong> ThiTruongs { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<HoaDon>()
				.HasOne(h => h.IddieuTriNavigation)
				.WithMany(d => d.HoaDons)
				.HasForeignKey(h => h.IddieuTri)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<HoaDon>()
				.HasOne(h => h.IddonThuocNavigation)
				.WithMany(d => d.HoaDons)
				.HasForeignKey(h => h.IddonThuoc)
				.OnDelete(DeleteBehavior.Restrict); // Change cascade to restrict or set null

			modelBuilder.Entity<HoaDon>()
				.HasOne(h => h.IdkhamNavigation)
				.WithMany(d => d.HoaDons)
				.HasForeignKey(h => h.Idkham)
				.OnDelete(DeleteBehavior.Restrict); // Change cascade to restrict or set null
		}
	}
}
