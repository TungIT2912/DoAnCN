using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebQuanLyNhaKhoa.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BenhNhan",
                columns: table => new
                {
                    IDBenhNhan = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Gioi = table.Column<bool>(type: "bit", nullable: true),
                    NamSinh = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: true),
                    SDT = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NgayKhamDau = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BenhNhan__33CC78E40A9D3BCF", x => x.IDBenhNhan);
                });

            migrationBuilder.CreateTable(
                name: "ChanDoan",
                columns: table => new
                {
                    IDChanDoan = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    TenChanDoan = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChanDoan__38CFF322B33E80A5", x => x.IDChanDoan);
                });

            migrationBuilder.CreateTable(
                name: "ChucVu",
                columns: table => new
                {
                    MaCV = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    TenCV = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChucVu__27258E76DBFE2135", x => x.MaCV);
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoan",
                columns: table => new
                {
                    TenDangNhap = table.Column<string>(type: "varchar(22)", unicode: false, maxLength: 22, nullable: false),
                    MatKhau = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TaiKhoan__55F68FC127E7A77D", x => x.TenDangNhap);
                });

            migrationBuilder.CreateTable(
                name: "ThiTruong",
                columns: table => new
                {
                    IDSanPham = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: false),
                    TenSanPham = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Loai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DonViTinh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DonGia = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ThiTruon__9D45E58A3AF783C3", x => x.IDSanPham);
                });

            migrationBuilder.CreateTable(
                name: "DichVu",
                columns: table => new
                {
                    IDDichVu = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: false),
                    IDChanDoan = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    TenDichVu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DonViTinh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DonGia = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DichVu__C0C95928C2C1BF83", x => x.IDDichVu);
                    table.ForeignKey(
                        name: "chk_IDChanDoan_DichVu",
                        column: x => x.IDChanDoan,
                        principalTable: "ChanDoan",
                        principalColumn: "IDChanDoan");
                });

            migrationBuilder.CreateTable(
                name: "NhanVien",
                columns: table => new
                {
                    MaNV = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "varchar(22)", unicode: false, maxLength: 22, nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SDT = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    MaCV = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    KinhNghiem = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Hinh = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NhanVien__2725D70A25545DAD", x => x.MaNV);
                    table.ForeignKey(
                        name: "chk_MaCV",
                        column: x => x.MaCV,
                        principalTable: "ChucVu",
                        principalColumn: "MaCV");
                    table.ForeignKey(
                        name: "chk_TenDangNhap",
                        column: x => x.TenDangNhap,
                        principalTable: "TaiKhoan",
                        principalColumn: "TenDangNhap");
                });

            migrationBuilder.CreateTable(
                name: "Kho",
                columns: table => new
                {
                    IDDungCu = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: false),
                    IDSanPham = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: false),
                    TenDungCu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Loai = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DonViTinh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Kho__50E8F16097B5FC5E", x => x.IDDungCu);
                    table.ForeignKey(
                        name: "chk_IDSanPham_Kho",
                        column: x => x.IDSanPham,
                        principalTable: "ThiTruong",
                        principalColumn: "IDSanPham");
                });

            migrationBuilder.CreateTable(
                name: "DanhSachKham",
                columns: table => new
                {
                    IDKham = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    IDBenhNhan = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    MaNV = table.Column<int>(type: "int", nullable: true),
                    NgayKham = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DanhSach__1AB7B4EBBB73E07A", x => x.IDKham);
                    table.ForeignKey(
                        name: "chk_IDBenhNhan_DanhSachKham",
                        column: x => x.IDBenhNhan,
                        principalTable: "BenhNhan",
                        principalColumn: "IDBenhNhan");
                    table.ForeignKey(
                        name: "chk_MaNV_DanhSachKham",
                        column: x => x.MaNV,
                        principalTable: "NhanVien",
                        principalColumn: "MaNV");
                });

            migrationBuilder.CreateTable(
                name: "LichSuNhapXuat",
                columns: table => new
                {
                    MaLS = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoiDung = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    IDDungCu = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: false),
                    TenDungCu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Loai = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DonViTinh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SoLuongNhapXuat = table.Column<int>(type: "int", nullable: true),
                    Don = table.Column<decimal>(type: "money", nullable: false),
                    ThanhTien = table.Column<decimal>(type: "money", nullable: false),
                    NgayNhap = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LichSuNh__2725C77279912860", x => x.MaLS);
                    table.ForeignKey(
                        name: "chk_IDDungCu_NK",
                        column: x => x.IDDungCu,
                        principalTable: "Kho",
                        principalColumn: "IDDungCu");
                });

            migrationBuilder.CreateTable(
                name: "DieuTri",
                columns: table => new
                {
                    IDDieuTri = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDDichVu = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: false),
                    IDKham = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    IDDungCu = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    ThanhTien = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DieuTri__85B3D346F07FFEB3", x => x.IDDieuTri);
                    table.ForeignKey(
                        name: "chk_IDDichVu_DieuTri",
                        column: x => x.IDDichVu,
                        principalTable: "DichVu",
                        principalColumn: "IDDichVu");
                    table.ForeignKey(
                        name: "chk_IDDungCu_DieuTri",
                        column: x => x.IDDungCu,
                        principalTable: "Kho",
                        principalColumn: "IDDungCu");
                    table.ForeignKey(
                        name: "chk_IDKham_DieuTri",
                        column: x => x.IDKham,
                        principalTable: "DanhSachKham",
                        principalColumn: "IDKham");
                });

            migrationBuilder.CreateTable(
                name: "DonThuoc",
                columns: table => new
                {
                    IDDonThuoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDKham = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: false),
                    IDDungCu = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    ThanhGia = table.Column<decimal>(type: "money", nullable: false),
                    TongTien = table.Column<decimal>(type: "money", nullable: false),
                    NgayLapDT = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DonThuoc__51DB14E889898F98", x => x.IDDonThuoc);
                    table.ForeignKey(
                        name: "chk_IDDungCu_DonThuoc",
                        column: x => x.IDDungCu,
                        principalTable: "Kho",
                        principalColumn: "IDDungCu");
                    table.ForeignKey(
                        name: "chk_IDKham_DonThuoc",
                        column: x => x.IDKham,
                        principalTable: "DanhSachKham",
                        principalColumn: "IDKham");
                });

            migrationBuilder.CreateTable(
                name: "HoaDon",
                columns: table => new
                {
                    IDHoaDon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDDonThuoc = table.Column<int>(type: "int", nullable: false),
                    IDDieuTri = table.Column<int>(type: "int", nullable: false),
                    IDKham = table.Column<string>(type: "char(3)", unicode: false, fixedLength: true, maxLength: 3, nullable: true),
                    PhuongThucThanhToan = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TienThuoc = table.Column<decimal>(type: "money", nullable: false),
                    TienDieuTri = table.Column<decimal>(type: "money", nullable: false),
                    TongTien = table.Column<decimal>(type: "money", nullable: false),
                    NgayLap = table.Column<DateTime>(type: "datetime", nullable: false),
                    EmailBN = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HoaDon__5B896F497F16612B", x => x.IDHoaDon);
                    table.ForeignKey(
                        name: "chk_IDDieuTri_HoaDon",
                        column: x => x.IDDieuTri,
                        principalTable: "DieuTri",
                        principalColumn: "IDDieuTri");
                    table.ForeignKey(
                        name: "chk_IDDonThuoc_HoaDon",
                        column: x => x.IDDonThuoc,
                        principalTable: "DonThuoc",
                        principalColumn: "IDDonThuoc");
                    table.ForeignKey(
                        name: "chk_IDKham_HoaDon",
                        column: x => x.IDKham,
                        principalTable: "DanhSachKham",
                        principalColumn: "IDKham");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DanhSachKham_IDBenhNhan",
                table: "DanhSachKham",
                column: "IDBenhNhan");

            migrationBuilder.CreateIndex(
                name: "IX_DanhSachKham_MaNV",
                table: "DanhSachKham",
                column: "MaNV");

            migrationBuilder.CreateIndex(
                name: "IX_DichVu_IDChanDoan",
                table: "DichVu",
                column: "IDChanDoan");

            migrationBuilder.CreateIndex(
                name: "IX_DieuTri_IDDichVu",
                table: "DieuTri",
                column: "IDDichVu");

            migrationBuilder.CreateIndex(
                name: "IX_DieuTri_IDDungCu",
                table: "DieuTri",
                column: "IDDungCu");

            migrationBuilder.CreateIndex(
                name: "IX_DieuTri_IDKham",
                table: "DieuTri",
                column: "IDKham");

            migrationBuilder.CreateIndex(
                name: "IX_DonThuoc_IDDungCu",
                table: "DonThuoc",
                column: "IDDungCu");

            migrationBuilder.CreateIndex(
                name: "IX_DonThuoc_IDKham",
                table: "DonThuoc",
                column: "IDKham");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_IDDieuTri",
                table: "HoaDon",
                column: "IDDieuTri");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_IDDonThuoc",
                table: "HoaDon",
                column: "IDDonThuoc");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_IDKham",
                table: "HoaDon",
                column: "IDKham");

            migrationBuilder.CreateIndex(
                name: "IX_Kho_IDSanPham",
                table: "Kho",
                column: "IDSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuNhapXuat_IDDungCu",
                table: "LichSuNhapXuat",
                column: "IDDungCu");

            migrationBuilder.CreateIndex(
                name: "IX_NhanVien_MaCV",
                table: "NhanVien",
                column: "MaCV");

            migrationBuilder.CreateIndex(
                name: "IX_NhanVien_TenDangNhap",
                table: "NhanVien",
                column: "TenDangNhap");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HoaDon");

            migrationBuilder.DropTable(
                name: "LichSuNhapXuat");

            migrationBuilder.DropTable(
                name: "DieuTri");

            migrationBuilder.DropTable(
                name: "DonThuoc");

            migrationBuilder.DropTable(
                name: "DichVu");

            migrationBuilder.DropTable(
                name: "Kho");

            migrationBuilder.DropTable(
                name: "DanhSachKham");

            migrationBuilder.DropTable(
                name: "ChanDoan");

            migrationBuilder.DropTable(
                name: "ThiTruong");

            migrationBuilder.DropTable(
                name: "BenhNhan");

            migrationBuilder.DropTable(
                name: "NhanVien");

            migrationBuilder.DropTable(
                name: "ChucVu");

            migrationBuilder.DropTable(
                name: "TaiKhoan");
        }
    }
}
