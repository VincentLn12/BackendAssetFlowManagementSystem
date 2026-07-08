using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class materialReceiveDetailstransection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "materialReceiveDetails",
                columns: table => new
                {
                    receive_detail_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    procurement_record_id = table.Column<int>(type: "int", nullable: false),
                    item_no = table.Column<int>(type: "int", nullable: false),
                    material_item_id = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    unit_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    operation_reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_materialReceiveDetails", x => x.receive_detail_id);
                    table.ForeignKey(
                        name: "FK_materialReceiveDetails_materialItems_material_item_id",
                        column: x => x.material_item_id,
                        principalTable: "materialItems",
                        principalColumn: "material_item_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_materialReceiveDetails_procurement_records_procurement_record_id",
                        column: x => x.procurement_record_id,
                        principalTable: "procurement_records",
                        principalColumn: "procurement_record_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "materialStockCards",
                columns: table => new
                {
                    stock_card_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    material_item_id = table.Column<int>(type: "int", nullable: false),
                    transaction_date = table.Column<DateTime>(type: "date", nullable: false),
                    transaction_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    reference_document_no = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    receive_detail_id = table.Column<int>(type: "int", nullable: true),
                    issue_detail_id = table.Column<int>(type: "int", nullable: true),
                    quantity_in = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    quantity_out = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    balance_qty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    unit_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_materialStockCards", x => x.stock_card_id);
                    table.ForeignKey(
                        name: "FK_materialStockCards_materialIssueDetails_issue_detail_id",
                        column: x => x.issue_detail_id,
                        principalTable: "materialIssueDetails",
                        principalColumn: "issue_detail_id");
                    table.ForeignKey(
                        name: "FK_materialStockCards_materialItems_material_item_id",
                        column: x => x.material_item_id,
                        principalTable: "materialItems",
                        principalColumn: "material_item_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_materialStockCards_materialReceiveDetails_receive_detail_id",
                        column: x => x.receive_detail_id,
                        principalTable: "materialReceiveDetails",
                        principalColumn: "receive_detail_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_materialReceiveDetails_material_item_id",
                table: "materialReceiveDetails",
                column: "material_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_materialReceiveDetails_procurement_record_id",
                table: "materialReceiveDetails",
                column: "procurement_record_id");

            migrationBuilder.CreateIndex(
                name: "IX_materialStockCards_issue_detail_id",
                table: "materialStockCards",
                column: "issue_detail_id");

            migrationBuilder.CreateIndex(
                name: "IX_materialStockCards_material_item_id",
                table: "materialStockCards",
                column: "material_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_materialStockCards_receive_detail_id",
                table: "materialStockCards",
                column: "receive_detail_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "materialStockCards");

            migrationBuilder.DropTable(
                name: "materialReceiveDetails");
        }
    }
}
