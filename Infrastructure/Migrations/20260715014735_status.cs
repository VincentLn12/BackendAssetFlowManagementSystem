using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "procurementRecordStatusHistories",
                columns: table => new
                {
                    status_history_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    procurement_record_id = table.Column<int>(type: "int", nullable: false),
                    from_status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    to_status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    changed_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    changed_by_staff_id = table.Column<int>(type: "int", nullable: true),
                    remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_procurementRecordStatusHistories", x => x.status_history_id);
                    table.ForeignKey(
                        name: "FK_procurementRecordStatusHistories_procurement_records_procurement_record_id",
                        column: x => x.procurement_record_id,
                        principalTable: "procurement_records",
                        principalColumn: "procurement_record_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_procurementRecordStatusHistories_staffs_changed_by_staff_id",
                        column: x => x.changed_by_staff_id,
                        principalTable: "staffs",
                        principalColumn: "staff_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_procurementRecordStatusHistories_changed_by_staff_id",
                table: "procurementRecordStatusHistories",
                column: "changed_by_staff_id");

            migrationBuilder.CreateIndex(
                name: "IX_procurementRecordStatusHistories_procurement_record_id",
                table: "procurementRecordStatusHistories",
                column: "procurement_record_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "procurementRecordStatusHistories");
        }
    }
}
