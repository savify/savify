using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.FinanceTracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMoneyAmountToInteger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "currency",
                schema: "finance_tracking",
                table: "transfers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "amount",
                schema: "finance_tracking",
                table: "transfers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "purchase_price_currency",
                schema: "finance_tracking",
                table: "investment_portfolio_assets",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.DropColumn(
                name: "purchase_price_amount",
                schema: "finance_tracking",
                table: "investment_portfolio_assets");

            migrationBuilder.AddColumn<int>(
                name: "purchase_price_amount",
                schema: "finance_tracking",
                table: "investment_portfolio_assets",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "currency",
                schema: "finance_tracking",
                table: "transfers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "amount",
                schema: "finance_tracking",
                table: "transfers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "purchase_price_currency",
                schema: "finance_tracking",
                table: "investment_portfolio_assets",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.DropColumn(
                name: "purchase_price_amount",
                schema: "finance_tracking",
                table: "investment_portfolio_assets");

            migrationBuilder.AddColumn<decimal>(
                name: "purchase_price_amount",
                schema: "finance_tracking",
                table: "investment_portfolio_assets",
                type: "money",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "purchase_price_amount",
                schema: "finance_tracking",
                table: "investment_portfolio_assets",
                type: "money",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
