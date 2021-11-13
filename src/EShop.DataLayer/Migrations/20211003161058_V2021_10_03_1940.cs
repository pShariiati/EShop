using Microsoft.EntityFrameworkCore.Migrations;

namespace EShop.DataLayer.Migrations;

public partial class V2021_10_03_1940 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ProductProperties_Products_ProductId",
            table: "ProductProperties");

        migrationBuilder.AlterColumn<int>(
            name: "ProductId",
            table: "ProductProperties",
            type: "int",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);

        migrationBuilder.AddForeignKey(
            name: "FK_ProductProperties_Products_ProductId",
            table: "ProductProperties",
            column: "ProductId",
            principalTable: "Products",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ProductProperties_Products_ProductId",
            table: "ProductProperties");

        migrationBuilder.AlterColumn<int>(
            name: "ProductId",
            table: "ProductProperties",
            type: "int",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AddForeignKey(
            name: "FK_ProductProperties_Products_ProductId",
            table: "ProductProperties",
            column: "ProductId",
            principalTable: "Products",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }
}
