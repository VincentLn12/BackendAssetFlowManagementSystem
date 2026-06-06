using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class asdasdasddas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "assetRepairs",
                columns: table => new
                {
                    asset_repair_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    repair_document_no = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    repair_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    problem_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    repair_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    repair_shop_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    repair_cost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    decree_document_no = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    asset_id = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assetRepairs", x => x.asset_repair_id);
                    table.ForeignKey(
                        name: "FK_assetRepairs_assetItems_asset_id",
                        column: x => x.asset_id,
                        principalTable: "assetItems",
                        principalColumn: "asset_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_assetRepairs_asset_id",
                table: "assetRepairs",
                column: "asset_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assetRepairs");
        }
    }
}
