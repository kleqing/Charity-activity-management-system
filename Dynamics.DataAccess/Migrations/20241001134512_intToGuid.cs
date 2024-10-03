using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dynamics.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class intToGuid : Migration
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
                name: "IX_OrganizationToProjectTransactionHistory_OrganizationID",
                table: "OrganizationToProjectTransactionHistory");

            migrationBuilder.DropColumn(
                name: "OrganizationID",
                table: "OrganizationToProjectTransactionHistory");

            migrationBuilder.DropColumn(
                name: "ResourceName",
                table: "OrganizationToProjectTransactionHistory");

            migrationBuilder.DropColumn(
                name: "ContentTransaction",
                table: "OrganizationResources");

            migrationBuilder.DropColumn(
                name: "ExpectedQuantity",
                table: "OrganizationResources");

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
                newName: "ResourceID");

            migrationBuilder.RenameColumn(
                name: "MoneyTransactionAmout",
                table: "UserToOrganizationTransactionHistories",
                newName: "Unit");

            migrationBuilder.RenameIndex(
                name: "IX_UserToOrganizationTransactionHistories_OrganizationID",
                table: "UserToOrganizationTransactionHistories",
                newName: "IX_UserToOrganizationTransactionHistories_ResourceID");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Requests",
                newName: "requestTitle");

            migrationBuilder.RenameColumn(
                name: "ProjectID",
                table: "OrganizationToProjectTransactionHistory",
                newName: "OrganizationResourceID");

            migrationBuilder.RenameIndex(
                name: "IX_OrganizationToProjectTransactionHistory_ProjectID",
                table: "OrganizationToProjectTransactionHistory",
                newName: "IX_OrganizationToProjectTransactionHistory_OrganizationResourceID");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Time",
                table: "UserToProjectTransactionHistories",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "UserToOrganizationTransactionHistories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "UserToOrganizationTransactionHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "UserToOrganizationTransactionHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddColumn<string>(
                name: "RequestAddress",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestEmail",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestPhoneNumber",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "LeaderID",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Attachment",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ProjectAddress",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectEmail",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectPhoneNumber",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShutdownReason",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ProjectMembers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "OrganizationToProjectTransactionHistory",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectResourceID",
                table: "OrganizationToProjectTransactionHistory",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "StartTime",
                table: "Organizations",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OrganizationPictures",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "OrganizationName",
                table: "Organizations",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "OrganizationAddress",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationEmail",
                table: "Organizations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationPhoneNumber",
                table: "Organizations",
                type: "nvarchar(max)",
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

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "OrganizationMember",
                type: "int",
                nullable: true);

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
                name: "IX_OrganizationToProjectTransactionHistory_ProjectResourceID",
                table: "OrganizationToProjectTransactionHistory",
                column: "ProjectResourceID");

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
                name: "IX_Users_UserEmail",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserFullName",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationToProjectTransactionHistory_ProjectResourceID",
                table: "OrganizationToProjectTransactionHistory");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_OrganizationEmail",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_OrganizationName",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "UserToOrganizationTransactionHistories");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "UserToOrganizationTransactionHistories");

            migrationBuilder.DropColumn(
                name: "RequestAddress",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "RequestEmail",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "RequestPhoneNumber",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ProjectAddress",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectEmail",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectPhoneNumber",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ShutdownReason",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ProjectMembers");

            migrationBuilder.DropColumn(
                name: "ProjectResourceID",
                table: "OrganizationToProjectTransactionHistory");

            migrationBuilder.DropColumn(
                name: "OrganizationAddress",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "OrganizationEmail",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "OrganizationPhoneNumber",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "OrganizationMember");

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
                name: "ResourceID",
                table: "UserToOrganizationTransactionHistories",
                newName: "OrganizationID");

            migrationBuilder.RenameIndex(
                name: "IX_UserToOrganizationTransactionHistories_ResourceID",
                table: "UserToOrganizationTransactionHistories",
                newName: "IX_UserToOrganizationTransactionHistories_OrganizationID");

            migrationBuilder.RenameColumn(
                name: "requestTitle",
                table: "Requests",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "OrganizationResourceID",
                table: "OrganizationToProjectTransactionHistory",
                newName: "ProjectID");

            migrationBuilder.RenameIndex(
                name: "IX_OrganizationToProjectTransactionHistory_OrganizationResourceID",
                table: "OrganizationToProjectTransactionHistory",
                newName: "IX_OrganizationToProjectTransactionHistory_ProjectID");

            migrationBuilder.AlterColumn<string>(
                name: "Time",
                table: "UserToProjectTransactionHistories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "UserToOrganizationTransactionHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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

            migrationBuilder.AlterColumn<Guid>(
                name: "LeaderID",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Attachment",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "OrganizationToProjectTransactionHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationID",
                table: "OrganizationToProjectTransactionHistory",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ResourceName",
                table: "OrganizationToProjectTransactionHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "StartTime",
                table: "Organizations",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "OrganizationPictures",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OrganizationName",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "OrganizationResources",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ContentTransaction",
                table: "OrganizationResources",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ExpectedQuantity",
                table: "OrganizationResources",
                type: "int",
                nullable: true);

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
