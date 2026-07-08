using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class gkagkdf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "procurement_record_id",
                table: "materialIssueDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_materialIssueDetails_procurement_record_id",
                table: "materialIssueDetails",
                column: "procurement_record_id");

            migrationBuilder.AddForeignKey(
                name: "FK_materialIssueDetails_procurement_records_procurement_record_id",
                table: "materialIssueDetails",
                column: "procurement_record_id",
                principalTable: "procurement_records",
                principalColumn: "procurement_record_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_materialIssueDetails_procurement_records_procurement_record_id",
                table: "materialIssueDetails");

            migrationBuilder.DropIndex(
                name: "IX_materialIssueDetails_procurement_record_id",
                table: "materialIssueDetails");

            migrationBuilder.DropColumn(
                name: "procurement_record_id",
                table: "materialIssueDetails");
        }
    }
}
