using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.FinanceTracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInvestmentPortfoliosAndViewMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "investment_portfolios",
                schema: "wallets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_investment_portfolios", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "portfolio_view_matadata",
                schema: "wallets",
                columns: table => new
                {
                    portfolio_id = table.Column<Guid>(type: "uuid", nullable: false),
                    color = table.Column<string>(type: "text", nullable: true),
                    icon = table.Column<string>(type: "text", nullable: true),
                    is_considered_in_total_balance = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_portfolio_view_matadata", x => x.portfolio_id);
                });

            migrationBuilder.CreateTable(
                name: "assets",
                schema: "wallets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    country = table.Column<string>(type: "text", nullable: false),
                    exchange = table.Column<string>(type: "text", nullable: false),
                    purchased_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ticker_symbol = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    investment_portfolio_id = table.Column<Guid>(type: "uuid", nullable: false),
                    purchase_price_amount = table.Column<decimal>(type: "money", nullable: true),
                    purchase_price_currency = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assets", x => x.id);
                    table.ForeignKey(
                        name: "FK_assets_investment_portfolios_investment_portfolio_id",
                        column: x => x.investment_portfolio_id,
                        principalSchema: "wallets",
                        principalTable: "investment_portfolios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_assets_investment_portfolio_id",
                schema: "wallets",
                table: "assets",
                column: "investment_portfolio_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assets",
                schema: "wallets");

            migrationBuilder.DropTable(
                name: "portfolio_view_matadata",
                schema: "wallets");

            migrationBuilder.DropTable(
                name: "investment_portfolios",
                schema: "wallets");
        }
    }
}
