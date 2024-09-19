using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    organizationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    organizationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    organizationDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    attachment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    startTime = table.Column<DateOnly>(type: "date", nullable: true),
                    shutdownDay = table.Column<DateOnly>(type: "date", nullable: true),
                    ceoID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.organizationID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dob = table.Column<DateOnly>(type: "date", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    roleID = table.Column<int>(type: "int", nullable: true),
                    avatar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userID);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationResources",
                columns: table => new
                {
                    resourceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    organizationID = table.Column<int>(type: "int", nullable: false),
                    resourceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: true),
                    unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    donator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contentTransaction = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationResources", x => x.resourceID);
                    table.ForeignKey(
                        name: "FK_OrganizationResources_Organizations_organizationID",
                        column: x => x.organizationID,
                        principalTable: "Organizations",
                        principalColumn: "organizationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationMember",
                columns: table => new
                {
                    userID = table.Column<int>(type: "int", nullable: false),
                    organizationID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationMember", x => new { x.organizationID, x.userID });
                    table.ForeignKey(
                        name: "FK_OrganizationMember_Organizations_organizationID",
                        column: x => x.organizationID,
                        principalTable: "Organizations",
                        principalColumn: "organizationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationMember_Users_userID",
                        column: x => x.userID,
                        principalTable: "Users",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    requestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userID = table.Column<int>(type: "int", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    creationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    attachment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isEmergency = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.requestID);
                    table.ForeignKey(
                        name: "FK_Requests_Users_userID",
                        column: x => x.userID,
                        principalTable: "Users",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserToOrganizationTransactionHistories",
                columns: table => new
                {
                    transactionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userID = table.Column<int>(type: "int", nullable: false),
                    organizationID = table.Column<int>(type: "int", nullable: false),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    time = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToOrganizationTransactionHistories", x => x.transactionID);
                    table.ForeignKey(
                        name: "FK_UserToOrganizationTransactionHistories_Organizations_organizationID",
                        column: x => x.organizationID,
                        principalTable: "Organizations",
                        principalColumn: "organizationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserToOrganizationTransactionHistories_Users_userID",
                        column: x => x.userID,
                        principalTable: "Users",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    projectID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    organizationID = table.Column<int>(type: "int", nullable: false),
                    requestID = table.Column<int>(type: "int", nullable: true),
                    projectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    attachment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    projectDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    startTime = table.Column<DateOnly>(type: "date", nullable: true),
                    endTime = table.Column<DateOnly>(type: "date", nullable: true),
                    leaderID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.projectID);
                    table.ForeignKey(
                        name: "FK_Projects_Organizations_organizationID",
                        column: x => x.organizationID,
                        principalTable: "Organizations",
                        principalColumn: "organizationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_Requests_requestID",
                        column: x => x.requestID,
                        principalTable: "Requests",
                        principalColumn: "requestID");
                });

            migrationBuilder.CreateTable(
                name: "OrganizationToProjectTransactionHistory",
                columns: table => new
                {
                    transactionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    organizationID = table.Column<int>(type: "int", nullable: false),
                    projectID = table.Column<int>(type: "int", nullable: false),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    time = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationToProjectTransactionHistory", x => x.transactionID);
                    table.ForeignKey(
                        name: "FK_OrganizationToProjectTransactionHistory_Organizations_organizationID",
                        column: x => x.organizationID,
                        principalTable: "Organizations",
                        principalColumn: "organizationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationToProjectTransactionHistory_Projects_projectID",
                        column: x => x.projectID,
                        principalTable: "Projects",
                        principalColumn: "projectID");
                });

            migrationBuilder.CreateTable(
                name: "ProjectMembers",
                columns: table => new
                {
                    userID = table.Column<int>(type: "int", nullable: false),
                    projectID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMembers", x => new { x.projectID, x.userID });
                    table.ForeignKey(
                        name: "FK_ProjectMembers_Projects_projectID",
                        column: x => x.projectID,
                        principalTable: "Projects",
                        principalColumn: "projectID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectMembers_Users_userID",
                        column: x => x.userID,
                        principalTable: "Users",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectResources",
                columns: table => new
                {
                    resourceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    projectID = table.Column<int>(type: "int", nullable: false),
                    resourceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: true),
                    expectedQuantity = table.Column<int>(type: "int", nullable: true),
                    unit = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectResources", x => x.resourceID);
                    table.ForeignKey(
                        name: "FK_ProjectResources_Projects_projectID",
                        column: x => x.projectID,
                        principalTable: "Projects",
                        principalColumn: "projectID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserToProjectTransactionHistories",
                columns: table => new
                {
                    transactionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userID = table.Column<int>(type: "int", nullable: false),
                    projectID = table.Column<int>(type: "int", nullable: false),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    time = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToProjectTransactionHistories", x => x.transactionID);
                    table.ForeignKey(
                        name: "FK_UserToProjectTransactionHistories_Projects_projectID",
                        column: x => x.projectID,
                        principalTable: "Projects",
                        principalColumn: "projectID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserToProjectTransactionHistories_Users_userID",
                        column: x => x.userID,
                        principalTable: "Users",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationMember_userID",
                table: "OrganizationMember",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationResources_organizationID",
                table: "OrganizationResources",
                column: "organizationID");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationToProjectTransactionHistory_organizationID",
                table: "OrganizationToProjectTransactionHistory",
                column: "organizationID");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationToProjectTransactionHistory_projectID",
                table: "OrganizationToProjectTransactionHistory",
                column: "projectID");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_userID",
                table: "ProjectMembers",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectResources_projectID",
                table: "ProjectResources",
                column: "projectID");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_organizationID",
                table: "Projects",
                column: "organizationID");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_requestID",
                table: "Projects",
                column: "requestID",
                unique: true,
                filter: "[requestID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_userID",
                table: "Requests",
                column: "userID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserToOrganizationTransactionHistories_organizationID",
                table: "UserToOrganizationTransactionHistories",
                column: "organizationID");

            migrationBuilder.CreateIndex(
                name: "IX_UserToOrganizationTransactionHistories_userID",
                table: "UserToOrganizationTransactionHistories",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_UserToProjectTransactionHistories_projectID",
                table: "UserToProjectTransactionHistories",
                column: "projectID");

            migrationBuilder.CreateIndex(
                name: "IX_UserToProjectTransactionHistories_userID",
                table: "UserToProjectTransactionHistories",
                column: "userID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationMember");

            migrationBuilder.DropTable(
                name: "OrganizationResources");

            migrationBuilder.DropTable(
                name: "OrganizationToProjectTransactionHistory");

            migrationBuilder.DropTable(
                name: "ProjectMembers");

            migrationBuilder.DropTable(
                name: "ProjectResources");

            migrationBuilder.DropTable(
                name: "UserToOrganizationTransactionHistories");

            migrationBuilder.DropTable(
                name: "UserToProjectTransactionHistories");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
