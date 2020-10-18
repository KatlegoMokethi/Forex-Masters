using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ForexMasters_site.Migrations
{
    public partial class ModifyAttributeType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "PictureFile",
                table: "Flashcard",
                nullable: true,
                oldClrType: typeof(byte[]));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "PictureFile",
                table: "Flashcard",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldNullable: true);
        }
    }
}
