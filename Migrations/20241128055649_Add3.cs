using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebQuanLyNhaKhoa.Migrations
{
    /// <inheritdoc />
    public partial class Add3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DichVus_NhanViens_AssignedDoctorMaNv",
                table: "DichVus");

            migrationBuilder.DropIndex(
                name: "IX_DichVus_AssignedDoctorMaNv",
                table: "DichVus");

            migrationBuilder.DropColumn(
                name: "AssignedDoctorMaNv",
                table: "DichVus");

            migrationBuilder.DropColumn(
                name: "Day",
                table: "DichVus");

            migrationBuilder.DropColumn(
                name: "TimeSlot",
                table: "DichVus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedDoctorMaNv",
                table: "DichVus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Day",
                table: "DichVus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeSlot",
                table: "DichVus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DichVus_AssignedDoctorMaNv",
                table: "DichVus",
                column: "AssignedDoctorMaNv");

            migrationBuilder.AddForeignKey(
                name: "FK_DichVus_NhanViens_AssignedDoctorMaNv",
                table: "DichVus",
                column: "AssignedDoctorMaNv",
                principalTable: "NhanViens",
                principalColumn: "MaNv");
        }
    }
}
