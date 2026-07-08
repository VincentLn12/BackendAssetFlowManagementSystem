using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class staffnamesssd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "fiscal_year_id",
                table: "materialStockCards",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_materialStockCards_fiscal_year_id",
                table: "materialStockCards",
                column: "fiscal_year_id");

            migrationBuilder.AddForeignKey(
                name: "FK_materialStockCards_fiscal_years_fiscal_year_id",
                table: "materialStockCards",
                column: "fiscal_year_id",
                principalTable: "fiscal_years",
                principalColumn: "fiscal_year_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_materialStockCards_fiscal_years_fiscal_year_id",
                table: "materialStockCards");

            migrationBuilder.DropIndex(
                name: "IX_materialStockCards_fiscal_year_id",
                table: "materialStockCards");

            migrationBuilder.DropColumn(
                name: "fiscal_year_id",
                table: "materialStockCards");
        }
    }
}
