using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.Wallets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdjustBankConnectionAndBankAccountFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "wallet_type",
                schema: "wallets",
                table: "bank_connection_processes",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "currency",
                schema: "wallets",
                table: "bank_accounts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "wallet_type",
                schema: "wallets",
                table: "bank_connection_processes");

            migrationBuilder.AlterColumn<string>(
                name: "currency",
                schema: "wallets",
                table: "bank_accounts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
