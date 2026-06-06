using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class asfgsdgdsgdgSDg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_assetRepairs_assetItems_asset_id",
                table: "assetRepairs");

            migrationBuilder.RenameColumn(
                name: "asset_id",
                table: "assetRepairs",
                newName: "procurement_withdrawal_id");

            migrationBuilder.RenameIndex(
                name: "IX_assetRepairs_asset_id",
                table: "assetRepairs",
                newName: "IX_assetRepairs_procurement_withdrawal_id");

            migrationBuilder.AddForeignKey(
                name: "FK_assetRepairs_assetWithdrawals_procurement_withdrawal_id",
                table: "assetRepairs",
                column: "procurement_withdrawal_id",
                principalTable: "assetWithdrawals",
                principalColumn: "procurement_withdrawal_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_assetRepairs_assetWithdrawals_procurement_withdrawal_id",
                table: "assetRepairs");

            migrationBuilder.RenameColumn(
                name: "procurement_withdrawal_id",
                table: "assetRepairs",
                newName: "asset_id");

            migrationBuilder.RenameIndex(
                name: "IX_assetRepairs_procurement_withdrawal_id",
                table: "assetRepairs",
                newName: "IX_assetRepairs_asset_id");

            migrationBuilder.AddForeignKey(
                name: "FK_assetRepairs_assetItems_asset_id",
                table: "assetRepairs",
                column: "asset_id",
                principalTable: "assetItems",
                principalColumn: "asset_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
