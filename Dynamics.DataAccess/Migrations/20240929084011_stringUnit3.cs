using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dynamics.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class stringUnit3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentTransaction",
                table: "OrganizationResources");

            migrationBuilder.DropColumn(
                name: "ExpectedQuantity",
                table: "OrganizationResources");

            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "OrganizationResources",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Unit",
                table: "OrganizationResources",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ContentTransaction",
                table: "OrganizationResources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExpectedQuantity",
                table: "OrganizationResources",
                type: "int",
                nullable: true);
        }
    }
}
