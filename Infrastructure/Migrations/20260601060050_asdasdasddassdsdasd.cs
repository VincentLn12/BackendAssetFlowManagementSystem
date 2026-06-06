using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class asdasdasddassdsdasd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "acquisition_method_id",
                table: "assetItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_assetItems_acquisition_method_id",
                table: "assetItems",
                column: "acquisition_method_id");

            migrationBuilder.AddForeignKey(
                name: "FK_assetItems_acquisitionMethods_acquisition_method_id",
                table: "assetItems",
                column: "acquisition_method_id",
                principalTable: "acquisitionMethods",
                principalColumn: "acquisition_method_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_assetItems_acquisitionMethods_acquisition_method_id",
                table: "assetItems");

            migrationBuilder.DropIndex(
                name: "IX_assetItems_acquisition_method_id",
                table: "assetItems");

            migrationBuilder.DropColumn(
                name: "acquisition_method_id",
                table: "assetItems");
        }
    }
}
