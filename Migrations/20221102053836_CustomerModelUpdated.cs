using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApplicatioon.Migrations
{
    public partial class CustomerModelUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CustomerStagesId",
                table: "Customer",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Compaign",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Messages = table.Column<int>(type: "integer", nullable: false),
                    TotalCustomers = table.Column<int>(type: "integer", nullable: false),
                    HasEnded = table.Column<bool>(type: "boolean", nullable: false),
                    TotalClicks = table.Column<List<Guid>>(type: "uuid[]", nullable: true),
                    DeliveredEmail = table.Column<List<Guid>>(type: "uuid[]", nullable: true),
                    ClickThroughRate = table.Column<double>(type: "double precision", nullable: false),
                    ClickToOpenRate = table.Column<double>(type: "double precision", nullable: false),
                    ComplaintRate = table.Column<double>(type: "double precision", nullable: false),
                    ConversionRate = table.Column<double>(type: "double precision", nullable: false),
                    OpenRates = table.Column<double>(type: "double precision", nullable: false),
                    UnsubscribeRate = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compaign", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyName = table.Column<string>(type: "text", nullable: true),
                    CompanyLogo = table.Column<string>(type: "text", nullable: true),
                    RegistrationNumber = table.Column<string>(type: "text", nullable: true),
                    ManagerId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Company_AspNetUsers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerStage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerStage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerStage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    CompanyModelId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_AspNetUsers_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Employee_Company_CompanyModelId",
                        column: x => x.CompanyModelId,
                        principalTable: "Company",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Profit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AmountMade = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompanyModelId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profit_Company_CompanyModelId",
                        column: x => x.CompanyModelId,
                        principalTable: "Company",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CustomerStagesId",
                table: "Customer",
                column: "CustomerStagesId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_ManagerId",
                table: "Company",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_CompanyModelId",
                table: "Employee",
                column: "CompanyModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_EmployeeId",
                table: "Employee",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Profit_CompanyModelId",
                table: "Profit",
                column: "CompanyModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_CustomerStage_CustomerStagesId",
                table: "Customer",
                column: "CustomerStagesId",
                principalTable: "CustomerStage",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_CustomerStage_CustomerStagesId",
                table: "Customer");

            migrationBuilder.DropTable(
                name: "Compaign");

            migrationBuilder.DropTable(
                name: "CustomerStage");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Profit");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropIndex(
                name: "IX_Customer_CustomerStagesId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CustomerStagesId",
                table: "Customer");
        }
    }
}
