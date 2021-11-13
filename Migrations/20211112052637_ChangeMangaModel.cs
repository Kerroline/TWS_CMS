using Microsoft.EntityFrameworkCore.Migrations;

namespace MangaCMS.Migrations
{
    public partial class ChangeMangaModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RuName",
                table: "Mangas",
                newName: "RU_Name");

            migrationBuilder.RenameColumn(
                name: "Poster",
                table: "Mangas",
                newName: "PosterPath");

            migrationBuilder.RenameColumn(
                name: "OrigName",
                table: "Mangas",
                newName: "JP_Name");

            migrationBuilder.RenameColumn(
                name: "EngName",
                table: "Mangas",
                newName: "ENG_Name");

            migrationBuilder.AddColumn<string>(
                name: "ContentDirPath",
                table: "Mangas",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentDirPath",
                table: "Mangas");

            migrationBuilder.RenameColumn(
                name: "RU_Name",
                table: "Mangas",
                newName: "RuName");

            migrationBuilder.RenameColumn(
                name: "PosterPath",
                table: "Mangas",
                newName: "Poster");

            migrationBuilder.RenameColumn(
                name: "JP_Name",
                table: "Mangas",
                newName: "OrigName");

            migrationBuilder.RenameColumn(
                name: "ENG_Name",
                table: "Mangas",
                newName: "EngName");
        }
    }
}
