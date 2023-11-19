using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.Transactions.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "transactions",
                schema: "transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: false),
                    _madeOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    tags = table.Column<string>(type: "text[]", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "transaction_sources",
                schema: "transactions",
                columns: table => new
                {
                    transaction_id = table.Column<Guid>(type: "uuid", nullable: false),
                    source_sender_address = table.Column<string>(type: "text", nullable: false),
                    source_amount = table.Column<int>(type: "integer", nullable: false),
                    source_amount_currency = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_sources", x => x.transaction_id);
                    table.ForeignKey(
                        name: "FK_transaction_sources_transactions_transaction_id",
                        column: x => x.transaction_id,
                        principalSchema: "transactions",
                        principalTable: "transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transaction_targets",
                schema: "transactions",
                columns: table => new
                {
                    transaction_id = table.Column<Guid>(type: "uuid", nullable: false),
                    target_recipient_address = table.Column<string>(type: "text", nullable: false),
                    target_amount = table.Column<int>(type: "integer", nullable: false),
                    target_amount_currency = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transaction_targets", x => x.transaction_id);
                    table.ForeignKey(
                        name: "FK_transaction_targets_transactions_transaction_id",
                        column: x => x.transaction_id,
                        principalSchema: "transactions",
                        principalTable: "transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transaction_sources",
                schema: "transactions");

            migrationBuilder.DropTable(
                name: "transaction_targets",
                schema: "transactions");

            migrationBuilder.DropTable(
                name: "transactions",
                schema: "transactions");
        }
    }
}
