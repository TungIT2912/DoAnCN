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
    [Migration("20241024073739_test")]
    partial class test
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.BenhNhan", b =>
                {
                    b.Property<string>("IdbenhNhan")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("IdBenhNhan");

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

                    b.HasKey("IdbenhNhan");

                    b.ToTable("BenhNhans");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.ChanDoan", b =>
                {
                    b.Property<string>("IdchanDoan")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("IdChanDoan");

                    b.Property<string>("TenChanDoan")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("IdchanDoan");

                    b.ToTable("ChanDoans");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.ChucVu", b =>
                {
                    b.Property<string>("MaCv")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("MaCV");

                    b.Property<string>("TenCv")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("MaCv");

                    b.ToTable("ChucVus");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.DanhSachKham", b =>
                {
                    b.Property<string>("Idkham")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("IdKham");

                    b.Property<string>("IdbenhNhan")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdbenhNhanNavigationIdbenhNhan")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("MaNv")
                        .HasColumnType("int");

                    b.Property<int?>("MaNvNavigationMaNv")
                        .HasColumnType("int");

                    b.Property<DateTime>("NgayKham")
                        .HasColumnType("datetime2");

                    b.HasKey("Idkham");

                    b.HasIndex("IdbenhNhanNavigationIdbenhNhan");

                    b.HasIndex("MaNvNavigationMaNv");

                    b.ToTable("DanhSachKhams");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.DichVu", b =>
                {
                    b.Property<string>("IddichVu")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("IdDichVu");

                    b.Property<decimal>("DonGia")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("DonViTinh")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("IdchanDoan")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdchanDoanNavigationIdchanDoan")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TenDichVu")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("IddichVu");

                    b.HasIndex("IdchanDoanNavigationIdchanDoan");

                    b.ToTable("DichVus");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.DieuTri", b =>
                {
                    b.Property<int>("IddieuTri")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IddieuTri"));

                    b.Property<string>("IddichVu")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IddichVuNavigationIddichVu")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IddungCu")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IddungCuNavigationIdsanPham")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Idkham")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdkhamNavigationIdkham")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.Property<decimal>("ThanhTien")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("IddieuTri");

                    b.HasIndex("IddichVuNavigationIddichVu");

                    b.HasIndex("IddungCuNavigationIdsanPham");

                    b.HasIndex("IdkhamNavigationIdkham");

                    b.ToTable("DieuTris");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.DonThuoc", b =>
                {
                    b.Property<int>("IddonThuoc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IddonThuoc"));

                    b.Property<string>("IddungCu")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IddungCuNavigationIdsanPham")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Idkham")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdkhamNavigationIdkham")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("NgayLapDt")
                        .HasColumnType("datetime2");

                    b.Property<int>("SoLuong")
                        .HasColumnType("int");

                    b.Property<decimal>("ThanhGia")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TongTien")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("IddonThuoc");

                    b.HasIndex("IddungCuNavigationIdsanPham");

                    b.HasIndex("IdkhamNavigationIdkham");

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

                    b.Property<string>("Idkham")
                        .HasColumnType("nvarchar(450)");

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
                    b.Property<string>("IdsanPham")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DonViTinh")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("IddungCu")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdsanPhamNavigationIdsanPham")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Loai")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("SoLuong")
                        .HasColumnType("int");

                    b.Property<string>("TenDungCu")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("IdsanPham");

                    b.HasIndex("IdsanPhamNavigationIdsanPham");

                    b.ToTable("Khos");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.LichSuNhapXuat", b =>
                {
                    b.Property<int>("MaLs")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaLs"));

                    b.Property<decimal>("Don")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("DonViTinh")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("IddungCu")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IddungCuNavigationIdsanPham")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Loai")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("NgayNhap")
                        .HasColumnType("datetime2");

                    b.Property<string>("NoiDung")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int?>("SoLuongNhapXuat")
                        .HasColumnType("int");

                    b.Property<string>("TenDungCu")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("ThanhTien")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("MaLs");

                    b.HasIndex("IddungCuNavigationIdsanPham");

                    b.ToTable("LichSuNhapXuats");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.NhanVien", b =>
                {
                    b.Property<int>("MaNv")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaNv"));

                    b.Property<string>("Hinh")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("KinhNghiem")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("MaCv")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MaCvNavigationMaCv")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Sdt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ten")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("TenDangNhap")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TenDangNhapNavigationTenDangNhap")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("MaNv");

                    b.HasIndex("MaCvNavigationMaCv");

                    b.HasIndex("TenDangNhapNavigationTenDangNhap");

                    b.ToTable("NhanViens");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.TaiKhoan", b =>
                {
                    b.Property<string>("TenDangNhap")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("MatKhau")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("TenDangNhap");

                    b.ToTable("TaiKhoans");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.ThiTruong", b =>
                {
                    b.Property<string>("IdsanPham")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

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

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.DanhSachKham", b =>
                {
                    b.HasOne("WebQuanLyNhaKhoa.Data.BenhNhan", "IdbenhNhanNavigation")
                        .WithMany("DanhSachKhams")
                        .HasForeignKey("IdbenhNhanNavigationIdbenhNhan");

                    b.HasOne("WebQuanLyNhaKhoa.Data.NhanVien", "MaNvNavigation")
                        .WithMany("DanhSachKhams")
                        .HasForeignKey("MaNvNavigationMaNv");

                    b.Navigation("IdbenhNhanNavigation");

                    b.Navigation("MaNvNavigation");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.DichVu", b =>
                {
                    b.HasOne("WebQuanLyNhaKhoa.Data.ChanDoan", "IdchanDoanNavigation")
                        .WithMany("DichVus")
                        .HasForeignKey("IdchanDoanNavigationIdchanDoan");

                    b.Navigation("IdchanDoanNavigation");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.DieuTri", b =>
                {
                    b.HasOne("WebQuanLyNhaKhoa.Data.DichVu", "IddichVuNavigation")
                        .WithMany("DieuTris")
                        .HasForeignKey("IddichVuNavigationIddichVu");

                    b.HasOne("WebQuanLyNhaKhoa.Data.Kho", "IddungCuNavigation")
                        .WithMany("DieuTris")
                        .HasForeignKey("IddungCuNavigationIdsanPham")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebQuanLyNhaKhoa.Data.DanhSachKham", "IdkhamNavigation")
                        .WithMany("DieuTris")
                        .HasForeignKey("IdkhamNavigationIdkham");

                    b.Navigation("IddichVuNavigation");

                    b.Navigation("IddungCuNavigation");

                    b.Navigation("IdkhamNavigation");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.DonThuoc", b =>
                {
                    b.HasOne("WebQuanLyNhaKhoa.Data.Kho", "IddungCuNavigation")
                        .WithMany("DonThuocs")
                        .HasForeignKey("IddungCuNavigationIdsanPham")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebQuanLyNhaKhoa.Data.DanhSachKham", "IdkhamNavigation")
                        .WithMany("DonThuocs")
                        .HasForeignKey("IdkhamNavigationIdkham");

                    b.Navigation("IddungCuNavigation");

                    b.Navigation("IdkhamNavigation");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.HoaDon", b =>
                {
                    b.HasOne("WebQuanLyNhaKhoa.Data.DieuTri", "IddieuTriNavigation")
                        .WithMany("HoaDons")
                        .HasForeignKey("IddieuTri")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebQuanLyNhaKhoa.Data.DonThuoc", "IddonThuocNavigation")
                        .WithMany("HoaDons")
                        .HasForeignKey("IddonThuoc")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WebQuanLyNhaKhoa.Data.DanhSachKham", "IdkhamNavigation")
                        .WithMany("HoaDons")
                        .HasForeignKey("Idkham")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("IddieuTriNavigation");

                    b.Navigation("IddonThuocNavigation");

                    b.Navigation("IdkhamNavigation");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.Kho", b =>
                {
                    b.HasOne("WebQuanLyNhaKhoa.Data.ThiTruong", "IdsanPhamNavigation")
                        .WithMany("Khos")
                        .HasForeignKey("IdsanPhamNavigationIdsanPham")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IdsanPhamNavigation");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.LichSuNhapXuat", b =>
                {
                    b.HasOne("WebQuanLyNhaKhoa.Data.Kho", "IddungCuNavigation")
                        .WithMany()
                        .HasForeignKey("IddungCuNavigationIdsanPham");

                    b.Navigation("IddungCuNavigation");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.NhanVien", b =>
                {
                    b.HasOne("WebQuanLyNhaKhoa.Data.ChucVu", "MaCvNavigation")
                        .WithMany("NhanViens")
                        .HasForeignKey("MaCvNavigationMaCv");

                    b.HasOne("WebQuanLyNhaKhoa.Data.TaiKhoan", "TenDangNhapNavigation")
                        .WithMany("NhanViens")
                        .HasForeignKey("TenDangNhapNavigationTenDangNhap")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MaCvNavigation");

                    b.Navigation("TenDangNhapNavigation");
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
                    b.Navigation("NhanViens");
                });

            modelBuilder.Entity("WebQuanLyNhaKhoa.Data.ThiTruong", b =>
                {
                    b.Navigation("Khos");
                });
#pragma warning restore 612, 618
        }
    }
}
