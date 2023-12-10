using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.FinanceTracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameToFinanceTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "finance_tracking");

            migrationBuilder.RenameTable(
                name: "wallet_view_metadata",
                schema: "wallets",
                newName: "wallet_view_metadata",
                newSchema: "finance_tracking");

            migrationBuilder.RenameTable(
                name: "salt_edge_customers",
                schema: "wallets",
                newName: "salt_edge_customers",
                newSchema: "finance_tracking");

            migrationBuilder.RenameTable(
                name: "salt_edge_connections",
                schema: "wallets",
                newName: "salt_edge_connections",
                newSchema: "finance_tracking");

            migrationBuilder.RenameTable(
                name: "portfolio_view_matadata",
                schema: "wallets",
                newName: "portfolio_view_matadata",
                newSchema: "finance_tracking");

            migrationBuilder.RenameTable(
                name: "outbox_messages",
                schema: "wallets",
                newName: "outbox_messages",
                newSchema: "finance_tracking");

            migrationBuilder.RenameTable(
                name: "investment_portfolios",
                schema: "wallets",
                newName: "investment_portfolios",
                newSchema: "finance_tracking");

            migrationBuilder.RenameTable(
                name: "investment_portfolio_assets",
                schema: "wallets",
                newName: "investment_portfolio_assets",
                newSchema: "finance_tracking");

            migrationBuilder.RenameTable(
                name: "internal_commands",
                schema: "wallets",
                newName: "internal_commands",
                newSchema: "finance_tracking");

            migrationBuilder.RenameTable(
                name: "inbox_messages",
                schema: "wallets",
                newName: "inbox_messages",
                newSchema: "finance_tracking");

            migrationBuilder.RenameTable(
                name: "debit_wallets",
                schema: "wallets",
                newName: "debit_wallets",
                newSchema: "finance_tracking");

            migrationBuilder.RenameTable(
                name: "credit_wallets",
                schema: "wallets",
                newName: "credit_wallets",
                newSchema: "finance_tracking");

            migrationBuilder.RenameTable(
                name: "cash_wallets",
                schema: "wallets",
                newName: "cash_wallets",
                newSchema: "finance_tracking");

            migrationBuilder.RenameTable(
                name: "bank_connections",
                schema: "wallets",
                newName: "bank_connections",
                newSchema: "finance_tracking");

            migrationBuilder.RenameTable(
                name: "bank_connection_processes",
                schema: "wallets",
                newName: "bank_connection_processes",
                newSchema: "finance_tracking");

            migrationBuilder.RenameTable(
                name: "bank_accounts",
                schema: "wallets",
                newName: "bank_accounts",
                newSchema: "finance_tracking");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "wallets");

            migrationBuilder.RenameTable(
                name: "wallet_view_metadata",
                schema: "finance_tracking",
                newName: "wallet_view_metadata",
                newSchema: "wallets");

            migrationBuilder.RenameTable(
                name: "salt_edge_customers",
                schema: "finance_tracking",
                newName: "salt_edge_customers",
                newSchema: "wallets");

            migrationBuilder.RenameTable(
                name: "salt_edge_connections",
                schema: "finance_tracking",
                newName: "salt_edge_connections",
                newSchema: "wallets");

            migrationBuilder.RenameTable(
                name: "portfolio_view_matadata",
                schema: "finance_tracking",
                newName: "portfolio_view_matadata",
                newSchema: "wallets");

            migrationBuilder.RenameTable(
                name: "outbox_messages",
                schema: "finance_tracking",
                newName: "outbox_messages",
                newSchema: "wallets");

            migrationBuilder.RenameTable(
                name: "investment_portfolios",
                schema: "finance_tracking",
                newName: "investment_portfolios",
                newSchema: "wallets");

            migrationBuilder.RenameTable(
                name: "investment_portfolio_assets",
                schema: "finance_tracking",
                newName: "investment_portfolio_assets",
                newSchema: "wallets");

            migrationBuilder.RenameTable(
                name: "internal_commands",
                schema: "finance_tracking",
                newName: "internal_commands",
                newSchema: "wallets");

            migrationBuilder.RenameTable(
                name: "inbox_messages",
                schema: "finance_tracking",
                newName: "inbox_messages",
                newSchema: "wallets");

            migrationBuilder.RenameTable(
                name: "debit_wallets",
                schema: "finance_tracking",
                newName: "debit_wallets",
                newSchema: "wallets");

            migrationBuilder.RenameTable(
                name: "credit_wallets",
                schema: "finance_tracking",
                newName: "credit_wallets",
                newSchema: "wallets");

            migrationBuilder.RenameTable(
                name: "cash_wallets",
                schema: "finance_tracking",
                newName: "cash_wallets",
                newSchema: "wallets");

            migrationBuilder.RenameTable(
                name: "bank_connections",
                schema: "finance_tracking",
                newName: "bank_connections",
                newSchema: "wallets");

            migrationBuilder.RenameTable(
                name: "bank_connection_processes",
                schema: "finance_tracking",
                newName: "bank_connection_processes",
                newSchema: "wallets");

            migrationBuilder.RenameTable(
                name: "bank_accounts",
                schema: "finance_tracking",
                newName: "bank_accounts",
                newSchema: "wallets");
        }
    }
}
