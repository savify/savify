using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.FinanceTracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRemovalFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                schema: "wallets",
                table: "debit_wallets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "removed_at",
                schema: "wallets",
                table: "debit_wallets",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                schema: "wallets",
                table: "credit_wallets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "removed_at",
                schema: "wallets",
                table: "credit_wallets",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_removed",
                schema: "wallets",
                table: "cash_wallets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "removed_at",
                schema: "wallets",
                table: "cash_wallets",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_removed",
                schema: "wallets",
                table: "debit_wallets");

            migrationBuilder.DropColumn(
                name: "removed_at",
                schema: "wallets",
                table: "debit_wallets");

            migrationBuilder.DropColumn(
                name: "is_removed",
                schema: "wallets",
                table: "credit_wallets");

            migrationBuilder.DropColumn(
                name: "removed_at",
                schema: "wallets",
                table: "credit_wallets");

            migrationBuilder.DropColumn(
                name: "is_removed",
                schema: "wallets",
                table: "cash_wallets");

            migrationBuilder.DropColumn(
                name: "removed_at",
                schema: "wallets",
                table: "cash_wallets");
        }
    }
}
