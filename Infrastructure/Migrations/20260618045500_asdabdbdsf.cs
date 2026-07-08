using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class asdabdbdsf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "department_id",
                table: "materialStockCards",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_materialStockCards_department_id",
                table: "materialStockCards",
                column: "department_id");

            migrationBuilder.AddForeignKey(
                name: "FK_materialStockCards_departments_department_id",
                table: "materialStockCards",
                column: "department_id",
                principalTable: "departments",
                principalColumn: "department_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_materialStockCards_departments_department_id",
                table: "materialStockCards");

            migrationBuilder.DropIndex(
                name: "IX_materialStockCards_department_id",
                table: "materialStockCards");

            migrationBuilder.DropColumn(
                name: "department_id",
                table: "materialStockCards");
        }
    }
}
