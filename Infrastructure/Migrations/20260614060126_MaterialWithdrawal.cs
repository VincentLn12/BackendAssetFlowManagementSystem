using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MaterialWithdrawal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "materialWithdrawals",
                columns: table => new
                {
                    material_withdrawal_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    material_receive_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    receive_document_no = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    withdrawal_document_no = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    staff_id = table.Column<int>(type: "int", nullable: false),
                    procurement_record_id = table.Column<int>(type: "int", nullable: false),
                    remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_materialWithdrawals", x => x.material_withdrawal_id);
                    table.ForeignKey(
                        name: "FK_materialWithdrawals_procurement_records_procurement_record_id",
                        column: x => x.procurement_record_id,
                        principalTable: "procurement_records",
                        principalColumn: "procurement_record_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_materialWithdrawals_staffs_staff_id",
                        column: x => x.staff_id,
                        principalTable: "staffs",
                        principalColumn: "staff_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_materialWithdrawals_procurement_record_id",
                table: "materialWithdrawals",
                column: "procurement_record_id");

            migrationBuilder.CreateIndex(
                name: "IX_materialWithdrawals_staff_id",
                table: "materialWithdrawals",
                column: "staff_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "materialWithdrawals");
        }
    }
}
