using Microsoft.EntityFrameworkCore.Migrations;

namespace MangaCMS.Migrations
{
    public partial class UpdateFileModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Files",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "posterName",
                table: "Files",
                newName: "filePath");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Files",
                newName: "fileName");

            migrationBuilder.AlterColumn<double>(
                name: "ChapterNumber",
                table: "Chapters",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Files",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "filePath",
                table: "Files",
                newName: "posterName");

            migrationBuilder.RenameColumn(
                name: "fileName",
                table: "Files",
                newName: "Path");

            migrationBuilder.AlterColumn<int>(
                name: "ChapterNumber",
                table: "Chapters",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }
    }
}
