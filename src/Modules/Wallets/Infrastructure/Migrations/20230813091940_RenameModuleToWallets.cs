using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.Wallets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameModuleToWallets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_view_metadata",
                schema: "accounts");

            migrationBuilder.DropTable(
                name: "cash_accounts",
                schema: "accounts");

            migrationBuilder.DropTable(
                name: "credit_accounts",
                schema: "accounts");

            migrationBuilder.DropTable(
                name: "debit_accounts",
                schema: "accounts");

            migrationBuilder.EnsureSchema(
                name: "wallets");

            migrationBuilder.RenameTable(
                name: "outbox_messages",
                schema: "accounts",
                newName: "outbox_messages",
                newSchema: "wallets");

            migrationBuilder.RenameTable(
                name: "internal_commands",
                schema: "accounts",
                newName: "internal_commands",
                newSchema: "wallets");

            migrationBuilder.RenameTable(
                name: "inbox_messages",
                schema: "accounts",
                newName: "inbox_messages",
                newSchema: "wallets");

            migrationBuilder.CreateTable(
                name: "cash_wallets",
                schema: "wallets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    balance = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    currency = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cash_wallets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "credit_wallets",
                schema: "wallets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    available_balance = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    credit_limit = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    currency = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_wallets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "debit_wallets",
                schema: "wallets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    balance = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    currency = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_debit_wallets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "wallet_view_metadata",
                schema: "wallets",
                columns: table => new
                {
                    wallet_id = table.Column<Guid>(type: "uuid", nullable: false),
                    color = table.Column<string>(type: "text", nullable: true),
                    icon = table.Column<string>(type: "text", nullable: true),
                    is_considered_in_total_balance = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallet_view_metadata", x => x.wallet_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cash_wallets",
                schema: "wallets");

            migrationBuilder.DropTable(
                name: "credit_wallets",
                schema: "wallets");

            migrationBuilder.DropTable(
                name: "debit_wallets",
                schema: "wallets");

            migrationBuilder.DropTable(
                name: "wallet_view_metadata",
                schema: "wallets");

            migrationBuilder.EnsureSchema(
                name: "accounts");

            migrationBuilder.RenameTable(
                name: "outbox_messages",
                schema: "wallets",
                newName: "outbox_messages",
                newSchema: "accounts");

            migrationBuilder.RenameTable(
                name: "internal_commands",
                schema: "wallets",
                newName: "internal_commands",
                newSchema: "accounts");

            migrationBuilder.RenameTable(
                name: "inbox_messages",
                schema: "wallets",
                newName: "inbox_messages",
                newSchema: "accounts");

            migrationBuilder.CreateTable(
                name: "account_view_metadata",
                schema: "accounts",
                columns: table => new
                {
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    color = table.Column<string>(type: "text", nullable: true),
                    icon = table.Column<string>(type: "text", nullable: true),
                    is_considered_in_total_balance = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_view_metadata", x => x.account_id);
                });

            migrationBuilder.CreateTable(
                name: "cash_accounts",
                schema: "accounts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    balance = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    currency = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cash_accounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "credit_accounts",
                schema: "accounts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    available_balance = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    credit_limit = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    currency = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_accounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "debit_accounts",
                schema: "accounts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    balance = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    currency = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_debit_accounts", x => x.id);
                });
        }
    }
}
