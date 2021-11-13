using Microsoft.EntityFrameworkCore.Migrations;

namespace EShop.DataLayer.Migrations;

public partial class V2021_10_01_0804 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "ProductTags",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductTags", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ProductProductTag",
            columns: table => new
            {
                ProductTagsId = table.Column<int>(type: "int", nullable: false),
                ProductsId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductProductTag", x => new { x.ProductTagsId, x.ProductsId });
                table.ForeignKey(
                    name: "FK_ProductProductTag_Products_ProductsId",
                    column: x => x.ProductsId,
                    principalTable: "Products",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ProductProductTag_ProductTags_ProductTagsId",
                    column: x => x.ProductTagsId,
                    principalTable: "ProductTags",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ProductProductTag_ProductsId",
            table: "ProductProductTag",
            column: "ProductsId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ProductProductTag");

        migrationBuilder.DropTable(
            name: "ProductTags");
    }
}
