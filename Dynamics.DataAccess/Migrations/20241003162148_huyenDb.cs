using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dynamics.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class huyenDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserToProjectTransactionHistories_ProjectResources_ResourceID",
                table: "UserToProjectTransactionHistories");

            migrationBuilder.DropColumn(
                name: "LeaderID",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CEOID",
                table: "Organizations");

            migrationBuilder.RenameColumn(
                name: "ResourceID",
                table: "UserToProjectTransactionHistories",
                newName: "ProjectResourceID");

            migrationBuilder.RenameIndex(
                name: "IX_UserToProjectTransactionHistories_ResourceID",
                table: "UserToProjectTransactionHistories",
                newName: "IX_UserToProjectTransactionHistories_ProjectResourceID");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "UserToProjectTransactionHistories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectID",
                table: "UserToProjectTransactionHistories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationID",
                table: "UserToOrganizationTransactionHistories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "isEmergency",
                table: "Requests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "RequestPhoneNumber",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Attachment",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectPhoneNumber",
                table: "Projects",
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

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectID",
                table: "OrganizationToProjectTransactionHistory",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OrganizationDescription",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "OrganizationResources",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "OrganizationMember",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
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
                name: "IX_UserToProjectTransactionHistories_ProjectID",
                table: "UserToProjectTransactionHistories",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationToProjectTransactionHistory_ProjectID",
                table: "OrganizationToProjectTransactionHistory",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReporterID",
                table: "Reports",
                column: "ReporterID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationToProjectTransactionHistory_Projects_ProjectID",
                table: "OrganizationToProjectTransactionHistory",
                column: "ProjectID",
                principalTable: "Projects",
                principalColumn: "ProjectID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserToProjectTransactionHistories_ProjectResources_ProjectResourceID",
                table: "UserToProjectTransactionHistories",
                column: "ProjectResourceID",
                principalTable: "ProjectResources",
                principalColumn: "ResourceID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserToProjectTransactionHistories_Projects_ProjectID",
                table: "UserToProjectTransactionHistories",
                column: "ProjectID",
                principalTable: "Projects",
                principalColumn: "ProjectID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationToProjectTransactionHistory_Projects_ProjectID",
                table: "OrganizationToProjectTransactionHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_UserToProjectTransactionHistories_ProjectResources_ProjectResourceID",
                table: "UserToProjectTransactionHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserToProjectTransactionHistories_Projects_ProjectID",
                table: "UserToProjectTransactionHistories");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_UserToProjectTransactionHistories_ProjectID",
                table: "UserToProjectTransactionHistories");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationToProjectTransactionHistory_ProjectID",
                table: "OrganizationToProjectTransactionHistory");

            migrationBuilder.DropColumn(
                name: "ProjectID",
                table: "UserToProjectTransactionHistories");

            migrationBuilder.DropColumn(
                name: "OrganizationID",
                table: "UserToOrganizationTransactionHistories");

            migrationBuilder.DropColumn(
                name: "ReportFile",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectID",
                table: "OrganizationToProjectTransactionHistory");

            migrationBuilder.RenameColumn(
                name: "ProjectResourceID",
                table: "UserToProjectTransactionHistories",
                newName: "ResourceID");

            migrationBuilder.RenameIndex(
                name: "IX_UserToProjectTransactionHistories_ProjectResourceID",
                table: "UserToProjectTransactionHistories",
                newName: "IX_UserToProjectTransactionHistories_ResourceID");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "UserToProjectTransactionHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
                name: "RequestPhoneNumber",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Attachment",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProjectPhoneNumber",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LeaderID",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ExpectedQuantity",
                table: "ProjectResources",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "OrganizationDescription",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CEOID",
                table: "Organizations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "OrganizationResources",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "OrganizationMember",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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
