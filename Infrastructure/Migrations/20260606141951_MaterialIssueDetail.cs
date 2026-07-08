using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MaterialIssueDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "materialIssueDetails",
                columns: table => new
                {
                    issue_detail_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    material_item_id = table.Column<int>(type: "int", nullable: false),
                    staff_id = table.Column<int>(type: "int", nullable: true),
                    issue_date = table.Column<DateTime>(type: "date", nullable: true),
                    quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    unit_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_materialIssueDetails", x => x.issue_detail_id);
                    table.ForeignKey(
                        name: "FK_materialIssueDetails_materialItems_material_item_id",
                        column: x => x.material_item_id,
                        principalTable: "materialItems",
                        principalColumn: "material_item_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_materialIssueDetails_staffs_staff_id",
                        column: x => x.staff_id,
                        principalTable: "staffs",
                        principalColumn: "staff_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_materialIssueDetails_material_item_id",
                table: "materialIssueDetails",
                column: "material_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_materialIssueDetails_staff_id",
                table: "materialIssueDetails",
                column: "staff_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "materialIssueDetails");
        }
    }
}
