using Microsoft.EntityFrameworkCore.Migrations;

namespace EShop.DataLayer.Migrations
{
    public partial class V2021_10_01_1348 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsProductTags_Products_ProductsId",
                table: "ProductsProductTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsProductTags_ProductTags_ProductTagsId",
                table: "ProductsProductTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsProductTags",
                table: "ProductsProductTags");

            migrationBuilder.RenameTable(
                name: "ProductsProductTags",
                newName: "ProductProductTag");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsProductTags_ProductsId",
                table: "ProductProductTag",
                newName: "IX_ProductProductTag_ProductsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductProductTag",
                table: "ProductProductTag",
                columns: new[] { "ProductTagsId", "ProductsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductTag_Products_ProductsId",
                table: "ProductProductTag",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductTag_ProductTags_ProductTagsId",
                table: "ProductProductTag",
                column: "ProductTagsId",
                principalTable: "ProductTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductTag_Products_ProductsId",
                table: "ProductProductTag");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductTag_ProductTags_ProductTagsId",
                table: "ProductProductTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductProductTag",
                table: "ProductProductTag");

            migrationBuilder.RenameTable(
                name: "ProductProductTag",
                newName: "ProductsProductTags");

            migrationBuilder.RenameIndex(
                name: "IX_ProductProductTag_ProductsId",
                table: "ProductsProductTags",
                newName: "IX_ProductsProductTags_ProductsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsProductTags",
                table: "ProductsProductTags",
                columns: new[] { "ProductTagsId", "ProductsId" });

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
    }
}
