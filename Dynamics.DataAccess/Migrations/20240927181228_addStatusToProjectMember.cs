using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dynamics.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addStatusToProjectMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProjectMembers",
                keyColumns: new[] { "ProjectID", "UserID" },
                keyValues: new object[] { new Guid("2d3c8b76-4e52-4446-8a5a-25cfa18b12aa"), new Guid("c3488db1-afe6-40ae-bebb-424c4acf97c1") });

            migrationBuilder.DeleteData(
                table: "ProjectMembers",
                keyColumns: new[] { "ProjectID", "UserID" },
                keyValues: new object[] { new Guid("2d3c8b76-4e52-4446-8a5a-25cfa18b12aa"), new Guid("e4894112-58a3-4fe6-9222-8ad70816adfe") });

            migrationBuilder.DeleteData(
                table: "ProjectMembers",
                keyColumns: new[] { "ProjectID", "UserID" },
                keyValues: new object[] { new Guid("2d3c8b76-4e52-4446-8a5a-25cfa18b12aa"), new Guid("fbd9f087-d6d2-4a27-a763-f1951da04361") });

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ProjectMembers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ProjectMembers");

            migrationBuilder.InsertData(
                table: "ProjectMembers",
                columns: new[] { "ProjectID", "UserID" },
                values: new object[,]
                {
                    { new Guid("2d3c8b76-4e52-4446-8a5a-25cfa18b12aa"), new Guid("c3488db1-afe6-40ae-bebb-424c4acf97c1") },
                    { new Guid("2d3c8b76-4e52-4446-8a5a-25cfa18b12aa"), new Guid("e4894112-58a3-4fe6-9222-8ad70816adfe") },
                    { new Guid("2d3c8b76-4e52-4446-8a5a-25cfa18b12aa"), new Guid("fbd9f087-d6d2-4a27-a763-f1951da04361") }
                });
        }
    }
}
