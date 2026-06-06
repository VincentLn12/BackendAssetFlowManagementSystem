using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class asasgasddsd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "assetWithdrawals",
                columns: table => new
                {
                    procurement_withdrawal_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    procurement_record_id = table.Column<int>(type: "int", nullable: false),
                    withdrawal_document_no = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    withdrawal_date = table.Column<DateTime>(type: "date", nullable: false),
                    staff_id = table.Column<int>(type: "int", nullable: false),
                    storage_location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    purpose = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assetWithdrawals", x => x.procurement_withdrawal_id);
                    table.ForeignKey(
                        name: "FK_assetWithdrawals_procurement_records_procurement_record_id",
                        column: x => x.procurement_record_id,
                        principalTable: "procurement_records",
                        principalColumn: "procurement_record_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_assetWithdrawals_staffs_staff_id",
                        column: x => x.staff_id,
                        principalTable: "staffs",
                        principalColumn: "staff_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_assetWithdrawals_procurement_record_id",
                table: "assetWithdrawals",
                column: "procurement_record_id");

            migrationBuilder.CreateIndex(
                name: "IX_assetWithdrawals_staff_id",
                table: "assetWithdrawals",
                column: "staff_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assetWithdrawals");
        }
    }
}
