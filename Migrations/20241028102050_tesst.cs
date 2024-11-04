using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebQuanLyNhaKhoa.Migrations
{
    /// <inheritdoc />
    public partial class tesst : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BenhNhans",
                columns: table => new
                {
                    IdBenhNhan = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gioi = table.Column<bool>(type: "bit", nullable: true),
                    NamSinh = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    Sdt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NgayKhamDau = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenhNhans", x => x.IdBenhNhan);
                });

            migrationBuilder.CreateTable(
                name: "ChanDoans",
                columns: table => new
                {
                    IdChanDoan = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    MaCV = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenCv = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChucVus", x => x.MaCV);
                });

            migrationBuilder.CreateTable(
                name: "ThiTruongs",
                columns: table => new
                {
                    IdsanPham = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DichVus",
                columns: table => new
                {
                    IdDichVu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdchanDoan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenDichVu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DonViTinh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IdchanDoanNavigationIdchanDoan = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DichVus", x => x.IdDichVu);
                    table.ForeignKey(
                        name: "FK_DichVus_ChanDoans_IdchanDoanNavigationIdchanDoan",
                        column: x => x.IdchanDoanNavigationIdchanDoan,
                        principalTable: "ChanDoans",
                        principalColumn: "IdChanDoan");
                });

            migrationBuilder.CreateTable(
                name: "NhanViens",
                columns: table => new
                {
                    MaNv = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sdt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaCv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KinhNghiem = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Diachi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Hinh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MaCvNavigationMaCv = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanViens", x => x.MaNv);
                    table.ForeignKey(
                        name: "FK_NhanViens_ChucVus_MaCvNavigationMaCv",
                        column: x => x.MaCvNavigationMaCv,
                        principalTable: "ChucVus",
                        principalColumn: "MaCV");
                });

            migrationBuilder.CreateTable(
                name: "Khos",
                columns: table => new
                {
                    IdsanPham = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IddungCu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenDungCu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Loai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DonViTinh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: true),
                    IdsanPhamNavigationIdsanPham = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Khos", x => x.IdsanPham);
                    table.ForeignKey(
                        name: "FK_Khos_ThiTruongs_IdsanPhamNavigationIdsanPham",
                        column: x => x.IdsanPhamNavigationIdsanPham,
                        principalTable: "ThiTruongs",
                        principalColumn: "IdsanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AvatarPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerID = table.Column<int>(type: "int", nullable: true),
                    StaffId = table.Column<int>(type: "int", nullable: true),
                    ChucVu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerIdbenhNhan = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_BenhNhans_CustomerIdbenhNhan",
                        column: x => x.CustomerIdbenhNhan,
                        principalTable: "BenhNhans",
                        principalColumn: "IdBenhNhan");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_NhanViens_StaffId",
                        column: x => x.StaffId,
                        principalTable: "NhanViens",
                        principalColumn: "MaNv");
                });

            migrationBuilder.CreateTable(
                name: "DanhSachKhams",
                columns: table => new
                {
                    IdKham = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdbenhNhan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaNv = table.Column<int>(type: "int", nullable: true),
                    NgayKham = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdbenhNhanNavigationIdbenhNhan = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MaNvNavigationMaNv = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhSachKhams", x => x.IdKham);
                    table.ForeignKey(
                        name: "FK_DanhSachKhams_BenhNhans_IdbenhNhanNavigationIdbenhNhan",
                        column: x => x.IdbenhNhanNavigationIdbenhNhan,
                        principalTable: "BenhNhans",
                        principalColumn: "IdBenhNhan");
                    table.ForeignKey(
                        name: "FK_DanhSachKhams_NhanViens_MaNvNavigationMaNv",
                        column: x => x.MaNvNavigationMaNv,
                        principalTable: "NhanViens",
                        principalColumn: "MaNv");
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoans",
                columns: table => new
                {
                    TenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MaBN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BenhNhanIdbenhNhan = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MaNV = table.Column<int>(type: "int", nullable: true),
                    NhanVienMaNv = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoans", x => x.TenDangNhap);
                    table.ForeignKey(
                        name: "FK_TaiKhoans_BenhNhans_BenhNhanIdbenhNhan",
                        column: x => x.BenhNhanIdbenhNhan,
                        principalTable: "BenhNhans",
                        principalColumn: "IdBenhNhan");
                    table.ForeignKey(
                        name: "FK_TaiKhoans_NhanViens_NhanVienMaNv",
                        column: x => x.NhanVienMaNv,
                        principalTable: "NhanViens",
                        principalColumn: "MaNv");
                });

            migrationBuilder.CreateTable(
                name: "LichSuNhapXuats",
                columns: table => new
                {
                    MaLs = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoiDung = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IddungCu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenDungCu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Loai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DonViTinh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SoLuongNhapXuat = table.Column<int>(type: "int", nullable: true),
                    Don = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ThanhTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NgayNhap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IddungCuNavigationIdsanPham = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichSuNhapXuats", x => x.MaLs);
                    table.ForeignKey(
                        name: "FK_LichSuNhapXuats_Khos_IddungCuNavigationIdsanPham",
                        column: x => x.IddungCuNavigationIdsanPham,
                        principalTable: "Khos",
                        principalColumn: "IdsanPham");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DieuTris",
                columns: table => new
                {
                    IddieuTri = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IddichVu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Idkham = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IddungCu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    ThanhTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IddichVuNavigationIddichVu = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IddungCuNavigationIdsanPham = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdkhamNavigationIdkham = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DieuTris", x => x.IddieuTri);
                    table.ForeignKey(
                        name: "FK_DieuTris_DanhSachKhams_IdkhamNavigationIdkham",
                        column: x => x.IdkhamNavigationIdkham,
                        principalTable: "DanhSachKhams",
                        principalColumn: "IdKham");
                    table.ForeignKey(
                        name: "FK_DieuTris_DichVus_IddichVuNavigationIddichVu",
                        column: x => x.IddichVuNavigationIddichVu,
                        principalTable: "DichVus",
                        principalColumn: "IdDichVu");
                    table.ForeignKey(
                        name: "FK_DieuTris_Khos_IddungCuNavigationIdsanPham",
                        column: x => x.IddungCuNavigationIdsanPham,
                        principalTable: "Khos",
                        principalColumn: "IdsanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonThuocs",
                columns: table => new
                {
                    IddonThuoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Idkham = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IddungCu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    ThanhGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NgayLapDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IddungCuNavigationIdsanPham = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdkhamNavigationIdkham = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonThuocs", x => x.IddonThuoc);
                    table.ForeignKey(
                        name: "FK_DonThuocs_DanhSachKhams_IdkhamNavigationIdkham",
                        column: x => x.IdkhamNavigationIdkham,
                        principalTable: "DanhSachKhams",
                        principalColumn: "IdKham");
                    table.ForeignKey(
                        name: "FK_DonThuocs_Khos_IddungCuNavigationIdsanPham",
                        column: x => x.IddungCuNavigationIdsanPham,
                        principalTable: "Khos",
                        principalColumn: "IdsanPham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoaDons",
                columns: table => new
                {
                    IdhoaDon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IddonThuoc = table.Column<int>(type: "int", nullable: false),
                    IddieuTri = table.Column<int>(type: "int", nullable: false),
                    Idkham = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PhuongThucThanhToan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TienThuoc = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TienDieuTri = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NgayLap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmailBn = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CustomerIdbenhNhan",
                table: "AspNetUsers",
                column: "CustomerIdbenhNhan");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_StaffId",
                table: "AspNetUsers",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DanhSachKhams_IdbenhNhanNavigationIdbenhNhan",
                table: "DanhSachKhams",
                column: "IdbenhNhanNavigationIdbenhNhan");

            migrationBuilder.CreateIndex(
                name: "IX_DanhSachKhams_MaNvNavigationMaNv",
                table: "DanhSachKhams",
                column: "MaNvNavigationMaNv");

            migrationBuilder.CreateIndex(
                name: "IX_DichVus_IdchanDoanNavigationIdchanDoan",
                table: "DichVus",
                column: "IdchanDoanNavigationIdchanDoan");

            migrationBuilder.CreateIndex(
                name: "IX_DieuTris_IddichVuNavigationIddichVu",
                table: "DieuTris",
                column: "IddichVuNavigationIddichVu");

            migrationBuilder.CreateIndex(
                name: "IX_DieuTris_IddungCuNavigationIdsanPham",
                table: "DieuTris",
                column: "IddungCuNavigationIdsanPham");

            migrationBuilder.CreateIndex(
                name: "IX_DieuTris_IdkhamNavigationIdkham",
                table: "DieuTris",
                column: "IdkhamNavigationIdkham");

            migrationBuilder.CreateIndex(
                name: "IX_DonThuocs_IddungCuNavigationIdsanPham",
                table: "DonThuocs",
                column: "IddungCuNavigationIdsanPham");

            migrationBuilder.CreateIndex(
                name: "IX_DonThuocs_IdkhamNavigationIdkham",
                table: "DonThuocs",
                column: "IdkhamNavigationIdkham");

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
                name: "IX_Khos_IdsanPhamNavigationIdsanPham",
                table: "Khos",
                column: "IdsanPhamNavigationIdsanPham");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuNhapXuats_IddungCuNavigationIdsanPham",
                table: "LichSuNhapXuats",
                column: "IddungCuNavigationIdsanPham");

            migrationBuilder.CreateIndex(
                name: "IX_NhanViens_MaCvNavigationMaCv",
                table: "NhanViens",
                column: "MaCvNavigationMaCv");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoans_BenhNhanIdbenhNhan",
                table: "TaiKhoans",
                column: "BenhNhanIdbenhNhan");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoans_NhanVienMaNv",
                table: "TaiKhoans",
                column: "NhanVienMaNv");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "HoaDons");

            migrationBuilder.DropTable(
                name: "LichSuNhapXuats");

            migrationBuilder.DropTable(
                name: "TaiKhoans");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "DieuTris");

            migrationBuilder.DropTable(
                name: "DonThuocs");

            migrationBuilder.DropTable(
                name: "DichVus");

            migrationBuilder.DropTable(
                name: "DanhSachKhams");

            migrationBuilder.DropTable(
                name: "Khos");

            migrationBuilder.DropTable(
                name: "ChanDoans");

            migrationBuilder.DropTable(
                name: "BenhNhans");

            migrationBuilder.DropTable(
                name: "NhanViens");

            migrationBuilder.DropTable(
                name: "ThiTruongs");

            migrationBuilder.DropTable(
                name: "ChucVus");
        }
    }
}
