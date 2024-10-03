using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dynamics.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class editDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Users_CEOID",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_LeaderID",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_LeaderID",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_CEOID",
                table: "Organizations");

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "ProjectID",
                keyValue: new Guid("2d3c8b76-4e52-4446-8a5a-25cfa18b12aa"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "ProjectID",
                keyValue: new Guid("bbe8d3dd-e15b-4151-b6ea-80dd44c2280f"));

            migrationBuilder.DropColumn(
                name: "LeaderID",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CEOID",
                table: "Organizations");

            migrationBuilder.AddColumn<string>(
                name: "ResourceName",
                table: "UserToProjectTransactionHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportFile",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "ProjectResources",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResourceName",
                table: "OrganizationToProjectTransactionHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "OrganizationID",
                keyValue: new Guid("4641b799-7fba-4d20-a78a-4d68db162e98"),
                columns: new[] { "Address", "Email", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "OrganizationID",
                keyValue: new Guid("c2f983db-d9bb-4214-ae40-eab93cc2de72"),
                columns: new[] { "Address", "Email", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "RequestID",
                keyValue: new Guid("4398bfb2-5a6d-4daf-a8be-52ef92f455a9"),
                columns: new[] { "Address", "Email", "PhoneNumber" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "RequestID",
                keyValue: new Guid("ad111cb1-39d5-42e2-a2f5-e943898e59e6"),
                columns: new[] { "Address", "Email", "PhoneNumber" },
                values: new object[] { null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResourceName",
                table: "UserToProjectTransactionHistories");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ReportFile",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ResourceName",
                table: "OrganizationToProjectTransactionHistory");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Organizations");

            migrationBuilder.AddColumn<Guid>(
                name: "LeaderID",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "ProjectResources",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "CEOID",
                table: "Organizations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "OrganizationID",
                keyValue: new Guid("4641b799-7fba-4d20-a78a-4d68db162e98"),
                column: "CEOID",
                value: null);

            migrationBuilder.UpdateData(
                table: "Organizations",
                keyColumn: "OrganizationID",
                keyValue: new Guid("c2f983db-d9bb-4214-ae40-eab93cc2de72"),
                column: "CEOID",
                value: null);

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "ProjectID", "Attachment", "EndTime", "LeaderID", "OrganizationID", "ProjectDescription", "ProjectName", "ProjectStatus", "Reason", "RequestID", "StartTime" },
                values: new object[,]
                {
                    { new Guid("2d3c8b76-4e52-4446-8a5a-25cfa18b12aa"), null, null, new Guid("e4894112-58a3-4fe6-9222-8ad70816adfe"), new Guid("4641b799-7fba-4d20-a78a-4d68db162e98"), "Project ds dg dg dfgdg ", "Project 1", 1, null, new Guid("ad111cb1-39d5-42e2-a2f5-e943898e59e6"), null },
                    { new Guid("bbe8d3dd-e15b-4151-b6ea-80dd44c2280f"), null, null, new Guid("e4894112-58a3-4fe6-9222-8ad70816adfe"), new Guid("c2f983db-d9bb-4214-ae40-eab93cc2de72"), "Project sdfsdf dg dg dg s", "Project 2", 1, null, new Guid("4398bfb2-5a6d-4daf-a8be-52ef92f455a9"), null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_LeaderID",
                table: "Projects",
                column: "LeaderID");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_CEOID",
                table: "Organizations",
                column: "CEOID");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Users_CEOID",
                table: "Organizations",
                column: "CEOID",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_LeaderID",
                table: "Projects",
                column: "LeaderID",
                principalTable: "Users",
                principalColumn: "UserID");
        }
    }
}
