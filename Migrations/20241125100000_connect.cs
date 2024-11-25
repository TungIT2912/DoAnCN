using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebQuanLyNhaKhoa.Migrations
{
    /// <inheritdoc />
<<<<<<<< HEAD:Migrations/20241125100000_connect.cs
    public partial class connect : Migration
========
    public partial class Add : Migration
>>>>>>>> origin/Phuc:Migrations/20241124033734_Add.cs
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChanDoans",
                columns: table => new
                {
                    IdChanDoan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenChanDoan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChanDoans", x => x.IdChanDoan);
                });

            migrationBuilder.CreateTable(
                name: "ChucVus",
                columns: table => new
                {
                    MaCV = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenCv = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChucVus", x => x.MaCV);
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    isLoocked = table.Column<bool>(type: "bit", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaBN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaNV = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThiTruongs",
                columns: table => new
                {
                    IdsanPham = table.Column<int>(type: "int", maxLength: 50, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenSanPham = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Loai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DonViTinh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThiTruongs", x => x.IdsanPham);
                });

            migrationBuilder.CreateTable(
                name: "DichVus",
                columns: table => new
                {
                    IdDichVu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdchanDoan = table.Column<int>(type: "int", nullable: false),
                    TenDichVu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DonViTinh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DichVus", x => x.IdDichVu);
                    table.ForeignKey(
                        name: "FK_DichVus_ChanDoans_IdchanDoan",
                        column: x => x.IdchanDoan,
                        principalTable: "ChanDoans",
                        principalColumn: "IdChanDoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BenhNhans",
                columns: table => new
                {
                    IdBenhNhan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gioi = table.Column<bool>(type: "bit", nullable: true),
                    NamSinh = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    Sdt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TrieuChung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdChanDoan = table.Column<int>(type: "int", nullable: true),
                    ChanDoanIdchanDoan = table.Column<int>(type: "int", nullable: true),
                    NgayKhamDau = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmailBn = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenhNhans", x => x.IdBenhNhan);
                    table.ForeignKey(
                        name: "FK_BenhNhans_ChanDoans_ChanDoanIdchanDoan",
                        column: x => x.ChanDoanIdchanDoan,
                        principalTable: "ChanDoans",
                        principalColumn: "IdChanDoan");
                    table.ForeignKey(
                        name: "FK_BenhNhans_TaiKhoans_UserId",
                        column: x => x.UserId,
                        principalTable: "TaiKhoans",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NhanViens",
                columns: table => new
                {
                    MaNv = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sdt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaCv = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    KinhNghiem = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Gioi = table.Column<bool>(type: "bit", nullable: true),
                    Diachi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Hinh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanViens", x => x.MaNv);
                    table.ForeignKey(
                        name: "FK_NhanViens_ChucVus_MaCv",
                        column: x => x.MaCv,
                        principalTable: "ChucVus",
                        principalColumn: "MaCV",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NhanViens_TaiKhoans_UserId",
                        column: x => x.UserId,
                        principalTable: "TaiKhoans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Khos",
                columns: table => new
                {
                    IddungCu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdsanPham = table.Column<int>(type: "int", nullable: false),
                    Loai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DonViTinh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: true),
                    ThiTruongIdsanPham = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Khos", x => x.IddungCu);
                    table.ForeignKey(
                        name: "FK_Khos_ThiTruongs_IdsanPham",
                        column: x => x.IdsanPham,
                        principalTable: "ThiTruongs",
                        principalColumn: "IdsanPham",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Khos_ThiTruongs_ThiTruongIdsanPham",
                        column: x => x.ThiTruongIdsanPham,
                        principalTable: "ThiTruongs",
                        principalColumn: "IdsanPham");
                });

            migrationBuilder.CreateTable(
                name: "LichSuNhapXuats",
                columns: table => new
                {
                    MaLs = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoiDung = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IdsanPham = table.Column<int>(type: "int", nullable: false),
                    Loai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DonViTinh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SoLuongNhapXuat = table.Column<int>(type: "int", nullable: false),
                    Don = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ThanhTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NgayNhap = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichSuNhapXuats", x => x.MaLs);
                    table.ForeignKey(
                        name: "FK_LichSuNhapXuats_ThiTruongs_IdsanPham",
                        column: x => x.IdsanPham,
                        principalTable: "ThiTruongs",
                        principalColumn: "IdsanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DanhSachKhams",
                columns: table => new
                {
                    IdKham = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdbenhNhan = table.Column<int>(type: "int", nullable: false),
                    MaNv = table.Column<int>(type: "int", nullable: true),
                    NgayKham = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhSachKhams", x => x.IdKham);
                    table.ForeignKey(
                        name: "FK_DanhSachKhams_BenhNhans_IdbenhNhan",
                        column: x => x.IdbenhNhan,
                        principalTable: "BenhNhans",
                        principalColumn: "IdBenhNhan",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DanhSachKhams_NhanViens_MaNv",
                        column: x => x.MaNv,
                        principalTable: "NhanViens",
                        principalColumn: "MaNv");
                });

            migrationBuilder.CreateTable(
                name: "UnansweredQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AskedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAnswered = table.Column<bool>(type: "bit", nullable: false),
                    MaNv = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnansweredQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnansweredQuestions_NhanViens_MaNv",
                        column: x => x.MaNv,
                        principalTable: "NhanViens",
                        principalColumn: "MaNv");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietHoaDons",
                columns: table => new
                {
                    IdchiTiet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IddonThuoc = table.Column<int>(type: "int", nullable: false),
                    IdhoaDon = table.Column<int>(type: "int", nullable: false),
                    IddieuTri = table.Column<int>(type: "int", nullable: false),
                    Idkham = table.Column<int>(type: "int", nullable: true),
                    PhuongThucThanhToan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DaThanhToan = table.Column<bool>(type: "bit", nullable: false),
                    TenDon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenDieuTri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TienThuoc = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TienDieuTri = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NgayLap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmailBn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietHoaDons", x => x.IdchiTiet);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDons_DanhSachKhams_Idkham",
                        column: x => x.Idkham,
                        principalTable: "DanhSachKhams",
                        principalColumn: "IdKham");
                });

            migrationBuilder.CreateTable(
                name: "DieuTris",
                columns: table => new
                {
                    IddieuTri = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IddichVu = table.Column<int>(type: "int", nullable: false),
                    Idkham = table.Column<int>(type: "int", nullable: false),
                    IddungCu = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    ThanhTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChiTietHoaDonId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DieuTris", x => x.IddieuTri);
                    table.ForeignKey(
                        name: "FK_DieuTris_ChiTietHoaDons_ChiTietHoaDonId",
                        column: x => x.ChiTietHoaDonId,
                        principalTable: "ChiTietHoaDons",
                        principalColumn: "IdchiTiet");
                    table.ForeignKey(
                        name: "FK_DieuTris_DanhSachKhams_Idkham",
                        column: x => x.Idkham,
                        principalTable: "DanhSachKhams",
                        principalColumn: "IdKham",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DieuTris_DichVus_IddichVu",
                        column: x => x.IddichVu,
                        principalTable: "DichVus",
                        principalColumn: "IdDichVu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DieuTris_Khos_IddungCu",
                        column: x => x.IddungCu,
                        principalTable: "Khos",
                        principalColumn: "IddungCu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonThuocs",
                columns: table => new
                {
                    IddonThuoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Idkham = table.Column<int>(type: "int", nullable: false),
                    IddungCu = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    ThanhGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NgayLapDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ChiTietHoaDonId = table.Column<int>(type: "int", nullable: true),
                    DonThuocIddonThuoc = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonThuocs", x => x.IddonThuoc);
                    table.ForeignKey(
                        name: "FK_DonThuocs_ChiTietHoaDons_ChiTietHoaDonId",
                        column: x => x.ChiTietHoaDonId,
                        principalTable: "ChiTietHoaDons",
                        principalColumn: "IdchiTiet");
                    table.ForeignKey(
                        name: "FK_DonThuocs_DanhSachKhams_Idkham",
                        column: x => x.Idkham,
                        principalTable: "DanhSachKhams",
                        principalColumn: "IdKham",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DonThuocs_DonThuocs_DonThuocIddonThuoc",
                        column: x => x.DonThuocIddonThuoc,
                        principalTable: "DonThuocs",
                        principalColumn: "IddonThuoc");
                    table.ForeignKey(
                        name: "FK_DonThuocs_Khos_IddungCu",
                        column: x => x.IddungCu,
                        principalTable: "Khos",
                        principalColumn: "IddungCu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoaDons",
                columns: table => new
                {
                    IdhoaDon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IddonThuoc = table.Column<int>(type: "int", nullable: true),
                    IddieuTri = table.Column<int>(type: "int", nullable: false),
                    Idkham = table.Column<int>(type: "int", nullable: true),
                    PhuongThucThanhToan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TienThuoc = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TienDieuTri = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NgayLap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmailBn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DaThanhToan = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDons", x => x.IdhoaDon);
                    table.ForeignKey(
                        name: "FK_HoaDons_DanhSachKhams_Idkham",
                        column: x => x.Idkham,
                        principalTable: "DanhSachKhams",
                        principalColumn: "IdKham",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HoaDons_DieuTris_IddieuTri",
                        column: x => x.IddieuTri,
                        principalTable: "DieuTris",
                        principalColumn: "IddieuTri",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoaDons_DonThuocs_IddonThuoc",
                        column: x => x.IddonThuoc,
                        principalTable: "DonThuocs",
                        principalColumn: "IddonThuoc",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BenhNhans_ChanDoanIdchanDoan",
                table: "BenhNhans",
                column: "ChanDoanIdchanDoan");

            migrationBuilder.CreateIndex(
                name: "IX_BenhNhans_UserId",
                table: "BenhNhans",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDons_IdhoaDon",
                table: "ChiTietHoaDons",
                column: "IdhoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDons_Idkham",
                table: "ChiTietHoaDons",
                column: "Idkham");

            migrationBuilder.CreateIndex(
                name: "IX_DanhSachKhams_IdbenhNhan",
                table: "DanhSachKhams",
                column: "IdbenhNhan");

            migrationBuilder.CreateIndex(
                name: "IX_DanhSachKhams_MaNv",
                table: "DanhSachKhams",
                column: "MaNv");

            migrationBuilder.CreateIndex(
                name: "IX_DichVus_IdchanDoan",
                table: "DichVus",
                column: "IdchanDoan");

            migrationBuilder.CreateIndex(
                name: "IX_DieuTris_ChiTietHoaDonId",
                table: "DieuTris",
                column: "ChiTietHoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_DieuTris_IddichVu",
                table: "DieuTris",
                column: "IddichVu");

            migrationBuilder.CreateIndex(
                name: "IX_DieuTris_IddungCu",
                table: "DieuTris",
                column: "IddungCu");

            migrationBuilder.CreateIndex(
                name: "IX_DieuTris_Idkham",
                table: "DieuTris",
                column: "Idkham");

            migrationBuilder.CreateIndex(
                name: "IX_DonThuocs_ChiTietHoaDonId",
                table: "DonThuocs",
                column: "ChiTietHoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_DonThuocs_DonThuocIddonThuoc",
                table: "DonThuocs",
                column: "DonThuocIddonThuoc");

            migrationBuilder.CreateIndex(
                name: "IX_DonThuocs_IddungCu",
                table: "DonThuocs",
                column: "IddungCu");

            migrationBuilder.CreateIndex(
                name: "IX_DonThuocs_Idkham",
                table: "DonThuocs",
                column: "Idkham");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_IddieuTri",
                table: "HoaDons",
                column: "IddieuTri");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_IddonThuoc",
                table: "HoaDons",
                column: "IddonThuoc");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_Idkham",
                table: "HoaDons",
                column: "Idkham");

            migrationBuilder.CreateIndex(
                name: "IX_Khos_IdsanPham",
                table: "Khos",
                column: "IdsanPham");

            migrationBuilder.CreateIndex(
                name: "IX_Khos_ThiTruongIdsanPham",
                table: "Khos",
                column: "ThiTruongIdsanPham");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuNhapXuats_IdsanPham",
                table: "LichSuNhapXuats",
                column: "IdsanPham");

            migrationBuilder.CreateIndex(
                name: "IX_NhanViens_MaCv",
                table: "NhanViens",
                column: "MaCv");

            migrationBuilder.CreateIndex(
                name: "IX_NhanViens_UserId",
                table: "NhanViens",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnansweredQuestions_MaNv",
                table: "UnansweredQuestions",
                column: "MaNv");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietHoaDons_HoaDons_IdhoaDon",
                table: "ChiTietHoaDons",
                column: "IdhoaDon",
                principalTable: "HoaDons",
                principalColumn: "IdhoaDon",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BenhNhans_ChanDoans_ChanDoanIdchanDoan",
                table: "BenhNhans");

            migrationBuilder.DropForeignKey(
                name: "FK_DichVus_ChanDoans_IdchanDoan",
                table: "DichVus");

            migrationBuilder.DropForeignKey(
                name: "FK_BenhNhans_TaiKhoans_UserId",
                table: "BenhNhans");

            migrationBuilder.DropForeignKey(
                name: "FK_NhanViens_TaiKhoans_UserId",
                table: "NhanViens");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietHoaDons_DanhSachKhams_Idkham",
                table: "ChiTietHoaDons");

            migrationBuilder.DropForeignKey(
                name: "FK_DieuTris_DanhSachKhams_Idkham",
                table: "DieuTris");

            migrationBuilder.DropForeignKey(
                name: "FK_DonThuocs_DanhSachKhams_Idkham",
                table: "DonThuocs");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_DanhSachKhams_Idkham",
                table: "HoaDons");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietHoaDons_HoaDons_IdhoaDon",
                table: "ChiTietHoaDons");

            migrationBuilder.DropTable(
                name: "LichSuNhapXuats");

            migrationBuilder.DropTable(
                name: "UnansweredQuestions");

            migrationBuilder.DropTable(
                name: "ChanDoans");

            migrationBuilder.DropTable(
                name: "TaiKhoans");

            migrationBuilder.DropTable(
                name: "DanhSachKhams");

            migrationBuilder.DropTable(
                name: "BenhNhans");

            migrationBuilder.DropTable(
                name: "NhanViens");

            migrationBuilder.DropTable(
                name: "ChucVus");

            migrationBuilder.DropTable(
                name: "HoaDons");

            migrationBuilder.DropTable(
                name: "DieuTris");

            migrationBuilder.DropTable(
                name: "DonThuocs");

            migrationBuilder.DropTable(
                name: "DichVus");

            migrationBuilder.DropTable(
                name: "ChiTietHoaDons");

            migrationBuilder.DropTable(
                name: "Khos");

            migrationBuilder.DropTable(
                name: "ThiTruongs");
        }
    }
}
