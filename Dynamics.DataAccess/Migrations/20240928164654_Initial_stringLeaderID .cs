using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dynamics.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial_stringLeaderID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResourceName",
                table: "UserToProjectTransactionHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.AddColumn<string>(
                name: "requestTitle",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "LeaderID",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectAddress",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectEmail",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectPhone",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ProjectMembers",
                type: "int",
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

            migrationBuilder.AddColumn<string>(
                name: "OrganizationAddress",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationEmail",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationPhoneNumber",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "OrganizationMember",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResourceName",
                table: "UserToProjectTransactionHistories");

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
                name: "requestTitle",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ProjectAddress",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectEmail",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectPhone",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ProjectMembers");

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

            migrationBuilder.AlterColumn<int>(
                name: "LeaderID",
                table: "Projects",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "StartTime",
                table: "Organizations",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
