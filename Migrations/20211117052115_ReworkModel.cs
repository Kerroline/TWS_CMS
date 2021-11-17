using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MangaCMS.Migrations
{
    public partial class ReworkModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mangas_Statuses_StatusId",
                table: "Mangas");

            migrationBuilder.DropColumn(
                name: "ContentDirPath",
                table: "Mangas");

            migrationBuilder.DropColumn(
                name: "PosterPath",
                table: "Mangas");

            migrationBuilder.RenameColumn(
                name: "Year",
                table: "Mangas",
                newName: "year");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "Mangas",
                newName: "statusID");

            migrationBuilder.RenameColumn(
                name: "Link",
                table: "Mangas",
                newName: "link");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Mangas",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Author",
                table: "Mangas",
                newName: "author");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Mangas",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "RU_Name",
                table: "Mangas",
                newName: "russianName");

            migrationBuilder.RenameColumn(
                name: "JP_Name",
                table: "Mangas",
                newName: "japanName");

            migrationBuilder.RenameColumn(
                name: "ENG_Name",
                table: "Mangas",
                newName: "englishName");

            migrationBuilder.RenameIndex(
                name: "IX_Mangas_StatusId",
                table: "Mangas",
                newName: "IX_Mangas_statusID");

            migrationBuilder.AddColumn<int>(
                name: "fileID",
                table: "Mangas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Path = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PageNumber = table.Column<int>(type: "integer", nullable: false),
                    FileId = table.Column<int>(type: "integer", nullable: false),
                    ChapterId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pages_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pages_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "StatusName" },
                values: new object[] { 1, "Настройка проекта" });

            migrationBuilder.CreateIndex(
                name: "IX_Mangas_fileID",
                table: "Mangas",
                column: "fileID");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_ChapterId",
                table: "Pages",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_FileId",
                table: "Pages",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mangas_Files_fileID",
                table: "Mangas",
                column: "fileID",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mangas_Statuses_statusID",
                table: "Mangas",
                column: "statusID",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mangas_Files_fileID",
                table: "Mangas");

            migrationBuilder.DropForeignKey(
                name: "FK_Mangas_Statuses_statusID",
                table: "Mangas");

            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Mangas_fileID",
                table: "Mangas");

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "fileID",
                table: "Mangas");

            migrationBuilder.RenameColumn(
                name: "year",
                table: "Mangas",
                newName: "Year");

            migrationBuilder.RenameColumn(
                name: "statusID",
                table: "Mangas",
                newName: "StatusId");

            migrationBuilder.RenameColumn(
                name: "link",
                table: "Mangas",
                newName: "Link");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Mangas",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "author",
                table: "Mangas",
                newName: "Author");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Mangas",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "russianName",
                table: "Mangas",
                newName: "RU_Name");

            migrationBuilder.RenameColumn(
                name: "japanName",
                table: "Mangas",
                newName: "JP_Name");

            migrationBuilder.RenameColumn(
                name: "englishName",
                table: "Mangas",
                newName: "ENG_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Mangas_statusID",
                table: "Mangas",
                newName: "IX_Mangas_StatusId");

            migrationBuilder.AddColumn<string>(
                name: "ContentDirPath",
                table: "Mangas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PosterPath",
                table: "Mangas",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Mangas_Statuses_StatusId",
                table: "Mangas",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
