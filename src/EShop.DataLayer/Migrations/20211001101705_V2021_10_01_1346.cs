using Microsoft.EntityFrameworkCore.Migrations;

namespace EShop.DataLayer.Migrations
{
    public partial class V2021_10_01_1346 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsProductTags_Products_ProductId",
                table: "ProductsProductTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsProductTags_ProductTags_ProductTagId",
                table: "ProductsProductTags");

            migrationBuilder.RenameColumn(
                name: "ProductTagId",
                table: "ProductsProductTags",
                newName: "ProductsId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductsProductTags",
                newName: "ProductTagsId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsProductTags_ProductTagId",
                table: "ProductsProductTags",
                newName: "IX_ProductsProductTags_ProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsProductTags_Products_ProductsId",
                table: "ProductsProductTags",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsProductTags_ProductTags_ProductTagsId",
                table: "ProductsProductTags",
                column: "ProductTagsId",
                principalTable: "ProductTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsProductTags_Products_ProductsId",
                table: "ProductsProductTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsProductTags_ProductTags_ProductTagsId",
                table: "ProductsProductTags");

            migrationBuilder.RenameColumn(
                name: "ProductsId",
                table: "ProductsProductTags",
                newName: "ProductTagId");

            migrationBuilder.RenameColumn(
                name: "ProductTagsId",
                table: "ProductsProductTags",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsProductTags_ProductsId",
                table: "ProductsProductTags",
                newName: "IX_ProductsProductTags_ProductTagId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsProductTags_Products_ProductId",
                table: "ProductsProductTags",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsProductTags_ProductTags_ProductTagId",
                table: "ProductsProductTags",
                column: "ProductTagId",
                principalTable: "ProductTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
