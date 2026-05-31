using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class procumentd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "project_code",
                table: "procurement_records");

            migrationBuilder.AddColumn<int>(
                name: "project_id",
                table: "procurement_records",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_procurement_records_project_id",
                table: "procurement_records",
                column: "project_id");

            migrationBuilder.AddForeignKey(
                name: "FK_procurement_records_projects_project_id",
                table: "procurement_records",
                column: "project_id",
                principalTable: "projects",
                principalColumn: "project_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_procurement_records_projects_project_id",
                table: "procurement_records");

            migrationBuilder.DropIndex(
                name: "IX_procurement_records_project_id",
                table: "procurement_records");

            migrationBuilder.DropColumn(
                name: "project_id",
                table: "procurement_records");

            migrationBuilder.AddColumn<string>(
                name: "project_code",
                table: "procurement_records",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
