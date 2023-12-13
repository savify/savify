using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.Transactions.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EFMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_outbox_messages",
                schema: "transactions",
                table: "outbox_messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_internal_commands",
                schema: "transactions",
                table: "internal_commands");

            migrationBuilder.DropPrimaryKey(
                name: "PK_inbox_messages",
                schema: "transactions",
                table: "inbox_messages");

            migrationBuilder.AddPrimaryKey(
                name: "pk_outbox_messages",
                schema: "transactions",
                table: "outbox_messages",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_internal_commands",
                schema: "transactions",
                table: "internal_commands",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_inbox_messages",
                schema: "transactions",
                table: "inbox_messages",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_outbox_messages",
                schema: "transactions",
                table: "outbox_messages");

            migrationBuilder.DropPrimaryKey(
                name: "pk_internal_commands",
                schema: "transactions",
                table: "internal_commands");

            migrationBuilder.DropPrimaryKey(
                name: "pk_inbox_messages",
                schema: "transactions",
                table: "inbox_messages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_outbox_messages",
                schema: "transactions",
                table: "outbox_messages",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_internal_commands",
                schema: "transactions",
                table: "internal_commands",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_inbox_messages",
                schema: "transactions",
                table: "inbox_messages",
                column: "id");
        }
    }
}
