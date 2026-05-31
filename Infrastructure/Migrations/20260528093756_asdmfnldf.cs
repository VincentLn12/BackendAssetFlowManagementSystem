using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class asdmfnldf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_projects_fiscal_years_fiscal_year_id1",
                table: "projects");

            migrationBuilder.DropForeignKey(
                name: "FK_projects_staffs_staff_id1",
                table: "projects");

            migrationBuilder.DropIndex(
                name: "IX_projects_fiscal_year_id1",
                table: "projects");

            migrationBuilder.DropIndex(
                name: "IX_projects_staff_id1",
                table: "projects");

            migrationBuilder.DropColumn(
                name: "fiscal_year_id1",
                table: "projects");

            migrationBuilder.DropColumn(
                name: "staff_id1",
                table: "projects");

            migrationBuilder.CreateIndex(
                name: "IX_projects_fiscal_year_id",
                table: "projects",
                column: "fiscal_year_id");

            migrationBuilder.CreateIndex(
                name: "IX_projects_staff_id",
                table: "projects",
                column: "staff_id");

            migrationBuilder.AddForeignKey(
                name: "FK_projects_fiscal_years_fiscal_year_id",
                table: "projects",
                column: "fiscal_year_id",
                principalTable: "fiscal_years",
                principalColumn: "fiscal_year_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_projects_staffs_staff_id",
                table: "projects",
                column: "staff_id",
                principalTable: "staffs",
                principalColumn: "staff_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_projects_fiscal_years_fiscal_year_id",
                table: "projects");

            migrationBuilder.DropForeignKey(
                name: "FK_projects_staffs_staff_id",
                table: "projects");

            migrationBuilder.DropIndex(
                name: "IX_projects_fiscal_year_id",
                table: "projects");

            migrationBuilder.DropIndex(
                name: "IX_projects_staff_id",
                table: "projects");

            migrationBuilder.AddColumn<int>(
                name: "fiscal_year_id1",
                table: "projects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "staff_id1",
                table: "projects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_projects_fiscal_year_id1",
                table: "projects",
                column: "fiscal_year_id1");

            migrationBuilder.CreateIndex(
                name: "IX_projects_staff_id1",
                table: "projects",
                column: "staff_id1");

            migrationBuilder.AddForeignKey(
                name: "FK_projects_fiscal_years_fiscal_year_id1",
                table: "projects",
                column: "fiscal_year_id1",
                principalTable: "fiscal_years",
                principalColumn: "fiscal_year_id");

            migrationBuilder.AddForeignKey(
                name: "FK_projects_staffs_staff_id1",
                table: "projects",
                column: "staff_id1",
                principalTable: "staffs",
                principalColumn: "staff_id");
        }
    }
}
