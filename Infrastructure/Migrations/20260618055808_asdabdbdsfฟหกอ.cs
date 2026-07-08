using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class asdabdbdsfฟหกอ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_materialItems_departments_department_id",
                table: "materialItems");

            migrationBuilder.DropIndex(
                name: "IX_materialItems_department_id",
                table: "materialItems");

            migrationBuilder.DropColumn(
                name: "department_id",
                table: "materialItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "department_id",
                table: "materialItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_materialItems_department_id",
                table: "materialItems",
                column: "department_id");

            migrationBuilder.AddForeignKey(
                name: "FK_materialItems_departments_department_id",
                table: "materialItems",
                column: "department_id",
                principalTable: "departments",
                principalColumn: "department_id");
        }
    }
}
