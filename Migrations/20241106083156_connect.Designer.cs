﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebQuanLyNhaKhoa.Data;

#nullable disable

namespace WebQuanLyNhaKhoa.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241106083156_connect")]
    partial class connect
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.BenhNhan", b =>
                {
                    b.Property<int>("IdbenhNhan")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdBenhNhan");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdbenhNhan"));

                    b.Property<string>("DiaChi")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool?>("Gioi")
                        .HasColumnType("bit");

                    b.Property<string>("HoTen")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NamSinh")
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<DateTime?>("NgayKhamDau")
                        .HasColumnType("datetime2");

                    b.Property<string>("Sdt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("IdbenhNhan");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("BenhNhans");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.ChanDoan", b =>
                {
                    b.Property<int>("IdchanDoan")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdChanDoan");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdchanDoan"));

                    b.Property<string>("TenChanDoan")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("IdchanDoan");

                    b.ToTable("ChanDoans");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.ChucVu", b =>
                {
                    b.Property<int>("MaCv")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("MaCV");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaCv"));

                    b.Property<string>("TenCv")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("MaCv");

                    b.ToTable("ChucVus");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.DanhSachKham", b =>
                {
                    b.Property<int>("Idkham")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdKham");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Idkham"));

                    b.Property<int>("IdbenhNhan")
                        .HasColumnType("int");

                    b.Property<int?>("MaNv")
                        .HasColumnType("int");

                    b.Property<DateTime>("NgayKham")
                        .HasColumnType("datetime2");

                    b.HasKey("Idkham");

                    b.HasIndex("IdbenhNhan");

                    b.HasIndex("MaNv");

                    b.ToTable("DanhSachKhams");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.DichVu", b =>
                {
                    b.Property<int>("IddichVu")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("IdDichVu");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IddichVu"));

                    b.Property<decimal>("DonGia")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("DonViTinh")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("IdchanDoan")
                        .HasColumnType("int");

                    b.Property<string>("TenDichVu")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("IddichVu");

                    b.HasIndex("IdchanDoan");

                    b.ToTable("DichVus");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.DieuTri", b =>
                {
                    b.Property<int>("IddieuTri")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IddieuTri"));

                    b.Property<int>("IddichVu")
                        .HasColumnType("int");

                    b.Property<int>("IddungCu")
                        .HasColumnType("int");

                    b.Property<int>("Idkham")
                        .HasColumnType("int");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.Property<decimal>("ThanhTien")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("IddieuTri");

                    b.HasIndex("IddichVu");

                    b.HasIndex("IddungCu");

                    b.HasIndex("Idkham");

                    b.ToTable("DieuTris");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.DonThuoc", b =>
                {
                    b.Property<int>("IddonThuoc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IddonThuoc"));

                    b.Property<int>("IddungCu")
                        .HasColumnType("int");

                    b.Property<int>("Idkham")
                        .HasColumnType("int");

                    b.Property<DateTime?>("NgayLapDt")
                        .HasColumnType("datetime2");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.Property<decimal>("ThanhGia")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TongTien")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("IddonThuoc");

                    b.HasIndex("IddungCu");

                    b.HasIndex("Idkham");

                    b.ToTable("DonThuocs");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.HoaDon", b =>
                {
                    b.Property<int>("IdhoaDon")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdhoaDon"));

                    b.Property<string>("EmailBn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IddieuTri")
                        .HasColumnType("int");

                    b.Property<int>("IddonThuoc")
                        .HasColumnType("int");

                    b.Property<int?>("Idkham")
                        .HasColumnType("int");

                    b.Property<DateTime>("NgayLap")
                        .HasColumnType("datetime2");

                    b.Property<string>("PhuongThucThanhToan")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TienDieuTri")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TienThuoc")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TongTien")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("IdhoaDon");

                    b.HasIndex("IddieuTri");

                    b.HasIndex("IddonThuoc");

                    b.HasIndex("Idkham");

                    b.ToTable("HoaDons");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.Kho", b =>
                {
                    b.Property<int>("IdsanPham")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdsanPham"));

                    b.Property<string>("DonViTinh")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("IddungCu")
                        .HasColumnType("int");

                    b.Property<string>("Loai")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("SoLuong")
                        .HasColumnType("int");

                    b.HasKey("IdsanPham");

                    b.HasIndex("IddungCu");

                    b.ToTable("Khos");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.LichSuNhapXuat", b =>
                {
                    b.Property<int>("MaLs")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaLs"));

                    b.Property<decimal?>("Don")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("DonViTinh")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("IdsanPham")
                        .HasColumnType("int");

                    b.Property<string>("Loai")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("NgayNhap")
                        .HasColumnType("datetime2");

                    b.Property<string>("NoiDung")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("SoLuongNhapXuat")
                        .HasColumnType("int");

                    b.Property<decimal>("ThanhTien")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("MaLs");

                    b.HasIndex("IdsanPham");

                    b.ToTable("LichSuNhapXuats");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.NhanVien", b =>
                {
                    b.Property<int>("MaNv")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaNv"));

                    b.Property<string>("Diachi")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Email")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool?>("Gioi")
                        .HasColumnType("bit");

                    b.Property<string>("Hinh")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("KinhNghiem")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("MaCv")
                        .HasColumnType("int");

                    b.Property<string>("Sdt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ten")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("MaNv");

                    b.HasIndex("MaCv");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("NhanViens");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.TaiKhoan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("MaBN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MaNV")
                        .HasColumnType("int");

                    b.Property<string>("MatKhau")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenDangNhap")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("TaiKhoans");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.ThiTruong", b =>
                {
                    b.Property<int>("IdsanPham")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdsanPham"));

                    b.Property<decimal>("DonGia")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("DonViTinh")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Loai")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TenSanPham")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("IdsanPham");

                    b.ToTable("ThiTruongs");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.BenhNhan", b =>
                {
                    b.HasOne("WebQuanLyNhaKhoa.Data.TaiKhoan", "TaiKhoan")
                        .WithOne("BenhNhan")
                        .HasForeignKey("WebQuanLyNhaKhoa.Data.BenhNhan", "UserId");

                    b.Navigation("TaiKhoan");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.DanhSachKham", b =>
                {
                    b.HasOne("WebQuanLyNhaKhoa.Data.BenhNhan", "BenhNhan")
                        .WithMany("DanhSachKhams")
                        .HasForeignKey("IdbenhNhan")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebQuanLyNhaKhoa.Data.NhanVien", "NhanVien")
                        .WithMany("DanhSachKhams")
                        .HasForeignKey("MaNv");

                    b.Navigation("BenhNhan");

                    b.Navigation("NhanVien");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.DichVu", b =>
                {
                    b.HasOne("WebQuanLyNhaKhoa.Data.ChanDoan", "ChanDoan")
                        .WithMany("DichVus")
                        .HasForeignKey("IdchanDoan")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChanDoan");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.DieuTri", b =>
                {
                    b.HasOne("WebQuanLyNhaKhoa.Data.DichVu", "DichVu")
                        .WithMany("DieuTris")
                        .HasForeignKey("IddichVu")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebQuanLyNhaKhoa.Data.Kho", "Kho")
                        .WithMany("DieuTris")
                        .HasForeignKey("IddungCu")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebQuanLyNhaKhoa.Data.DanhSachKham", "DanhSachKham")
                        .WithMany("DieuTris")
                        .HasForeignKey("Idkham")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DanhSachKham");

                    b.Navigation("DichVu");

                    b.Navigation("Kho");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.DonThuoc", b =>
                {
                    b.HasOne("WebQuanLyNhaKhoa.Data.Kho", "Kho")
                        .WithMany("DonThuocs")
                        .HasForeignKey("IddungCu")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebQuanLyNhaKhoa.Data.DanhSachKham", "DanhSachKham")
                        .WithMany("DonThuocs")
                        .HasForeignKey("Idkham")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DanhSachKham");

                    b.Navigation("Kho");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.HoaDon", b =>
                {
                    b.HasOne("WebQuanLyNhaKhoa.Data.DieuTri", "DieuTri")
                        .WithMany("HoaDons")
                        .HasForeignKey("IddieuTri")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebQuanLyNhaKhoa.Data.DonThuoc", "DonThuoc")
                        .WithMany("HoaDons")
                        .HasForeignKey("IddonThuoc")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WebQuanLyNhaKhoa.Data.DanhSachKham", "DanhSachKham")
                        .WithMany("HoaDons")
                        .HasForeignKey("Idkham")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("DanhSachKham");

                    b.Navigation("DieuTri");

                    b.Navigation("DonThuoc");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.Kho", b =>
                {
                    b.HasOne("WebQuanLyNhaKhoa.Data.ThiTruong", "ThiTruong")
                        .WithMany("Khos")
                        .HasForeignKey("IddungCu")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ThiTruong");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.LichSuNhapXuat", b =>
                {
                    b.HasOne("WebQuanLyNhaKhoa.Data.ThiTruong", "ThiTruong")
                        .WithMany()
                        .HasForeignKey("IdsanPham")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ThiTruong");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.NhanVien", b =>
                {
                    b.HasOne("WebQuanLyNhaKhoa.Data.ChucVu", "ChucVu")
                        .WithMany("NhanViens")
                        .HasForeignKey("MaCv")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebQuanLyNhaKhoa.Data.TaiKhoan", "TaiKhoan")
                        .WithOne("NhanVien")
                        .HasForeignKey("WebQuanLyNhaKhoa.Data.NhanVien", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChucVu");

                    b.Navigation("TaiKhoan");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.BenhNhan", b =>
                {
                    b.Navigation("DanhSachKhams");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.ChanDoan", b =>
                {
                    b.Navigation("DichVus");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.ChucVu", b =>
                {
                    b.Navigation("NhanViens");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.DanhSachKham", b =>
                {
                    b.Navigation("DieuTris");

                    b.Navigation("DonThuocs");

                    b.Navigation("HoaDons");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.DichVu", b =>
                {
                    b.Navigation("DieuTris");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.DieuTri", b =>
                {
                    b.Navigation("HoaDons");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.DonThuoc", b =>
                {
                    b.Navigation("HoaDons");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.Kho", b =>
                {
                    b.Navigation("DieuTris");

                    b.Navigation("DonThuocs");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.NhanVien", b =>
                {
                    b.Navigation("DanhSachKhams");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.TaiKhoan", b =>
                {
                    b.Navigation("BenhNhan");

                    b.Navigation("NhanVien");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.ThiTruong", b =>
                {
                    b.Navigation("Khos");
                });
#pragma warning restore 612, 618
        }
    }
}
