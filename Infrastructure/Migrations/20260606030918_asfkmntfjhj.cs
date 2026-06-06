using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class asfkmntfjhj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "staff_id",
                table: "assetSubItemHistories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "staff_id",
                table: "assetRepairs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_assetSubItemHistories_staff_id",
                table: "assetSubItemHistories",
                column: "staff_id");

            migrationBuilder.CreateIndex(
                name: "IX_assetRepairs_staff_id",
                table: "assetRepairs",
                column: "staff_id");

            migrationBuilder.AddForeignKey(
                name: "FK_assetRepairs_staffs_staff_id",
                table: "assetRepairs",
                column: "staff_id",
                principalTable: "staffs",
                principalColumn: "staff_id");

            migrationBuilder.AddForeignKey(
                name: "FK_assetSubItemHistories_staffs_staff_id",
                table: "assetSubItemHistories",
                column: "staff_id",
                principalTable: "staffs",
                principalColumn: "staff_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_assetRepairs_staffs_staff_id",
                table: "assetRepairs");

            migrationBuilder.DropForeignKey(
                name: "FK_assetSubItemHistories_staffs_staff_id",
                table: "assetSubItemHistories");

            migrationBuilder.DropIndex(
                name: "IX_assetSubItemHistories_staff_id",
                table: "assetSubItemHistories");

            migrationBuilder.DropIndex(
                name: "IX_assetRepairs_staff_id",
                table: "assetRepairs");

            migrationBuilder.DropColumn(
                name: "staff_id",
                table: "assetSubItemHistories");

            migrationBuilder.DropColumn(
                name: "staff_id",
                table: "assetRepairs");
        }
    }
}
