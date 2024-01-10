using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.FinanceTracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CashWalletInitialBalanceProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "finance_tracking",
                table: "cash_wallets");

            migrationBuilder.DropColumn(
                name: "removed_at",
                schema: "finance_tracking",
                table: "cash_wallets");

            migrationBuilder.DropColumn(
                name: "updated_at",
                schema: "finance_tracking",
                table: "cash_wallets");

            migrationBuilder.RenameColumn(
                name: "balance",
                schema: "finance_tracking",
                table: "cash_wallets",
                newName: "initial_balance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "initial_balance",
                schema: "finance_tracking",
                table: "cash_wallets",
                newName: "balance");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "finance_tracking",
                table: "cash_wallets",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "removed_at",
                schema: "finance_tracking",
                table: "cash_wallets",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                schema: "finance_tracking",
                table: "cash_wallets",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
