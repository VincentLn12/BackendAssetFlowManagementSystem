using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class materialReceiveDetailstransectiondd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_hireDetails_units_unit_id",
                table: "hireDetails");

            migrationBuilder.AlterColumn<int>(
                name: "unit_id",
                table: "hireDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_hireDetails_units_unit_id",
                table: "hireDetails",
                column: "unit_id",
                principalTable: "units",
                principalColumn: "unit_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_hireDetails_units_unit_id",
                table: "hireDetails");

            migrationBuilder.AlterColumn<int>(
                name: "unit_id",
                table: "hireDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_hireDetails_units_unit_id",
                table: "hireDetails",
                column: "unit_id",
                principalTable: "units",
                principalColumn: "unit_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
