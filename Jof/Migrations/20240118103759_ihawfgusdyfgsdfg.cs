using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jof.Migrations
{
    public partial class ihawfgusdyfgsdfg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgUrl",
                table: "Fruits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgUrl",
                table: "Fruits");
        }
    }
}
