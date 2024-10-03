using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dynamics.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class deleteJLDateFromProjectMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProjectMembers_ProjectID_UserID_JoinDate",
                table: "ProjectMembers");

            migrationBuilder.DropColumn(
                name: "JoinDate",
                table: "ProjectMembers");

            migrationBuilder.DropColumn(
                name: "LeaveDate",
                table: "ProjectMembers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "JoinDate",
                table: "ProjectMembers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LeaveDate",
                table: "ProjectMembers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_ProjectID_UserID_JoinDate",
                table: "ProjectMembers",
                columns: new[] { "ProjectID", "UserID", "JoinDate" },
                unique: true,
                filter: "[JoinDate] IS NOT NULL");
        }
    }
}
