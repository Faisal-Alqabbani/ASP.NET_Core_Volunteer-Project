using Microsoft.EntityFrameworkCore.Migrations;

namespace CSharpProject.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lang",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "Lat",
                table: "Works");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Works",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Works",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Works",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Works");

            migrationBuilder.AddColumn<string>(
                name: "Lang",
                table: "Works",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lat",
                table: "Works",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
