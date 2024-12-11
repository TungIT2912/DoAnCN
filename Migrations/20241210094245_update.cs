using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebQuanLyNhaKhoa.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegisForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNv = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ReasonForChange = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegisForms_NhanViens_MaNv",
                        column: x => x.MaNv,
                        principalTable: "NhanViens",
                        principalColumn: "MaNv",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResponseForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdFormRegis = table.Column<int>(type: "int", nullable: false),
                    ResponseStatus = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponseForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResponseForms_RegisForms_IdFormRegis",
                        column: x => x.IdFormRegis,
                        principalTable: "RegisForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegisForms_MaNv",
                table: "RegisForms",
                column: "MaNv");

            migrationBuilder.CreateIndex(
                name: "IX_ResponseForms_IdFormRegis",
                table: "ResponseForms",
                column: "IdFormRegis");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResponseForms");

            migrationBuilder.DropTable(
                name: "RegisForms");
        }
    }
}
