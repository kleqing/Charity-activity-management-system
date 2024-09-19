using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dynamics.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addUserRoleId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Users",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Avatar", "Dob", "Email", "Name", "Phone", "RoleId" },
                values: new object[] { new Guid("f5f3b3b3-3b3b-4b3b-8b3b-3b3b3b3b3b3b"), "1234 Main St", "https://www.gravatar.com/avatar/205e460b479e2e5b48aec07710c08d50", new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "testing@gmail.com", "John Doe", "1234567890", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f5f3b3b3-3b3b-4b3b-8b3b-3b3b3b3b3b3b"));

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);
        }
    }
}
