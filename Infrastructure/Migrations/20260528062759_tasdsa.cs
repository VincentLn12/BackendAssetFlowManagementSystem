using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class tasdsa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    project_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    project_code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    project_name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    fiscal_year_id = table.Column<int>(type: "int", nullable: false),
                    project_budget_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    staff_id = table.Column<int>(type: "int", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fiscal_year_id1 = table.Column<int>(type: "int", nullable: true),
                    staff_id1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projects", x => x.project_id);
                    table.ForeignKey(
                        name: "FK_projects_fiscal_years_fiscal_year_id1",
                        column: x => x.fiscal_year_id1,
                        principalTable: "fiscal_years",
                        principalColumn: "fiscal_year_id");
                    table.ForeignKey(
                        name: "FK_projects_staffs_staff_id1",
                        column: x => x.staff_id1,
                        principalTable: "staffs",
                        principalColumn: "staff_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_projects_fiscal_year_id1",
                table: "projects",
                column: "fiscal_year_id1");

            migrationBuilder.CreateIndex(
                name: "IX_projects_staff_id1",
                table: "projects",
                column: "staff_id1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "projects");
        }
    }
}
