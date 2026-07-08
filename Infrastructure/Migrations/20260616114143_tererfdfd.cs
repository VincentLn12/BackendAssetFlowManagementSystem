using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class tererfdfd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "fiscal_year_id",
                table: "materialIssueDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_materialIssueDetails_fiscal_year_id",
                table: "materialIssueDetails",
                column: "fiscal_year_id");

            migrationBuilder.AddForeignKey(
                name: "FK_materialIssueDetails_fiscal_years_fiscal_year_id",
                table: "materialIssueDetails",
                column: "fiscal_year_id",
                principalTable: "fiscal_years",
                principalColumn: "fiscal_year_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_materialIssueDetails_fiscal_years_fiscal_year_id",
                table: "materialIssueDetails");

            migrationBuilder.DropIndex(
                name: "IX_materialIssueDetails_fiscal_year_id",
                table: "materialIssueDetails");

            migrationBuilder.DropColumn(
                name: "fiscal_year_id",
                table: "materialIssueDetails");
        }
    }
}
