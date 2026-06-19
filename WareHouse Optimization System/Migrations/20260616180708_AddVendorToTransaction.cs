using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WareHouse_Optimization_System.Migrations
{
    /// <inheritdoc />
    public partial class AddVendorToTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VendorId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_VendorId",
                table: "Transactions",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Vendors_VendorId",
                table: "Transactions",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "VendorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Vendors_VendorId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_VendorId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "Transactions");
        }
    }
}
