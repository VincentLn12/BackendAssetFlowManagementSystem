using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class asdasd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "assetSubItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "assetSubItemDisposals",
                columns: table => new
                {
                    sub_item_disposal_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    asset_sub_item_id = table.Column<int>(type: "int", nullable: false),
                    disposal_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    disposal_method = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    disposal_reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    document_no = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    approved_by = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    quantity_disposed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assetSubItemDisposals", x => x.sub_item_disposal_id);
                    table.ForeignKey(
                        name: "FK_assetSubItemDisposals_assetSubItems_asset_sub_item_id",
                        column: x => x.asset_sub_item_id,
                        principalTable: "assetSubItems",
                        principalColumn: "asset_sub_item_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_assetSubItemDisposals_asset_sub_item_id",
                table: "assetSubItemDisposals",
                column: "asset_sub_item_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assetSubItemDisposals");

            migrationBuilder.DropColumn(
                name: "status",
                table: "assetSubItems");
        }
    }
}
