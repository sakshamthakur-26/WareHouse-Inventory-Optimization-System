using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vendor_Management.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Vendors",
                newName: "VendorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "Vendors",
                newName: "Id");
        }
    }
}
