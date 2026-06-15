using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WareHouse_Optimization_System.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintsToVendor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Vendors",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Vendors",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Vendors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_Email",
                table: "Vendors",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_PhoneNumber",
                table: "Vendors",
                column: "PhoneNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vendors_Email",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_PhoneNumber",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Vendors");

            migrationBuilder.AlterColumn<double>(
                name: "PhoneNumber",
                table: "Vendors",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
