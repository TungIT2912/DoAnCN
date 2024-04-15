using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebQuanLyNhaKhoa.Data;

public partial class QlnhaKhoaContext : DbContext
{
    public QlnhaKhoaContext()
    {
    }

    public QlnhaKhoaContext(DbContextOptions<QlnhaKhoaContext> options)
        : base(options)
    {
    }

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-INB6SBCV\\SQLEXPRESS;Initial Catalog=QLNhaKhoa;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BenhNhan>(entity =>
        {
            entity.HasKey(e => e.IdbenhNhan).HasName("PK__BenhNhan__33CC78E40A9D3BCF");

            entity.ToTable("BenhNhan");

            entity.Property(e => e.IdbenhNhan)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IDBenhNhan");
            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.HoTen).HasMaxLength(255);
            entity.Property(e => e.NamSinh)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.NgayKhamDau).HasColumnType("datetime");
            entity.Property(e => e.Sdt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SDT");
        });

        modelBuilder.Entity<ChanDoan>(entity =>
        {
            entity.HasKey(e => e.IdchanDoan).HasName("PK__ChanDoan__38CFF322B33E80A5");

            entity.ToTable("ChanDoan");

            entity.Property(e => e.IdchanDoan)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IDChanDoan");
            entity.Property(e => e.TenChanDoan).HasMaxLength(255);
        });

        modelBuilder.Entity<ChucVu>(entity =>
        {
            entity.HasKey(e => e.MaCv).HasName("PK__ChucVu__27258E76DBFE2135");

            entity.ToTable("ChucVu");

            entity.Property(e => e.MaCv)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MaCV");
            entity.Property(e => e.TenCv)
                .HasMaxLength(255)
                .HasColumnName("TenCV");
        });

        modelBuilder.Entity<DanhSachKham>(entity =>
        {
            entity.HasKey(e => e.Idkham).HasName("PK__DanhSach__1AB7B4EBBB73E07A");

            entity.ToTable("DanhSachKham");

            entity.Property(e => e.Idkham)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IDKham");
            entity.Property(e => e.IdbenhNhan)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IDBenhNhan");
            entity.Property(e => e.MaNv).HasColumnName("MaNV");
            entity.Property(e => e.NgayKham).HasColumnType("datetime");

            entity.HasOne(d => d.IdbenhNhanNavigation).WithMany(p => p.DanhSachKhams)
                .HasForeignKey(d => d.IdbenhNhan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chk_IDBenhNhan_DanhSachKham");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.DanhSachKhams)
                .HasForeignKey(d => d.MaNv)
                .HasConstraintName("chk_MaNV_DanhSachKham");
        });

        modelBuilder.Entity<DichVu>(entity =>
        {
            entity.HasKey(e => e.IddichVu).HasName("PK__DichVu__C0C95928C2C1BF83");

            entity.ToTable("DichVu");

            entity.Property(e => e.IddichVu)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IDDichVu");
            entity.Property(e => e.DonGia).HasColumnType("money");
            entity.Property(e => e.DonViTinh).HasMaxLength(255);
            entity.Property(e => e.IdchanDoan)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IDChanDoan");
            entity.Property(e => e.TenDichVu).HasMaxLength(255);

            entity.HasOne(d => d.IdchanDoanNavigation).WithMany(p => p.DichVus)
                .HasForeignKey(d => d.IdchanDoan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chk_IDChanDoan_DichVu");
        });

        modelBuilder.Entity<DieuTri>(entity =>
        {
            entity.HasKey(e => e.IddieuTri).HasName("PK__DieuTri__85B3D346F07FFEB3");

            entity.ToTable("DieuTri");

            entity.Property(e => e.IddieuTri).HasColumnName("IDDieuTri");
            entity.Property(e => e.IddichVu)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IDDichVu");
            entity.Property(e => e.IddungCu)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IDDungCu");
            entity.Property(e => e.Idkham)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IDKham");
            entity.Property(e => e.ThanhTien).HasColumnType("money");

            entity.HasOne(d => d.IddichVuNavigation).WithMany(p => p.DieuTris)
                .HasForeignKey(d => d.IddichVu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chk_IDDichVu_DieuTri");

            entity.HasOne(d => d.IddungCuNavigation).WithMany(p => p.DieuTris)
                .HasForeignKey(d => d.IddungCu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chk_IDDungCu_DieuTri");

            entity.HasOne(d => d.IdkhamNavigation).WithMany(p => p.DieuTris)
                .HasForeignKey(d => d.Idkham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chk_IDKham_DieuTri");
        });

        modelBuilder.Entity<DonThuoc>(entity =>
        {
            entity.HasKey(e => e.IddonThuoc).HasName("PK__DonThuoc__51DB14E889898F98");

            entity.ToTable("DonThuoc");

            entity.Property(e => e.IddonThuoc).HasColumnName("IDDonThuoc");
            entity.Property(e => e.IddungCu)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IDDungCu");
            entity.Property(e => e.Idkham)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IDKham");
            entity.Property(e => e.NgayLapDt)
                .HasColumnType("datetime")
                .HasColumnName("NgayLapDT");
            entity.Property(e => e.ThanhGia).HasColumnType("money");
            entity.Property(e => e.TongTien).HasColumnType("money");

            entity.HasOne(d => d.IddungCuNavigation).WithMany(p => p.DonThuocs)
                .HasForeignKey(d => d.IddungCu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chk_IDDungCu_DonThuoc");

            entity.HasOne(d => d.IdkhamNavigation).WithMany(p => p.DonThuocs)
                .HasForeignKey(d => d.Idkham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chk_IDKham_DonThuoc");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.IdhoaDon).HasName("PK__HoaDon__5B896F497F16612B");

            entity.ToTable("HoaDon");

            entity.Property(e => e.IdhoaDon).HasColumnName("IDHoaDon");
            entity.Property(e => e.EmailBn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EmailBN");
            entity.Property(e => e.IddieuTri).HasColumnName("IDDieuTri");
            entity.Property(e => e.IddonThuoc).HasColumnName("IDDonThuoc");
            entity.Property(e => e.Idkham)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IDKham");
            entity.Property(e => e.NgayLap).HasColumnType("datetime");
            entity.Property(e => e.PhuongThucThanhToan).HasMaxLength(255);
            entity.Property(e => e.TienDieuTri).HasColumnType("money");
            entity.Property(e => e.TienThuoc).HasColumnType("money");
            entity.Property(e => e.TongTien).HasColumnType("money");

            entity.HasOne(d => d.IddieuTriNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.IddieuTri)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chk_IDDieuTri_HoaDon");

            entity.HasOne(d => d.IddonThuocNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.IddonThuoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chk_IDDonThuoc_HoaDon");

            entity.HasOne(d => d.IdkhamNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.Idkham)
                .HasConstraintName("chk_IDKham_HoaDon");
        });

        modelBuilder.Entity<Kho>(entity =>
        {
            entity.HasKey(e => e.IddungCu).HasName("PK__Kho__50E8F16097B5FC5E");

            entity.ToTable("Kho");

            entity.Property(e => e.IddungCu)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IDDungCu");
            entity.Property(e => e.DonViTinh).HasMaxLength(255);
            entity.Property(e => e.IdsanPham)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IDSanPham");
            entity.Property(e => e.Loai).HasMaxLength(255);
            entity.Property(e => e.TenDungCu).HasMaxLength(255);

            entity.HasOne(d => d.IdsanPhamNavigation).WithMany(p => p.Khos)
                .HasForeignKey(d => d.IdsanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chk_IDSanPham_Kho");
        });

        modelBuilder.Entity<LichSuNhapXuat>(entity =>
        {
            entity.HasKey(e => e.MaLs).HasName("PK__LichSuNh__2725C77279912860");

            entity.ToTable("LichSuNhapXuat");

            entity.Property(e => e.MaLs).HasColumnName("MaLS");
            entity.Property(e => e.Don).HasColumnType("money");
            entity.Property(e => e.DonViTinh).HasMaxLength(255);
            entity.Property(e => e.IddungCu)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IDDungCu");
            entity.Property(e => e.Loai).HasMaxLength(255);
            entity.Property(e => e.NgayNhap).HasColumnType("datetime");
            entity.Property(e => e.NoiDung).HasMaxLength(6);
            entity.Property(e => e.TenDungCu).HasMaxLength(255);
            entity.Property(e => e.ThanhTien).HasColumnType("money");

            entity.HasOne(d => d.IddungCuNavigation).WithMany(p => p.LichSuNhapXuats)
                .HasForeignKey(d => d.IddungCu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chk_IDDungCu_NK");
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNv).HasName("PK__NhanVien__2725D70A25545DAD");

            entity.ToTable("NhanVien");

            entity.Property(e => e.MaNv).HasColumnName("MaNV");
            entity.Property(e => e.KinhNghiem).HasMaxLength(50);
            entity.Property(e => e.MaCv)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("MaCV");
            entity.Property(e => e.Sdt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SDT");
            entity.Property(e => e.Ten).HasMaxLength(50);
            entity.Property(e => e.TenDangNhap)
                .HasMaxLength(22)
                .IsUnicode(false);

            entity.HasOne(d => d.MaCvNavigation).WithMany(p => p.NhanViens)
                .HasForeignKey(d => d.MaCv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chk_MaCV");

            entity.HasOne(d => d.TenDangNhapNavigation).WithMany(p => p.NhanViens)
                .HasForeignKey(d => d.TenDangNhap)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chk_TenDangNhap");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.TenDangNhap).HasName("PK__TaiKhoan__55F68FC127E7A77D");

            entity.ToTable("TaiKhoan");

            entity.Property(e => e.TenDangNhap)
                .HasMaxLength(22)
                .IsUnicode(false);
            entity.Property(e => e.MatKhau)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ThiTruong>(entity =>
        {
            entity.HasKey(e => e.IdsanPham).HasName("PK__ThiTruon__9D45E58A3AF783C3");

            entity.ToTable("ThiTruong");

            entity.Property(e => e.IdsanPham)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IDSanPham");
            entity.Property(e => e.DonGia).HasColumnType("money");
            entity.Property(e => e.DonViTinh).HasMaxLength(255);
            entity.Property(e => e.Loai).HasMaxLength(50);
            entity.Property(e => e.TenSanPham).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
