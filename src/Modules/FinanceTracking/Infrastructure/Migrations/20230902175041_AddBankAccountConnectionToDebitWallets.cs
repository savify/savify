using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.FinanceTracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBankAccountConnectionToDebitWallets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "bank_account_id",
                schema: "wallets",
                table: "debit_wallets",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "bank_connection_id",
                schema: "wallets",
                table: "debit_wallets",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bank_account_id",
                schema: "wallets",
                table: "debit_wallets");

            migrationBuilder.DropColumn(
                name: "bank_connection_id",
                schema: "wallets",
                table: "debit_wallets");
        }
    }
}
