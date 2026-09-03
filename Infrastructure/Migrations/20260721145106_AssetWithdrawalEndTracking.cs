using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AssetWithdrawalEndTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "end_date",
                table: "assetWithdrawals",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "end_reason",
                table: "assetWithdrawals",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "end_date",
                table: "assetWithdrawals");

            migrationBuilder.DropColumn(
                name: "end_reason",
                table: "assetWithdrawals");
        }
    }
}
