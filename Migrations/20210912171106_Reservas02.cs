using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiPJ.Migrations
{
    public partial class Reservas02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_BLACKOUTDATE");

            migrationBuilder.AddColumn<DateTime>(
                name: "FinalDate",
                table: "TB_RESERVE",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "InitialDate",
                table: "TB_RESERVE",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalDate",
                table: "TB_RESERVE");

            migrationBuilder.DropColumn(
                name: "InitialDate",
                table: "TB_RESERVE");

            migrationBuilder.CreateTable(
                name: "TB_BLACKOUTDATE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApartmentId = table.Column<int>(type: "int", nullable: false),
                    FinalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InitialDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_BLACKOUTDATE", x => x.Id);
                });
        }
    }
}
