using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dynamics.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addReasonToProjectTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "OrganizationToProjectTransactionHistory",
                keyColumn: "TransactionID",
                keyValue: new Guid("757b360b-c213-411b-ac33-6fcc67348ebd"),
                column: "Status",
                value: 0);

            migrationBuilder.UpdateData(
                table: "OrganizationToProjectTransactionHistory",
                keyColumn: "TransactionID",
                keyValue: new Guid("84580c07-c689-450b-b38c-86c3a3e24119"),
                column: "Status",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "ProjectID",
                keyValue: new Guid("2d3c8b76-4e52-4446-8a5a-25cfa18b12aa"),
                column: "Reason",
                value: null);

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "ProjectID",
                keyValue: new Guid("bbe8d3dd-e15b-4151-b6ea-80dd44c2280f"),
                column: "Reason",
                value: null);

            migrationBuilder.UpdateData(
                table: "UserToProjectTransactionHistories",
                keyColumn: "TransactionID",
                keyValue: new Guid("4c404bee-5e7e-4d71-9c28-4c2600fb3679"),
                column: "Status",
                value: 0);

            migrationBuilder.UpdateData(
                table: "UserToProjectTransactionHistories",
                keyColumn: "TransactionID",
                keyValue: new Guid("7262dec9-3f2d-4712-ac7f-91e148755d3b"),
                column: "Status",
                value: 0);

            migrationBuilder.UpdateData(
                table: "UserToProjectTransactionHistories",
                keyColumn: "TransactionID",
                keyValue: new Guid("c600e72d-b1db-49f3-a811-fa1687d96ca9"),
                column: "Status",
                value: 0);

            migrationBuilder.UpdateData(
                table: "UserToProjectTransactionHistories",
                keyColumn: "TransactionID",
                keyValue: new Guid("dd1fadda-5a48-4533-8c05-551f653cd39a"),
                column: "Status",
                value: 0);

            migrationBuilder.UpdateData(
                table: "UserToProjectTransactionHistories",
                keyColumn: "TransactionID",
                keyValue: new Guid("e7815a35-4f19-4e78-9b85-b4623b120fc9"),
                column: "Status",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                table: "Projects");

            migrationBuilder.UpdateData(
                table: "OrganizationToProjectTransactionHistory",
                keyColumn: "TransactionID",
                keyValue: new Guid("757b360b-c213-411b-ac33-6fcc67348ebd"),
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "OrganizationToProjectTransactionHistory",
                keyColumn: "TransactionID",
                keyValue: new Guid("84580c07-c689-450b-b38c-86c3a3e24119"),
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "UserToProjectTransactionHistories",
                keyColumn: "TransactionID",
                keyValue: new Guid("4c404bee-5e7e-4d71-9c28-4c2600fb3679"),
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "UserToProjectTransactionHistories",
                keyColumn: "TransactionID",
                keyValue: new Guid("7262dec9-3f2d-4712-ac7f-91e148755d3b"),
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "UserToProjectTransactionHistories",
                keyColumn: "TransactionID",
                keyValue: new Guid("c600e72d-b1db-49f3-a811-fa1687d96ca9"),
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "UserToProjectTransactionHistories",
                keyColumn: "TransactionID",
                keyValue: new Guid("dd1fadda-5a48-4533-8c05-551f653cd39a"),
                column: "Status",
                value: 1);

            migrationBuilder.UpdateData(
                table: "UserToProjectTransactionHistories",
                keyColumn: "TransactionID",
                keyValue: new Guid("e7815a35-4f19-4e78-9b85-b4623b120fc9"),
                column: "Status",
                value: 1);
        }
    }
}
