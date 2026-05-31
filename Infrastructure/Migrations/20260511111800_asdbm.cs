using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class asdbm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "budget_sources",
                columns: table => new
                {
                    budget_source_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    budget_source_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_budget_sources", x => x.budget_source_id);
                });

            migrationBuilder.CreateTable(
                name: "expense_types",
                columns: table => new
                {
                    expense_type_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    expense_type_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expense_types", x => x.expense_type_id);
                });

            migrationBuilder.CreateTable(
                name: "fiscal_years",
                columns: table => new
                {
                    fiscal_year_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fiscal_year = table.Column<int>(type: "int", nullable: false),
                    year_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_closed = table.Column<bool>(type: "bit", nullable: false),
                    closed_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fiscal_years", x => x.fiscal_year_id);
                });

            migrationBuilder.CreateTable(
                name: "fund_categories",
                columns: table => new
                {
                    fund_category_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fund_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fund_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fund_categories", x => x.fund_category_id);
                });

            migrationBuilder.CreateTable(
                name: "operation_types",
                columns: table => new
                {
                    operation_type_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    operation_type_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operation_types", x => x.operation_type_id);
                });

            migrationBuilder.CreateTable(
                name: "vendors",
                columns: table => new
                {
                    vendor_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vendor_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    tax_no = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contact_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vendors", x => x.vendor_id);
                });

            migrationBuilder.CreateTable(
                name: "procurement_records",
                columns: table => new
                {
                    procurement_record_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    document_no = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    document_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    inspection_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    amount_text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    approval_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    reference_no = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    remark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    project_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fiscal_year_id = table.Column<int>(type: "int", nullable: false),
                    operation_type_id = table.Column<int>(type: "int", nullable: false),
                    expense_type_id = table.Column<int>(type: "int", nullable: false),
                    department_id = table.Column<int>(type: "int", nullable: false),
                    vendor_id = table.Column<int>(type: "int", nullable: false),
                    fund_category_id = table.Column<int>(type: "int", nullable: false),
                    budget_source_id = table.Column<int>(type: "int", nullable: false),
                    staff_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_procurement_records", x => x.procurement_record_id);
                    table.ForeignKey(
                        name: "FK_procurement_records_budget_sources_budget_source_id",
                        column: x => x.budget_source_id,
                        principalTable: "budget_sources",
                        principalColumn: "budget_source_id");
                    table.ForeignKey(
                        name: "FK_procurement_records_departments_department_id",
                        column: x => x.department_id,
                        principalTable: "departments",
                        principalColumn: "department_id");
                    table.ForeignKey(
                        name: "FK_procurement_records_expense_types_expense_type_id",
                        column: x => x.expense_type_id,
                        principalTable: "expense_types",
                        principalColumn: "expense_type_id");
                    table.ForeignKey(
                        name: "FK_procurement_records_fiscal_years_fiscal_year_id",
                        column: x => x.fiscal_year_id,
                        principalTable: "fiscal_years",
                        principalColumn: "fiscal_year_id");
                    table.ForeignKey(
                        name: "FK_procurement_records_fund_categories_fund_category_id",
                        column: x => x.fund_category_id,
                        principalTable: "fund_categories",
                        principalColumn: "fund_category_id");
                    table.ForeignKey(
                        name: "FK_procurement_records_operation_types_operation_type_id",
                        column: x => x.operation_type_id,
                        principalTable: "operation_types",
                        principalColumn: "operation_type_id");
                    table.ForeignKey(
                        name: "FK_procurement_records_staffs_staff_id",
                        column: x => x.staff_id,
                        principalTable: "staffs",
                        principalColumn: "staff_id");
                    table.ForeignKey(
                        name: "FK_procurement_records_vendors_vendor_id",
                        column: x => x.vendor_id,
                        principalTable: "vendors",
                        principalColumn: "vendor_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_procurement_records_budget_source_id",
                table: "procurement_records",
                column: "budget_source_id");

            migrationBuilder.CreateIndex(
                name: "IX_procurement_records_department_id",
                table: "procurement_records",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_procurement_records_expense_type_id",
                table: "procurement_records",
                column: "expense_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_procurement_records_fiscal_year_id",
                table: "procurement_records",
                column: "fiscal_year_id");

            migrationBuilder.CreateIndex(
                name: "IX_procurement_records_fund_category_id",
                table: "procurement_records",
                column: "fund_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_procurement_records_operation_type_id",
                table: "procurement_records",
                column: "operation_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_procurement_records_staff_id",
                table: "procurement_records",
                column: "staff_id");

            migrationBuilder.CreateIndex(
                name: "IX_procurement_records_vendor_id",
                table: "procurement_records",
                column: "vendor_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "procurement_records");

            migrationBuilder.DropTable(
                name: "budget_sources");

            migrationBuilder.DropTable(
                name: "expense_types");

            migrationBuilder.DropTable(
                name: "fiscal_years");

            migrationBuilder.DropTable(
                name: "fund_categories");

            migrationBuilder.DropTable(
                name: "operation_types");

            migrationBuilder.DropTable(
                name: "vendors");
        }
    }
}
