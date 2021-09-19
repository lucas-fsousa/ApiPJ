using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiPJ.Migrations
{
    public partial class baseatt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "TB_RESERVE",
                newName: "IdReserve");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "TB_IMAGEPATH",
                newName: "IdImgPath");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "TB_APARTMENT",
                newName: "IdAp");

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "TB_IMAGEPATH",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_TB_IMAGEPATH_Path",
                table: "TB_IMAGEPATH",
                column: "Path",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TB_IMAGEPATH_Path",
                table: "TB_IMAGEPATH");

            migrationBuilder.RenameColumn(
                name: "IdReserve",
                table: "TB_RESERVE",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "IdImgPath",
                table: "TB_IMAGEPATH",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "IdAp",
                table: "TB_APARTMENT",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "TB_IMAGEPATH",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
