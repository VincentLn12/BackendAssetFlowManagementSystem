using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class asdasdasd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_assetSubItems_asset_id",
                table: "assetSubItems",
                column: "asset_id");

            migrationBuilder.AddForeignKey(
                name: "FK_assetSubItems_assetItems_asset_id",
                table: "assetSubItems",
                column: "asset_id",
                principalTable: "assetItems",
                principalColumn: "asset_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_assetSubItems_assetItems_asset_id",
                table: "assetSubItems");

            migrationBuilder.DropIndex(
                name: "IX_assetSubItems_asset_id",
                table: "assetSubItems");
        }
    }
}
