using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dynamics.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class removeUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unit",
                table: "OrganizationToProjectTransactionHistory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "OrganizationToProjectTransactionHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
