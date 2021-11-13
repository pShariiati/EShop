using Microsoft.EntityFrameworkCore.Migrations;

namespace EShop.DataLayer.Migrations;

public partial class V2021_10_01_1030 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            name: "IX_ProductTags_Title",
            table: "ProductTags",
            column: "Title",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_ProductTags_Title",
            table: "ProductTags");
    }
}
