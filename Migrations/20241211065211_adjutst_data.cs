using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebQuanLyNhaKhoa.Migrations
{
    /// <inheritdoc />
    public partial class adjutst_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResponseForms_RegisForms_IdFormRegis",
                table: "ResponseForms");

            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "RegisForms");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "RegisForms");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "RegisForms");

            migrationBuilder.RenameColumn(
                name: "IdFormRegis",
                table: "ResponseForms",
                newName: "RegisFormId");

            migrationBuilder.RenameIndex(
                name: "IX_ResponseForms_IdFormRegis",
                table: "ResponseForms",
                newName: "IX_ResponseForms_RegisFormId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDay",
                table: "RegisForms",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "NewShifts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegisFormId = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewShifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewShifts_RegisForms_RegisFormId",
                        column: x => x.RegisFormId,
                        principalTable: "RegisForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewShifts_RegisFormId",
                table: "NewShifts",
                column: "RegisFormId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResponseForms_RegisForms_RegisFormId",
                table: "ResponseForms",
                column: "RegisFormId",
                principalTable: "RegisForms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResponseForms_RegisForms_RegisFormId",
                table: "ResponseForms");

            migrationBuilder.DropTable(
                name: "NewShifts");

            migrationBuilder.DropColumn(
                name: "CreateDay",
                table: "RegisForms");

            migrationBuilder.RenameColumn(
                name: "RegisFormId",
                table: "ResponseForms",
                newName: "IdFormRegis");

            migrationBuilder.RenameIndex(
                name: "IX_ResponseForms_RegisFormId",
                table: "ResponseForms",
                newName: "IX_ResponseForms_IdFormRegis");

            migrationBuilder.AddColumn<string>(
                name: "DayOfWeek",
                table: "RegisForms",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                table: "RegisForms",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                table: "RegisForms",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddForeignKey(
                name: "FK_ResponseForms_RegisForms_IdFormRegis",
                table: "ResponseForms",
                column: "IdFormRegis",
                principalTable: "RegisForms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
