using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class asfgsdgdsgdgSDgsad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "assetUsageTypes",
                columns: table => new
                {
                    usage_type_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usage_type_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assetUsageTypes", x => x.usage_type_id);
                });

            migrationBuilder.CreateTable(
                name: "assetSubItemHistories",
                columns: table => new
                {
                    sub_item_history_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    procurement_withdrawal_id = table.Column<int>(type: "int", nullable: false),
                    history_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    history_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    usage_type_id = table.Column<int>(type: "int", nullable: false),
                    detail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assetSubItemHistories", x => x.sub_item_history_id);
                    table.ForeignKey(
                        name: "FK_assetSubItemHistories_assetUsageTypes_usage_type_id",
                        column: x => x.usage_type_id,
                        principalTable: "assetUsageTypes",
                        principalColumn: "usage_type_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_assetSubItemHistories_assetWithdrawals_procurement_withdrawal_id",
                        column: x => x.procurement_withdrawal_id,
                        principalTable: "assetWithdrawals",
                        principalColumn: "procurement_withdrawal_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_assetSubItemHistories_procurement_withdrawal_id",
                table: "assetSubItemHistories",
                column: "procurement_withdrawal_id");

            migrationBuilder.CreateIndex(
                name: "IX_assetSubItemHistories_usage_type_id",
                table: "assetSubItemHistories",
                column: "usage_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assetSubItemHistories");

            migrationBuilder.DropTable(
                name: "assetUsageTypes");
        }
    }
}
