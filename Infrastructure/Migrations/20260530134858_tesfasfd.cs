using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class tesfasfd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_assetItems_units_unit_id",
                table: "assetItems");

            migrationBuilder.DropIndex(
                name: "IX_assetItems_unit_id",
                table: "assetItems");

            migrationBuilder.DropColumn(
                name: "unit_id",
                table: "assetItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "unit_id",
                table: "assetItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_assetItems_unit_id",
                table: "assetItems",
                column: "unit_id");

            migrationBuilder.AddForeignKey(
                name: "FK_assetItems_units_unit_id",
                table: "assetItems",
                column: "unit_id",
                principalTable: "units",
                principalColumn: "unit_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
