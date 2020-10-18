using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ForexMasters_site.Migrations
{
    public partial class RemoveUselessFieldOnFlashcard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureFile",
                table: "Flashcard");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Flashcard",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Flashcard",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PictureFile",
                table: "Flashcard",
                nullable: true);
        }
    }
}
