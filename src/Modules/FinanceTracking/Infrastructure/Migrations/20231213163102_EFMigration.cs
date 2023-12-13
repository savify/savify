using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.FinanceTracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EFMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bank_accounts_bank_connections_bank_connection_id",
                schema: "finance_tracking",
                table: "bank_accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_investment_portfolio_assets_investment_portfolios_investmen~",
                schema: "finance_tracking",
                table: "investment_portfolio_assets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_wallet_view_metadata",
                schema: "finance_tracking",
                table: "wallet_view_metadata");

            migrationBuilder.DropPrimaryKey(
                name: "PK_salt_edge_customers",
                schema: "finance_tracking",
                table: "salt_edge_customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_salt_edge_connections",
                schema: "finance_tracking",
                table: "salt_edge_connections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_outbox_messages",
                schema: "finance_tracking",
                table: "outbox_messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_investment_portfolios",
                schema: "finance_tracking",
                table: "investment_portfolios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_investment_portfolio_assets",
                schema: "finance_tracking",
                table: "investment_portfolio_assets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_internal_commands",
                schema: "finance_tracking",
                table: "internal_commands");

            migrationBuilder.DropPrimaryKey(
                name: "PK_inbox_messages",
                schema: "finance_tracking",
                table: "inbox_messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_debit_wallets",
                schema: "finance_tracking",
                table: "debit_wallets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_credit_wallets",
                schema: "finance_tracking",
                table: "credit_wallets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cash_wallets",
                schema: "finance_tracking",
                table: "cash_wallets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bank_connections",
                schema: "finance_tracking",
                table: "bank_connections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bank_connection_processes",
                schema: "finance_tracking",
                table: "bank_connection_processes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bank_accounts",
                schema: "finance_tracking",
                table: "bank_accounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_portfolio_view_matadata",
                schema: "finance_tracking",
                table: "portfolio_view_matadata");

            migrationBuilder.RenameTable(
                name: "portfolio_view_matadata",
                schema: "finance_tracking",
                newName: "portfolio_view_metadata",
                newSchema: "finance_tracking");

            migrationBuilder.RenameIndex(
                name: "IX_investment_portfolio_assets_investment_portfolio_id",
                schema: "finance_tracking",
                table: "investment_portfolio_assets",
                newName: "ix_investment_portfolio_assets_investment_portfolio_id");

            migrationBuilder.RenameIndex(
                name: "IX_bank_accounts_bank_connection_id",
                schema: "finance_tracking",
                table: "bank_accounts",
                newName: "ix_bank_accounts_bank_connection_id");

            migrationBuilder.AlterColumn<string>(
                name: "currency",
                schema: "finance_tracking",
                table: "debit_wallets",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "currency",
                schema: "finance_tracking",
                table: "credit_wallets",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "currency",
                schema: "finance_tracking",
                table: "cash_wallets",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "consent_expires_at",
                schema: "finance_tracking",
                table: "bank_connections",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "wallet_type",
                schema: "finance_tracking",
                table: "bank_connection_processes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                schema: "finance_tracking",
                table: "bank_connection_processes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_wallet_view_metadata",
                schema: "finance_tracking",
                table: "wallet_view_metadata",
                column: "wallet_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_salt_edge_customers",
                schema: "finance_tracking",
                table: "salt_edge_customers",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_salt_edge_connections",
                schema: "finance_tracking",
                table: "salt_edge_connections",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_outbox_messages",
                schema: "finance_tracking",
                table: "outbox_messages",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_investment_portfolios",
                schema: "finance_tracking",
                table: "investment_portfolios",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_investment_portfolio_assets",
                schema: "finance_tracking",
                table: "investment_portfolio_assets",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_internal_commands",
                schema: "finance_tracking",
                table: "internal_commands",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_inbox_messages",
                schema: "finance_tracking",
                table: "inbox_messages",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_debit_wallets",
                schema: "finance_tracking",
                table: "debit_wallets",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_credit_wallets",
                schema: "finance_tracking",
                table: "credit_wallets",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_cash_wallets",
                schema: "finance_tracking",
                table: "cash_wallets",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_bank_connections",
                schema: "finance_tracking",
                table: "bank_connections",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_bank_connection_processes",
                schema: "finance_tracking",
                table: "bank_connection_processes",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_bank_accounts",
                schema: "finance_tracking",
                table: "bank_accounts",
                columns: new[] { "id", "bank_connection_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_portfolio_view_metadata",
                schema: "finance_tracking",
                table: "portfolio_view_metadata",
                column: "portfolio_id");

            migrationBuilder.AddForeignKey(
                name: "fk_bank_accounts_bank_connections_bank_connection_id",
                schema: "finance_tracking",
                table: "bank_accounts",
                column: "bank_connection_id",
                principalSchema: "finance_tracking",
                principalTable: "bank_connections",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_investment_portfolio_assets_investment_portfolios_investmen",
                schema: "finance_tracking",
                table: "investment_portfolio_assets",
                column: "investment_portfolio_id",
                principalSchema: "finance_tracking",
                principalTable: "investment_portfolios",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_bank_accounts_bank_connections_bank_connection_id",
                schema: "finance_tracking",
                table: "bank_accounts");

            migrationBuilder.DropForeignKey(
                name: "fk_investment_portfolio_assets_investment_portfolios_investmen",
                schema: "finance_tracking",
                table: "investment_portfolio_assets");

            migrationBuilder.DropPrimaryKey(
                name: "pk_wallet_view_metadata",
                schema: "finance_tracking",
                table: "wallet_view_metadata");

            migrationBuilder.DropPrimaryKey(
                name: "pk_salt_edge_customers",
                schema: "finance_tracking",
                table: "salt_edge_customers");

            migrationBuilder.DropPrimaryKey(
                name: "pk_salt_edge_connections",
                schema: "finance_tracking",
                table: "salt_edge_connections");

            migrationBuilder.DropPrimaryKey(
                name: "pk_outbox_messages",
                schema: "finance_tracking",
                table: "outbox_messages");

            migrationBuilder.DropPrimaryKey(
                name: "pk_investment_portfolios",
                schema: "finance_tracking",
                table: "investment_portfolios");

            migrationBuilder.DropPrimaryKey(
                name: "pk_investment_portfolio_assets",
                schema: "finance_tracking",
                table: "investment_portfolio_assets");

            migrationBuilder.DropPrimaryKey(
                name: "pk_internal_commands",
                schema: "finance_tracking",
                table: "internal_commands");

            migrationBuilder.DropPrimaryKey(
                name: "pk_inbox_messages",
                schema: "finance_tracking",
                table: "inbox_messages");

            migrationBuilder.DropPrimaryKey(
                name: "pk_debit_wallets",
                schema: "finance_tracking",
                table: "debit_wallets");

            migrationBuilder.DropPrimaryKey(
                name: "pk_credit_wallets",
                schema: "finance_tracking",
                table: "credit_wallets");

            migrationBuilder.DropPrimaryKey(
                name: "pk_cash_wallets",
                schema: "finance_tracking",
                table: "cash_wallets");

            migrationBuilder.DropPrimaryKey(
                name: "pk_bank_connections",
                schema: "finance_tracking",
                table: "bank_connections");

            migrationBuilder.DropPrimaryKey(
                name: "pk_bank_connection_processes",
                schema: "finance_tracking",
                table: "bank_connection_processes");

            migrationBuilder.DropPrimaryKey(
                name: "pk_bank_accounts",
                schema: "finance_tracking",
                table: "bank_accounts");

            migrationBuilder.DropPrimaryKey(
                name: "pk_portfolio_view_metadata",
                schema: "finance_tracking",
                table: "portfolio_view_metadata");

            migrationBuilder.RenameTable(
                name: "portfolio_view_metadata",
                schema: "finance_tracking",
                newName: "portfolio_view_matadata",
                newSchema: "finance_tracking");

            migrationBuilder.RenameIndex(
                name: "ix_investment_portfolio_assets_investment_portfolio_id",
                schema: "finance_tracking",
                table: "investment_portfolio_assets",
                newName: "IX_investment_portfolio_assets_investment_portfolio_id");

            migrationBuilder.RenameIndex(
                name: "ix_bank_accounts_bank_connection_id",
                schema: "finance_tracking",
                table: "bank_accounts",
                newName: "IX_bank_accounts_bank_connection_id");

            migrationBuilder.AlterColumn<string>(
                name: "currency",
                schema: "finance_tracking",
                table: "debit_wallets",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "currency",
                schema: "finance_tracking",
                table: "credit_wallets",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "currency",
                schema: "finance_tracking",
                table: "cash_wallets",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "consent_expires_at",
                schema: "finance_tracking",
                table: "bank_connections",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "wallet_type",
                schema: "finance_tracking",
                table: "bank_connection_processes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                schema: "finance_tracking",
                table: "bank_connection_processes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_wallet_view_metadata",
                schema: "finance_tracking",
                table: "wallet_view_metadata",
                column: "wallet_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_salt_edge_customers",
                schema: "finance_tracking",
                table: "salt_edge_customers",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_salt_edge_connections",
                schema: "finance_tracking",
                table: "salt_edge_connections",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_outbox_messages",
                schema: "finance_tracking",
                table: "outbox_messages",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_investment_portfolios",
                schema: "finance_tracking",
                table: "investment_portfolios",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_investment_portfolio_assets",
                schema: "finance_tracking",
                table: "investment_portfolio_assets",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_internal_commands",
                schema: "finance_tracking",
                table: "internal_commands",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_inbox_messages",
                schema: "finance_tracking",
                table: "inbox_messages",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_debit_wallets",
                schema: "finance_tracking",
                table: "debit_wallets",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_credit_wallets",
                schema: "finance_tracking",
                table: "credit_wallets",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cash_wallets",
                schema: "finance_tracking",
                table: "cash_wallets",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bank_connections",
                schema: "finance_tracking",
                table: "bank_connections",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bank_connection_processes",
                schema: "finance_tracking",
                table: "bank_connection_processes",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bank_accounts",
                schema: "finance_tracking",
                table: "bank_accounts",
                columns: new[] { "id", "bank_connection_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_portfolio_view_matadata",
                schema: "finance_tracking",
                table: "portfolio_view_matadata",
                column: "portfolio_id");

            migrationBuilder.AddForeignKey(
                name: "FK_bank_accounts_bank_connections_bank_connection_id",
                schema: "finance_tracking",
                table: "bank_accounts",
                column: "bank_connection_id",
                principalSchema: "finance_tracking",
                principalTable: "bank_connections",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_investment_portfolio_assets_investment_portfolios_investmen~",
                schema: "finance_tracking",
                table: "investment_portfolio_assets",
                column: "investment_portfolio_id",
                principalSchema: "finance_tracking",
                principalTable: "investment_portfolios",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
