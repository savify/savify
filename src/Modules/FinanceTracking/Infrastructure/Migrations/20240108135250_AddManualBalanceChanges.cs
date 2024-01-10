using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace App.Modules.FinanceTracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddManualBalanceChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cash_wallet_manual_balance_changes",
                schema: "finance_tracking",
                columns: table => new
                {
                    wallet_id = table.Column<Guid>(type: "uuid", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(type: "text", nullable: false),
                    amount = table.Column<int>(type: "integer", nullable: false),
                    currency = table.Column<string>(type: "text", nullable: false),
                    made_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cash_wallet_manual_balance_changes", x => new { x.wallet_id, x.id });
                    table.ForeignKey(
                        name: "fk_cash_wallet_manual_balance_changes_cash_wallets_wallet_id",
                        column: x => x.wallet_id,
                        principalSchema: "finance_tracking",
                        principalTable: "cash_wallets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "credit_wallet_manual_balance_changes",
                schema: "finance_tracking",
                columns: table => new
                {
                    wallet_id = table.Column<Guid>(type: "uuid", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(type: "text", nullable: false),
                    amount = table.Column<int>(type: "integer", nullable: false),
                    currency = table.Column<string>(type: "text", nullable: false),
                    made_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_credit_wallet_manual_balance_changes", x => new { x.wallet_id, x.id });
                    table.ForeignKey(
                        name: "fk_credit_wallet_manual_balance_changes_credit_wallets_wallet_",
                        column: x => x.wallet_id,
                        principalSchema: "finance_tracking",
                        principalTable: "credit_wallets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "debit_wallet_manual_balance_changes",
                schema: "finance_tracking",
                columns: table => new
                {
                    wallet_id = table.Column<Guid>(type: "uuid", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(type: "text", nullable: false),
                    amount = table.Column<int>(type: "integer", nullable: false),
                    currency = table.Column<string>(type: "text", nullable: false),
                    made_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_debit_wallet_manual_balance_changes", x => new { x.wallet_id, x.id });
                    table.ForeignKey(
                        name: "fk_debit_wallet_manual_balance_changes_debit_wallets_wallet_id",
                        column: x => x.wallet_id,
                        principalSchema: "finance_tracking",
                        principalTable: "debit_wallets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cash_wallet_manual_balance_changes",
                schema: "finance_tracking");

            migrationBuilder.DropTable(
                name: "credit_wallet_manual_balance_changes",
                schema: "finance_tracking");

            migrationBuilder.DropTable(
                name: "debit_wallet_manual_balance_changes",
                schema: "finance_tracking");
        }
    }
}
