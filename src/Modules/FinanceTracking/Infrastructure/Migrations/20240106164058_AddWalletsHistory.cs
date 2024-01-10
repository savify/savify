using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.FinanceTracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWalletsHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "wallet_histories",
                schema: "finance_tracking",
                columns: table => new
                {
                    wallet_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_wallet_histories", x => x.wallet_id);
                });

            migrationBuilder.CreateTable(
                name: "wallet_history_events",
                schema: "finance_tracking",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    data = table.Column<string>(type: "text", nullable: false),
                    wallet_history_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_wallet_history_events", x => x.id);
                    table.ForeignKey(
                        name: "fk_wallet_history_events_wallet_histories_wallet_history_id",
                        column: x => x.wallet_history_id,
                        principalSchema: "finance_tracking",
                        principalTable: "wallet_histories",
                        principalColumn: "wallet_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_wallet_history_events_wallet_history_id",
                schema: "finance_tracking",
                table: "wallet_history_events",
                column: "wallet_history_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "wallet_history_events",
                schema: "finance_tracking");

            migrationBuilder.DropTable(
                name: "wallet_histories",
                schema: "finance_tracking");
        }
    }
}
