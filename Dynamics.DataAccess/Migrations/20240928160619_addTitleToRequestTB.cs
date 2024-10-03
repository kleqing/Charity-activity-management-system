using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dynamics.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addTitleToRequestTB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "OrganizationToProjectTransactionHistory",
                keyColumn: "TransactionID",
                keyValue: new Guid("757b360b-c213-411b-ac33-6fcc67348ebd"),
                column: "ResourceName",
                value: "Money");

            migrationBuilder.UpdateData(
                table: "OrganizationToProjectTransactionHistory",
                keyColumn: "TransactionID",
                keyValue: new Guid("84580c07-c689-450b-b38c-86c3a3e24119"),
                column: "ResourceName",
                value: "Money");

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "RequestID",
                keyValue: new Guid("4398bfb2-5a6d-4daf-a8be-52ef92f455a9"),
                columns: new[] { "Content", "Title" },
                values: new object[] { "Content of request 2", "Request 2" });

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "RequestID",
                keyValue: new Guid("ad111cb1-39d5-42e2-a2f5-e943898e59e6"),
                columns: new[] { "Content", "Title" },
                values: new object[] { "Content of request 1", "Request 1" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Requests");

            migrationBuilder.UpdateData(
                table: "OrganizationToProjectTransactionHistory",
                keyColumn: "TransactionID",
                keyValue: new Guid("757b360b-c213-411b-ac33-6fcc67348ebd"),
                column: "ResourceName",
                value: null);

            migrationBuilder.UpdateData(
                table: "OrganizationToProjectTransactionHistory",
                keyColumn: "TransactionID",
                keyValue: new Guid("84580c07-c689-450b-b38c-86c3a3e24119"),
                column: "ResourceName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "RequestID",
                keyValue: new Guid("4398bfb2-5a6d-4daf-a8be-52ef92f455a9"),
                column: "Content",
                value: "Request 2");

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "RequestID",
                keyValue: new Guid("ad111cb1-39d5-42e2-a2f5-e943898e59e6"),
                column: "Content",
                value: "Request 1");
        }
    }
}
