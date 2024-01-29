using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.FinanceTracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionAmountVOToTransfers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "currency",
                schema: "finance_tracking",
                table: "transfers",
                newName: "target_currency_code");

            migrationBuilder.RenameColumn(
                name: "amount",
                schema: "finance_tracking",
                table: "transfers",
                newName: "target_amount");

            migrationBuilder.AddColumn<string>(
                name: "exchange_rate_from_currency_code",
                schema: "finance_tracking",
                table: "transfers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "exchange_rate_rate",
                schema: "finance_tracking",
                table: "transfers",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "exchange_rate_to_currency_code",
                schema: "finance_tracking",
                table: "transfers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "source_amount",
                schema: "finance_tracking",
                table: "transfers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "source_currency_code",
                schema: "finance_tracking",
                table: "transfers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "exchange_rate_from_currency_code",
                schema: "finance_tracking",
                table: "transfers");

            migrationBuilder.DropColumn(
                name: "exchange_rate_rate",
                schema: "finance_tracking",
                table: "transfers");

            migrationBuilder.DropColumn(
                name: "exchange_rate_to_currency_code",
                schema: "finance_tracking",
                table: "transfers");

            migrationBuilder.DropColumn(
                name: "source_amount",
                schema: "finance_tracking",
                table: "transfers");

            migrationBuilder.DropColumn(
                name: "source_currency_code",
                schema: "finance_tracking",
                table: "transfers");

            migrationBuilder.RenameColumn(
                name: "target_currency_code",
                schema: "finance_tracking",
                table: "transfers",
                newName: "currency");

            migrationBuilder.RenameColumn(
                name: "target_amount",
                schema: "finance_tracking",
                table: "transfers",
                newName: "amount");
        }
    }
}
