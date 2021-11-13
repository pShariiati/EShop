using Microsoft.EntityFrameworkCore.Migrations;

namespace EShop.DataLayer.Migrations;

public partial class V2021_09_26_1642 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Address",
            table: "Carts",
            type: "nvarchar(300)",
            maxLength: 300,
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Address",
            table: "Carts");
    }
}
