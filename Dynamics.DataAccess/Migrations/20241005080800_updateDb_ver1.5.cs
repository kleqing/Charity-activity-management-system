using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dynamics.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateDb_ver15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserToProjectTransactionHistories_ProjectResources_ResourceID",
                table: "UserToProjectTransactionHistories");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "UserToProjectTransactionHistories");

            migrationBuilder.DropColumn(
                name: "RequestAddress",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "OrganizationToProjectTransactionHistory");

            migrationBuilder.RenameColumn(
                name: "ResourceID",
                table: "UserToProjectTransactionHistories",
                newName: "ProjectResourceID");

            migrationBuilder.RenameIndex(
                name: "IX_UserToProjectTransactionHistories_ResourceID",
                table: "UserToProjectTransactionHistories",
                newName: "IX_UserToProjectTransactionHistories_ProjectResourceID");

            migrationBuilder.RenameColumn(
                name: "requestTitle",
                table: "Requests",
                newName: "RequestTitle");

            migrationBuilder.RenameColumn(
                name: "shutdownReanson",
                table: "Projects",
                newName: "ShutdownReason");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "UserToProjectTransactionHistories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserFullName",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserEmail",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "isEmergency",
                table: "Requests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Attachment",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ReportFile",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ExpectedQuantity",
                table: "ProjectResources",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OrganizationEmail",
                table: "Organizations",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Attachment",
                table: "Histories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReporterID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ObjectID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportID);
                    table.ForeignKey(
                        name: "FK_Reports_Users_ReporterID",
                        column: x => x.ReporterID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserEmail",
                table: "Users",
                column: "UserEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserFullName",
                table: "Users",
                column: "UserFullName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_OrganizationEmail",
                table: "Organizations",
                column: "OrganizationEmail",
                unique: true,
                filter: "[OrganizationEmail] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_OrganizationName",
                table: "Organizations",
                column: "OrganizationName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReporterID",
                table: "Reports",
                column: "ReporterID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserToProjectTransactionHistories_ProjectResources_ProjectResourceID",
                table: "UserToProjectTransactionHistories",
                column: "ProjectResourceID",
                principalTable: "ProjectResources",
                principalColumn: "ResourceID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserToProjectTransactionHistories_ProjectResources_ProjectResourceID",
                table: "UserToProjectTransactionHistories");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserEmail",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserFullName",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_OrganizationEmail",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_OrganizationName",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "ReportFile",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "ProjectResourceID",
                table: "UserToProjectTransactionHistories",
                newName: "ResourceID");

            migrationBuilder.RenameIndex(
                name: "IX_UserToProjectTransactionHistories_ProjectResourceID",
                table: "UserToProjectTransactionHistories",
                newName: "IX_UserToProjectTransactionHistories_ResourceID");

            migrationBuilder.RenameColumn(
                name: "RequestTitle",
                table: "Requests",
                newName: "requestTitle");

            migrationBuilder.RenameColumn(
                name: "ShutdownReason",
                table: "Projects",
                newName: "shutdownReanson");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "UserToProjectTransactionHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "UserToProjectTransactionHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserFullName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserEmail",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "isEmergency",
                table: "Requests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Attachment",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestAddress",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ExpectedQuantity",
                table: "ProjectResources",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "OrganizationToProjectTransactionHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "OrganizationEmail",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Attachment",
                table: "Histories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserToProjectTransactionHistories_ProjectResources_ResourceID",
                table: "UserToProjectTransactionHistories",
                column: "ResourceID",
                principalTable: "ProjectResources",
                principalColumn: "ResourceID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
