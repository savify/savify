using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.Wallets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsFinalFromBankConnectionProcessStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status_is_final",
                schema: "wallets",
                table: "bank_connection_processes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "status_is_final",
                schema: "wallets",
                table: "bank_connection_processes",
                type: "boolean",
                nullable: true);
        }
    }
}
