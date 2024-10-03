using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dynamics.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class editDB2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResourceName",
                table: "UserToProjectTransactionHistories");

            migrationBuilder.DropColumn(
                name: "ResourceName",
                table: "OrganizationToProjectTransactionHistory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResourceName",
                table: "UserToProjectTransactionHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResourceName",
                table: "OrganizationToProjectTransactionHistory",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
