using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WareHouse_Optimization_System.Migrations
{
    /// <inheritdoc />
    public partial class AddVendorToStock_ManualFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VendorId",
                table: "StockItems",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockItems_VendorId",
                table: "StockItems",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockItems_Vendors_VendorId",
                table: "StockItems",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "VendorId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockItems_Vendors_VendorId",
                table: "StockItems");

            migrationBuilder.DropIndex(
                name: "IX_StockItems_VendorId",
                table: "StockItems");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "StockItems");
        }

    }
}
