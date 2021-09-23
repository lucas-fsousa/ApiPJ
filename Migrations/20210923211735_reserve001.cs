using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiPJ.Migrations
{
    public partial class reserve001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Checkin",
                table: "TB_RESERVE",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Checkout",
                table: "TB_RESERVE",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Checkin",
                table: "TB_RESERVE");

            migrationBuilder.DropColumn(
                name: "Checkout",
                table: "TB_RESERVE");
        }
    }
}
