using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dynamics.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateDb_ver20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationToProjectTransactionHistory_Organizations_OrganizationID",
                table: "OrganizationToProjectTransactionHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationToProjectTransactionHistory_Projects_ProjectID",
                table: "OrganizationToProjectTransactionHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_UserToOrganizationTransactionHistories_Organizations_OrganizationID",
                table: "UserToOrganizationTransactionHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserToProjectTransactionHistories_Projects_ProjectID",
                table: "UserToProjectTransactionHistories");

            migrationBuilder.DropIndex(
                name: "IX_UserToOrganizationTransactionHistories_OrganizationID",
                table: "UserToOrganizationTransactionHistories");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationToProjectTransactionHistory_OrganizationID",
                table: "OrganizationToProjectTransactionHistory");

            migrationBuilder.DropColumn(
                name: "MoneyTransactionAmout",
                table: "UserToProjectTransactionHistories");

            migrationBuilder.DropColumn(
                name: "ResourceName",
                table: "UserToProjectTransactionHistories");

            migrationBuilder.DropColumn(
                name: "OrganizationID",
                table: "OrganizationToProjectTransactionHistory");

            migrationBuilder.DropColumn(
                name: "ResourceName",
                table: "OrganizationToProjectTransactionHistory");

            migrationBuilder.RenameColumn(
                name: "ProjectID",
                table: "UserToProjectTransactionHistories",
                newName: "ResourceID");

            migrationBuilder.RenameIndex(
                name: "IX_UserToProjectTransactionHistories_ProjectID",
                table: "UserToProjectTransactionHistories",
                newName: "IX_UserToProjectTransactionHistories_ResourceID");

            migrationBuilder.RenameColumn(
                name: "OrganizationID",
                table: "UserToOrganizationTransactionHistories",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "MoneyTransactionAmout",
                table: "UserToOrganizationTransactionHistories",
                newName: "Unit");

            migrationBuilder.RenameColumn(
                name: "ProjectPhone",
                table: "Projects",
                newName: "shutdownReanson");

            migrationBuilder.RenameColumn(
                name: "ProjectID",
                table: "OrganizationToProjectTransactionHistory",
                newName: "OrganizationResourceID");

            migrationBuilder.RenameIndex(
                name: "IX_OrganizationToProjectTransactionHistory_ProjectID",
                table: "OrganizationToProjectTransactionHistory",
                newName: "IX_OrganizationToProjectTransactionHistory_OrganizationResourceID");

            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "UserToProjectTransactionHistories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Time",
                table: "UserToProjectTransactionHistories",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "UserToOrganizationTransactionHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResourceID",
                table: "UserToOrganizationTransactionHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProjectPhoneNumber",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "OrganizationToProjectTransactionHistory",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ProjectResourceID",
                table: "OrganizationToProjectTransactionHistory",
                type: "int",
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

            migrationBuilder.CreateIndex(
                name: "IX_UserToOrganizationTransactionHistories_ResourceID",
                table: "UserToOrganizationTransactionHistories",
                column: "ResourceID");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationToProjectTransactionHistory_ProjectResourceID",
                table: "OrganizationToProjectTransactionHistory",
                column: "ProjectResourceID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationToProjectTransactionHistory_OrganizationResources_OrganizationResourceID",
                table: "OrganizationToProjectTransactionHistory",
                column: "OrganizationResourceID",
                principalTable: "OrganizationResources",
                principalColumn: "ResourceID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationToProjectTransactionHistory_ProjectResources_ProjectResourceID",
                table: "OrganizationToProjectTransactionHistory",
                column: "ProjectResourceID",
                principalTable: "ProjectResources",
                principalColumn: "ResourceID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserToOrganizationTransactionHistories_OrganizationResources_ResourceID",
                table: "UserToOrganizationTransactionHistories",
                column: "ResourceID",
                principalTable: "OrganizationResources",
                principalColumn: "ResourceID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserToProjectTransactionHistories_ProjectResources_ResourceID",
                table: "UserToProjectTransactionHistories",
                column: "ResourceID",
                principalTable: "ProjectResources",
                principalColumn: "ResourceID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationToProjectTransactionHistory_OrganizationResources_OrganizationResourceID",
                table: "OrganizationToProjectTransactionHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationToProjectTransactionHistory_ProjectResources_ProjectResourceID",
                table: "OrganizationToProjectTransactionHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_UserToOrganizationTransactionHistories_OrganizationResources_ResourceID",
                table: "UserToOrganizationTransactionHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserToProjectTransactionHistories_ProjectResources_ResourceID",
                table: "UserToProjectTransactionHistories");

            migrationBuilder.DropIndex(
                name: "IX_UserToOrganizationTransactionHistories_ResourceID",
                table: "UserToOrganizationTransactionHistories");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationToProjectTransactionHistory_ProjectResourceID",
                table: "OrganizationToProjectTransactionHistory");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "UserToOrganizationTransactionHistories");

            migrationBuilder.DropColumn(
                name: "ResourceID",
                table: "UserToOrganizationTransactionHistories");

            migrationBuilder.DropColumn(
                name: "ProjectPhoneNumber",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectResourceID",
                table: "OrganizationToProjectTransactionHistory");

            migrationBuilder.RenameColumn(
                name: "ResourceID",
                table: "UserToProjectTransactionHistories",
                newName: "ProjectID");

            migrationBuilder.RenameIndex(
                name: "IX_UserToProjectTransactionHistories_ResourceID",
                table: "UserToProjectTransactionHistories",
                newName: "IX_UserToProjectTransactionHistories_ProjectID");

            migrationBuilder.RenameColumn(
                name: "Unit",
                table: "UserToOrganizationTransactionHistories",
                newName: "MoneyTransactionAmout");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "UserToOrganizationTransactionHistories",
                newName: "OrganizationID");

            migrationBuilder.RenameColumn(
                name: "shutdownReanson",
                table: "Projects",
                newName: "ProjectPhone");

            migrationBuilder.RenameColumn(
                name: "OrganizationResourceID",
                table: "OrganizationToProjectTransactionHistory",
                newName: "ProjectID");

            migrationBuilder.RenameIndex(
                name: "IX_OrganizationToProjectTransactionHistory_OrganizationResourceID",
                table: "OrganizationToProjectTransactionHistory",
                newName: "IX_OrganizationToProjectTransactionHistory_ProjectID");

            migrationBuilder.AlterColumn<int>(
                name: "Unit",
                table: "UserToProjectTransactionHistories",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Time",
                table: "UserToProjectTransactionHistories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<string>(
                name: "MoneyTransactionAmout",
                table: "UserToProjectTransactionHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResourceName",
                table: "UserToProjectTransactionHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Unit",
                table: "OrganizationToProjectTransactionHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "OrganizationID",
                table: "OrganizationToProjectTransactionHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ResourceName",
                table: "OrganizationToProjectTransactionHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "OrganizationResources",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_UserToOrganizationTransactionHistories_OrganizationID",
                table: "UserToOrganizationTransactionHistories",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationToProjectTransactionHistory_OrganizationID",
                table: "OrganizationToProjectTransactionHistory",
                column: "OrganizationID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationToProjectTransactionHistory_Organizations_OrganizationID",
                table: "OrganizationToProjectTransactionHistory",
                column: "OrganizationID",
                principalTable: "Organizations",
                principalColumn: "OrganizationID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationToProjectTransactionHistory_Projects_ProjectID",
                table: "OrganizationToProjectTransactionHistory",
                column: "ProjectID",
                principalTable: "Projects",
                principalColumn: "ProjectID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserToOrganizationTransactionHistories_Organizations_OrganizationID",
                table: "UserToOrganizationTransactionHistories",
                column: "OrganizationID",
                principalTable: "Organizations",
                principalColumn: "OrganizationID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserToProjectTransactionHistories_Projects_ProjectID",
                table: "UserToProjectTransactionHistories",
                column: "ProjectID",
                principalTable: "Projects",
                principalColumn: "ProjectID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
