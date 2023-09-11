using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.Wallets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWalletIdToBankConnectionProcess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "wallet_id",
                schema: "wallets",
                table: "bank_connection_processes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "wallet_id",
                schema: "wallets",
                table: "bank_connection_processes");
        }
    }
}
