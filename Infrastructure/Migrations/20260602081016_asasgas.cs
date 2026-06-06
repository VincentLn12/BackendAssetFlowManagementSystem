using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class asasgas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_assetItems_staffs_staff_id",
                table: "assetItems");

            migrationBuilder.DropForeignKey(
                name: "FK_assetItems_vendors_vendor_id",
                table: "assetItems");

            migrationBuilder.DropIndex(
                name: "IX_assetItems_staff_id",
                table: "assetItems");

            migrationBuilder.DropIndex(
                name: "IX_assetItems_vendor_id",
                table: "assetItems");

            migrationBuilder.DropColumn(
                name: "staff_id",
                table: "assetItems");

            migrationBuilder.DropColumn(
                name: "vendor_id",
                table: "assetItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "staff_id",
                table: "assetItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "vendor_id",
                table: "assetItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_assetItems_staff_id",
                table: "assetItems",
                column: "staff_id");

            migrationBuilder.CreateIndex(
                name: "IX_assetItems_vendor_id",
                table: "assetItems",
                column: "vendor_id");

            migrationBuilder.AddForeignKey(
                name: "FK_assetItems_staffs_staff_id",
                table: "assetItems",
                column: "staff_id",
                principalTable: "staffs",
                principalColumn: "staff_id");

            migrationBuilder.AddForeignKey(
                name: "FK_assetItems_vendors_vendor_id",
                table: "assetItems",
                column: "vendor_id",
                principalTable: "vendors",
                principalColumn: "vendor_id");
        }
    }
}
