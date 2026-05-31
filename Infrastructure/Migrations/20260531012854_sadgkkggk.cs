using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class sadgkkggk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_assetItems_assetCategories_asset_category_id",
                table: "assetItems");

            migrationBuilder.DropIndex(
                name: "IX_assetItems_asset_category_id",
                table: "assetItems");

            migrationBuilder.DropColumn(
                name: "asset_category_id",
                table: "assetItems");

            migrationBuilder.DropColumn(
                name: "useful_life_year",
                table: "assetItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "asset_category_id",
                table: "assetItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "useful_life_year",
                table: "assetItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_assetItems_asset_category_id",
                table: "assetItems",
                column: "asset_category_id");

            migrationBuilder.AddForeignKey(
                name: "FK_assetItems_assetCategories_asset_category_id",
                table: "assetItems",
                column: "asset_category_id",
                principalTable: "assetCategories",
                principalColumn: "asset_category_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
