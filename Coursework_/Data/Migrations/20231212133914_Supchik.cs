using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coursework_.Data.Migrations
{
    /// <inheritdoc />
    public partial class Supchik : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopItems_Products_productId",
                table: "ShopItems");

            migrationBuilder.RenameColumn(
                name: "productId",
                table: "ShopItems",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopItems_productId",
                table: "ShopItems",
                newName: "IX_ShopItems_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopItems_Products_ProductId",
                table: "ShopItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopItems_Products_ProductId",
                table: "ShopItems");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ShopItems",
                newName: "productId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopItems_ProductId",
                table: "ShopItems",
                newName: "IX_ShopItems_productId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopItems_Products_productId",
                table: "ShopItems",
                column: "productId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
