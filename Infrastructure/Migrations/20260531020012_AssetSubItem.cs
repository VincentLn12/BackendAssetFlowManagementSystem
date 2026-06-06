using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AssetSubItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "assetSubItems",
                columns: table => new
                {
                    asset_sub_item_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    asset_id = table.Column<int>(type: "int", nullable: false),
                    item_no = table.Column<int>(type: "int", nullable: true),
                    sub_item_name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    asset_category_id = table.Column<int>(type: "int", nullable: false),
                    running_start_no = table.Column<int>(type: "int", nullable: false),
                    running_end_no = table.Column<int>(type: "int", nullable: false),
                    fiscal_asset_year = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    unit_id = table.Column<int>(type: "int", nullable: false),
                    unit_price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    total_price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    useful_life_year = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assetSubItems", x => x.asset_sub_item_id);
                    table.ForeignKey(
                        name: "FK_assetSubItems_assetCategories_asset_category_id",
                        column: x => x.asset_category_id,
                        principalTable: "assetCategories",
                        principalColumn: "asset_category_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_assetSubItems_units_unit_id",
                        column: x => x.unit_id,
                        principalTable: "units",
                        principalColumn: "unit_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_assetSubItems_asset_category_id",
                table: "assetSubItems",
                column: "asset_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_assetSubItems_unit_id",
                table: "assetSubItems",
                column: "unit_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assetSubItems");
        }
    }
}
