using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.Wallets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameAssetsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assets",
                schema: "wallets");

            migrationBuilder.CreateTable(
                name: "investment_portfolio_assets",
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
                    table.PrimaryKey("PK_investment_portfolio_assets", x => x.id);
                    table.ForeignKey(
                        name: "FK_investment_portfolio_assets_investment_portfolios_investmen~",
                        column: x => x.investment_portfolio_id,
                        principalSchema: "wallets",
                        principalTable: "investment_portfolios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_investment_portfolio_assets_investment_portfolio_id",
                schema: "wallets",
                table: "investment_portfolio_assets",
                column: "investment_portfolio_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "investment_portfolio_assets",
                schema: "wallets");

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
    }
}
