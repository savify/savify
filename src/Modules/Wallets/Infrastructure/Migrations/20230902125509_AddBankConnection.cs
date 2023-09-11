using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.Wallets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBankConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bank_connections",
                schema: "wallets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    bank_id = table.Column<Guid>(type: "uuid", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    refreshed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    consent_expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bank_connections", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "salt_edge_connections",
                schema: "wallets",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    internal_connection_id = table.Column<Guid>(type: "uuid", nullable: false),
                    provider_code = table.Column<string>(type: "text", nullable: false),
                    country_code = table.Column<string>(type: "text", nullable: false),
                    last_consent_id = table.Column<string>(type: "text", nullable: false),
                    customer_id = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salt_edge_connections", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bank_accounts",
                schema: "wallets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    bank_connection_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<int>(type: "integer", nullable: false),
                    external_id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    currency = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bank_accounts", x => new { x.id, x.bank_connection_id });
                    table.ForeignKey(
                        name: "FK_bank_accounts_bank_connections_bank_connection_id",
                        column: x => x.bank_connection_id,
                        principalSchema: "wallets",
                        principalTable: "bank_connections",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bank_accounts_bank_connection_id",
                schema: "wallets",
                table: "bank_accounts",
                column: "bank_connection_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bank_accounts",
                schema: "wallets");

            migrationBuilder.DropTable(
                name: "salt_edge_connections",
                schema: "wallets");

            migrationBuilder.DropTable(
                name: "bank_connections",
                schema: "wallets");
        }
    }
}
