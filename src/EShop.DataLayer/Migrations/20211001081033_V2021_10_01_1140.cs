using Microsoft.EntityFrameworkCore.Migrations;

namespace EShop.DataLayer.Migrations;

public partial class V2021_10_01_1140 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ProductProductTag");

        migrationBuilder.CreateTable(
            name: "ProductsProductTags",
            columns: table => new
            {
                ProductId = table.Column<int>(type: "int", nullable: false),
                ProductTagId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductsProductTags", x => new { x.ProductId, x.ProductTagId });
                table.ForeignKey(
                    name: "FK_ProductsProductTags_Products_ProductId",
                    column: x => x.ProductId,
                    principalTable: "Products",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ProductsProductTags_ProductTags_ProductTagId",
                    column: x => x.ProductTagId,
                    principalTable: "ProductTags",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ProductsProductTags_ProductTagId",
            table: "ProductsProductTags",
            column: "ProductTagId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ProductsProductTags");

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
}
