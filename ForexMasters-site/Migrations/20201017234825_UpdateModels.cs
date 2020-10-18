using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ForexMasters_site.Migrations
{
    public partial class UpdateModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureFile",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "User",
                newName: "PhoneNumber");

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "User",
                type: "money",
                nullable: false,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "User",
                newName: "Password");

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "User",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money");

            migrationBuilder.AddColumn<byte[]>(
                name: "PictureFile",
                table: "User",
                nullable: true);
        }
    }
}
