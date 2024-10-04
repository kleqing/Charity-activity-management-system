using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dynamics.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReportTB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReporterID",
                table: "Reports",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReporterID",
                table: "Reports",
                column: "ReporterID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_ReporterID",
                table: "Reports",
                column: "ReporterID",
                principalTable: "Users",
                principalColumn: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_ReporterID",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ReporterID",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ReporterID",
                table: "Reports");
        }
    }
}
