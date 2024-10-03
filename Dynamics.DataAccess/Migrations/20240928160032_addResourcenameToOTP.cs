using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dynamics.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addResourcenameToOTP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResourceName",
                table: "OrganizationToProjectTransactionHistory",
                type: "nvarchar(max)",
                nullable: true);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResourceName",
                table: "OrganizationToProjectTransactionHistory");
        }
    }
}
