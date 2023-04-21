using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Migrations
{
    public partial class newColumnsDDTForSync : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DDT",
                table: "Settings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Settings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DDT",
                table: "Passwords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Passwords",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Synced",
                table: "Passwords",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DDT",
                table: "AppUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "AppUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DDT",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "DDT",
                table: "Passwords");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Passwords");

            migrationBuilder.DropColumn(
                name: "Synced",
                table: "Passwords");

            migrationBuilder.DropColumn(
                name: "DDT",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "AppUsers");
        }
    }
}
