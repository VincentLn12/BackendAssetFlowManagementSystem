using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MaterialItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "materialItems",
                columns: table => new
                {
                    material_item_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    material_code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    material_name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    specification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    unit_id = table.Column<int>(type: "int", nullable: false),
                    opening_balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    quantity_in = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    quantity_out = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    current_balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    unit_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    min_stock = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_materialItems", x => x.material_item_id);
                    table.ForeignKey(
                        name: "FK_materialItems_units_unit_id",
                        column: x => x.unit_id,
                        principalTable: "units",
                        principalColumn: "unit_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_materialItems_unit_id",
                table: "materialItems",
                column: "unit_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "materialItems");
        }
    }
}
