﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiPJ.Migrations
{
    public partial class Init1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_ADRESS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PublicPlace = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ADRESS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_CUSTOMER",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    MaritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Rg = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Sex = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_CUSTOMER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_CUSTOMER_TB_ADRESS_Id",
                        column: x => x.Id,
                        principalTable: "TB_ADRESS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_EMPLOYEE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    AdmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DemissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WalletWorkId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FunctionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContractualSalary = table.Column<decimal>(type: "decimal(2,2)", precision: 2, nullable: false),
                    AcessLevel = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    MaritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Rg = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Sex = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_EMPLOYEE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_EMPLOYEE_TB_ADRESS_Id",
                        column: x => x.Id,
                        principalTable: "TB_ADRESS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_CUSTOMER_Cpf",
                table: "TB_CUSTOMER",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_EMPLOYEE_Cpf",
                table: "TB_EMPLOYEE",
                column: "Cpf",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_CUSTOMER");

            migrationBuilder.DropTable(
                name: "TB_EMPLOYEE");

            migrationBuilder.DropTable(
                name: "TB_ADRESS");
        }
    }
}
