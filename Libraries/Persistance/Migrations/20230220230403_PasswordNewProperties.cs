using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    public partial class PasswordNewProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Passwords");

            migrationBuilder.AddColumn<bool>(
                name: "Favorite",
                table: "Passwords",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "IDT",
                table: "Passwords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Passwords",
                type: "nvarchar(max)",
                maxLength: 100000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UDT",
                table: "Passwords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "URL",
                table: "Passwords",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Favorite",
                table: "Passwords");

            migrationBuilder.DropColumn(
                name: "IDT",
                table: "Passwords");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Passwords");

            migrationBuilder.DropColumn(
                name: "UDT",
                table: "Passwords");

            migrationBuilder.DropColumn(
                name: "URL",
                table: "Passwords");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Passwords",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);
        }
    }
}
