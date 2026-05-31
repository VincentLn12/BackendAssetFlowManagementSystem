using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class asdasg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "unit_id",
                table: "hireDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_hireDetails_unit_id",
                table: "hireDetails",
                column: "unit_id");

            migrationBuilder.AddForeignKey(
                name: "FK_hireDetails_units_unit_id",
                table: "hireDetails",
                column: "unit_id",
                principalTable: "units",
                principalColumn: "unit_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_hireDetails_units_unit_id",
                table: "hireDetails");

            migrationBuilder.DropIndex(
                name: "IX_hireDetails_unit_id",
                table: "hireDetails");

            migrationBuilder.DropColumn(
                name: "unit_id",
                table: "hireDetails");
        }
    }
}
