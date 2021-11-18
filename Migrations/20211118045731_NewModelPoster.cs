using Microsoft.EntityFrameworkCore.Migrations;

namespace MangaCMS.Migrations
{
    public partial class NewModelPoster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mangas_Files_fileID",
                table: "Mangas");

            migrationBuilder.DropIndex(
                name: "IX_Mangas_fileID",
                table: "Mangas");

            migrationBuilder.DropColumn(
                name: "fileID",
                table: "Mangas");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Files",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MangaModelID",
                table: "Files",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "mangaID",
                table: "Files",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "posterName",
                table: "Files",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_MangaModelID",
                table: "Files",
                column: "MangaModelID");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Mangas_MangaModelID",
                table: "Files",
                column: "MangaModelID",
                principalTable: "Mangas",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Mangas_MangaModelID",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_MangaModelID",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "MangaModelID",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "mangaID",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "posterName",
                table: "Files");

            migrationBuilder.AddColumn<int>(
                name: "fileID",
                table: "Mangas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Mangas_fileID",
                table: "Mangas",
                column: "fileID");

            migrationBuilder.AddForeignKey(
                name: "FK_Mangas_Files_fileID",
                table: "Mangas",
                column: "fileID",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
