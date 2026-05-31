using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AssetItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "assetItems",
                columns: table => new
                {
                    asset_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    procurement_record_id = table.Column<int>(type: "int", nullable: true),
                    item_no = table.Column<int>(type: "int", nullable: false),
                    asset_code_prefix = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    asset_name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    unit_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    total_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    receive_date = table.Column<DateTime>(type: "date", nullable: false),
                    useful_life_year = table.Column<int>(type: "int", nullable: false),
                    asset_category_id = table.Column<int>(type: "int", nullable: false),
                    unit_id = table.Column<int>(type: "int", nullable: false),
                    fund_category_id = table.Column<int>(type: "int", nullable: true),
                    department_id = table.Column<int>(type: "int", nullable: true),
                    staff_id = table.Column<int>(type: "int", nullable: true),
                    vendor_id = table.Column<int>(type: "int", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assetItems", x => x.asset_id);
                    table.ForeignKey(
                        name: "FK_assetItems_assetCategories_asset_category_id",
                        column: x => x.asset_category_id,
                        principalTable: "assetCategories",
                        principalColumn: "asset_category_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_assetItems_departments_department_id",
                        column: x => x.department_id,
                        principalTable: "departments",
                        principalColumn: "department_id");
                    table.ForeignKey(
                        name: "FK_assetItems_fund_categories_fund_category_id",
                        column: x => x.fund_category_id,
                        principalTable: "fund_categories",
                        principalColumn: "fund_category_id");
                    table.ForeignKey(
                        name: "FK_assetItems_staffs_staff_id",
                        column: x => x.staff_id,
                        principalTable: "staffs",
                        principalColumn: "staff_id");
                    table.ForeignKey(
                        name: "FK_assetItems_units_unit_id",
                        column: x => x.unit_id,
                        principalTable: "units",
                        principalColumn: "unit_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_assetItems_vendors_vendor_id",
                        column: x => x.vendor_id,
                        principalTable: "vendors",
                        principalColumn: "vendor_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_assetItems_asset_category_id",
                table: "assetItems",
                column: "asset_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_assetItems_department_id",
                table: "assetItems",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_assetItems_fund_category_id",
                table: "assetItems",
                column: "fund_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_assetItems_staff_id",
                table: "assetItems",
                column: "staff_id");

            migrationBuilder.CreateIndex(
                name: "IX_assetItems_unit_id",
                table: "assetItems",
                column: "unit_id");

            migrationBuilder.CreateIndex(
                name: "IX_assetItems_vendor_id",
                table: "assetItems",
                column: "vendor_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assetItems");
        }
    }
}
