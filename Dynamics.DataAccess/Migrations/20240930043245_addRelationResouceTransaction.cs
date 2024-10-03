using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dynamics.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addRelationResouceTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrganizationToProjectTransactionHistory",
                keyColumn: "TransactionID",
                keyValue: new Guid("757b360b-c213-411b-ac33-6fcc67348ebd"));

            migrationBuilder.DeleteData(
                table: "OrganizationToProjectTransactionHistory",
                keyColumn: "TransactionID",
                keyValue: new Guid("84580c07-c689-450b-b38c-86c3a3e24119"));

            migrationBuilder.DeleteData(
                table: "UserToProjectTransactionHistories",
                keyColumn: "TransactionID",
                keyValue: new Guid("4c404bee-5e7e-4d71-9c28-4c2600fb3679"));

            migrationBuilder.DeleteData(
                table: "UserToProjectTransactionHistories",
                keyColumn: "TransactionID",
                keyValue: new Guid("7262dec9-3f2d-4712-ac7f-91e148755d3b"));

            migrationBuilder.DeleteData(
                table: "UserToProjectTransactionHistories",
                keyColumn: "TransactionID",
                keyValue: new Guid("a3423f75-6977-42a7-bd21-0455ea75d1d8"));

            migrationBuilder.DeleteData(
                table: "UserToProjectTransactionHistories",
                keyColumn: "TransactionID",
                keyValue: new Guid("af323e75-2977-44b7-bc31-0235fa85d1d5"));

            migrationBuilder.DeleteData(
                table: "UserToProjectTransactionHistories",
                keyColumn: "TransactionID",
                keyValue: new Guid("b9173cd8-347d-4b8a-a351-1234a6f70539"));

            migrationBuilder.DeleteData(
                table: "UserToProjectTransactionHistories",
                keyColumn: "TransactionID",
                keyValue: new Guid("c23b94a5-4219-4311-8153-56d9152341b3"));

            migrationBuilder.DeleteData(
                table: "UserToProjectTransactionHistories",
                keyColumn: "TransactionID",
                keyValue: new Guid("c600e72d-b1db-49f3-a811-fa1687d96ca9"));

            migrationBuilder.DeleteData(
                table: "UserToProjectTransactionHistories",
                keyColumn: "TransactionID",
                keyValue: new Guid("dd1fadda-5a48-4533-8c05-551f653cd39a"));

            migrationBuilder.DeleteData(
                table: "UserToProjectTransactionHistories",
                keyColumn: "TransactionID",
                keyValue: new Guid("e7815a35-4f19-4e78-9b85-b4623b120fc9"));

            migrationBuilder.DropColumn(
                name: "ResourceName",
                table: "UserToProjectTransactionHistories");

            migrationBuilder.DropColumn(
                name: "ResourceName",
                table: "OrganizationToProjectTransactionHistory");

            migrationBuilder.AddColumn<Guid>(
                name: "ResourceID",
                table: "UserToProjectTransactionHistories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ResourceID",
                table: "OrganizationToProjectTransactionHistory",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserToProjectTransactionHistories_ResourceID",
                table: "UserToProjectTransactionHistories",
                column: "ResourceID");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationToProjectTransactionHistory_ResourceID",
                table: "OrganizationToProjectTransactionHistory",
                column: "ResourceID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationToProjectTransactionHistory_ProjectResources_ResourceID",
                table: "OrganizationToProjectTransactionHistory",
                column: "ResourceID",
                principalTable: "ProjectResources",
                principalColumn: "ResourceID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserToProjectTransactionHistories_ProjectResources_ResourceID",
                table: "UserToProjectTransactionHistories",
                column: "ResourceID",
                principalTable: "ProjectResources",
                principalColumn: "ResourceID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationToProjectTransactionHistory_ProjectResources_ResourceID",
                table: "OrganizationToProjectTransactionHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_UserToProjectTransactionHistories_ProjectResources_ResourceID",
                table: "UserToProjectTransactionHistories");

            migrationBuilder.DropIndex(
                name: "IX_UserToProjectTransactionHistories_ResourceID",
                table: "UserToProjectTransactionHistories");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationToProjectTransactionHistory_ResourceID",
                table: "OrganizationToProjectTransactionHistory");

            migrationBuilder.DropColumn(
                name: "ResourceID",
                table: "UserToProjectTransactionHistories");

            migrationBuilder.DropColumn(
                name: "ResourceID",
                table: "OrganizationToProjectTransactionHistory");

            migrationBuilder.AddColumn<string>(
                name: "ResourceName",
                table: "UserToProjectTransactionHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResourceName",
                table: "OrganizationToProjectTransactionHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "OrganizationToProjectTransactionHistory",
                columns: new[] { "TransactionID", "Amount", "Message", "OrganizationID", "ProjectID", "ResourceName", "Status", "Time", "Unit" },
                values: new object[,]
                {
                    { new Guid("757b360b-c213-411b-ac33-6fcc67348ebd"), 18987000.0, "Gui toi project 1 nhe", new Guid("4641b799-7fba-4d20-a78a-4d68db162e98"), new Guid("2d3c8b76-4e52-4446-8a5a-25cfa18b12aa"), "Money", 0, new DateOnly(2024, 7, 10), "VND" },
                    { new Guid("84580c07-c689-450b-b38c-86c3a3e24119"), 1342000.0, "Gui toi project 1 nhe lan 2", new Guid("4641b799-7fba-4d20-a78a-4d68db162e98"), new Guid("2d3c8b76-4e52-4446-8a5a-25cfa18b12aa"), "Money", 0, new DateOnly(2024, 9, 10), "VND" }
                });

            migrationBuilder.InsertData(
                table: "UserToProjectTransactionHistories",
                columns: new[] { "TransactionID", "Amount", "Message", "ProjectID", "ResourceName", "Status", "Time", "Unit", "UserID" },
                values: new object[,]
                {
                    { new Guid("4c404bee-5e7e-4d71-9c28-4c2600fb3679"), 9802000.0, "Gui tien toi project 1", new Guid("2d3c8b76-4e52-4446-8a5a-25cfa18b12aa"), "Money", 0, "2024-09-21", "VND", new Guid("e4894112-58a3-4fe6-9222-8ad70816adfe") },
                    { new Guid("7262dec9-3f2d-4712-ac7f-91e148755d3b"), 127000.0, "Gui tien toi project 1 user 2", new Guid("2d3c8b76-4e52-4446-8a5a-25cfa18b12aa"), "Money", 0, "2024-09-23", "VND", new Guid("fbd9f087-d6d2-4a27-a763-f1951da04361") },
                    { new Guid("a3423f75-6977-42a7-bd21-0455ea75d1d8"), 50.0, "Donated blankets to project 1", new Guid("2d3c8b76-4e52-4446-8a5a-25cfa18b12aa"), "Blanket", 0, "2024-08-15", "Piece", new Guid("c3488db1-afe6-40ae-bebb-424c4acf97c1") },
                    { new Guid("af323e75-2977-44b7-bc31-0235fa85d1d5"), 100000.0, "Gui bento toi project 1", new Guid("2d3c8b76-4e52-4446-8a5a-25cfa18b12aa"), "Bento", 0, "2024-09-10", "Box", new Guid("fbd9f087-d6d2-4a27-a763-f1951da04361") },
                    { new Guid("b9173cd8-347d-4b8a-a351-1234a6f70539"), 100.0, "Sent water bottles to project 1", new Guid("2d3c8b76-4e52-4446-8a5a-25cfa18b12aa"), "Water Bottles", 0, "2024-08-20", "Liters", new Guid("fbd9f087-d6d2-4a27-a763-f1951da04361") },
                    { new Guid("c23b94a5-4219-4311-8153-56d9152341b3"), 150.0, "Donated pillows to project 1", new Guid("2d3c8b76-4e52-4446-8a5a-25cfa18b12aa"), "Pillows", 0, "2024-08-05", "Piece", new Guid("c3488db1-afe6-40ae-bebb-424c4acf97c1") },
                    { new Guid("c600e72d-b1db-49f3-a811-fa1687d96ca9"), 1880000.0, "Gui tien toi project 1", new Guid("2d3c8b76-4e52-4446-8a5a-25cfa18b12aa"), "Money", 0, "2024-09-10", "VND", new Guid("e4894112-58a3-4fe6-9222-8ad70816adfe") },
                    { new Guid("dd1fadda-5a48-4533-8c05-551f653cd39a"), 100.0, "Gui coat toi project 1", new Guid("2d3c8b76-4e52-4446-8a5a-25cfa18b12aa"), "Coat", 0, "2024-07-22", "M", new Guid("fbd9f087-d6d2-4a27-a763-f1951da04361") },
                    { new Guid("e7815a35-4f19-4e78-9b85-b4623b120fc9"), 200.0, "Provided T-shirts to project 1", new Guid("2d3c8b76-4e52-4446-8a5a-25cfa18b12aa"), "T-shirt", 0, "2024-09-01", "Piece", new Guid("e4894112-58a3-4fe6-9222-8ad70816adfe") }
                });
        }
    }
}
