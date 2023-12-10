using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.FinanceTracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameExpiresAtOnBankConnectionProcess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "expires_at",
                schema: "wallets",
                table: "bank_connection_processes",
                newName: "redirect_url_expires_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "redirect_url_expires_at",
                schema: "wallets",
                table: "bank_connection_processes",
                newName: "expires_at");
        }
    }
}
