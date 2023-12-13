using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.Banks.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EFMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_outbox_messages",
                schema: "banks",
                table: "outbox_messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_internal_commands",
                schema: "banks",
                table: "internal_commands");

            migrationBuilder.DropPrimaryKey(
                name: "PK_inbox_messages",
                schema: "banks",
                table: "inbox_messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_banks_synchronisation_processes",
                schema: "banks",
                table: "banks_synchronisation_processes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_banks",
                schema: "banks",
                table: "banks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bank_revisions",
                schema: "banks",
                table: "bank_revisions");

            migrationBuilder.RenameIndex(
                name: "IX_banks_external_id",
                schema: "banks",
                table: "banks",
                newName: "ix_banks_external_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_outbox_messages",
                schema: "banks",
                table: "outbox_messages",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_internal_commands",
                schema: "banks",
                table: "internal_commands",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_inbox_messages",
                schema: "banks",
                table: "inbox_messages",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_banks_synchronisation_processes",
                schema: "banks",
                table: "banks_synchronisation_processes",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_banks",
                schema: "banks",
                table: "banks",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_bank_revisions",
                schema: "banks",
                table: "bank_revisions",
                columns: new[] { "id", "bank_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_outbox_messages",
                schema: "banks",
                table: "outbox_messages");

            migrationBuilder.DropPrimaryKey(
                name: "pk_internal_commands",
                schema: "banks",
                table: "internal_commands");

            migrationBuilder.DropPrimaryKey(
                name: "pk_inbox_messages",
                schema: "banks",
                table: "inbox_messages");

            migrationBuilder.DropPrimaryKey(
                name: "pk_banks_synchronisation_processes",
                schema: "banks",
                table: "banks_synchronisation_processes");

            migrationBuilder.DropPrimaryKey(
                name: "pk_banks",
                schema: "banks",
                table: "banks");

            migrationBuilder.DropPrimaryKey(
                name: "pk_bank_revisions",
                schema: "banks",
                table: "bank_revisions");

            migrationBuilder.RenameIndex(
                name: "ix_banks_external_id",
                schema: "banks",
                table: "banks",
                newName: "IX_banks_external_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_outbox_messages",
                schema: "banks",
                table: "outbox_messages",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_internal_commands",
                schema: "banks",
                table: "internal_commands",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_inbox_messages",
                schema: "banks",
                table: "inbox_messages",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_banks_synchronisation_processes",
                schema: "banks",
                table: "banks_synchronisation_processes",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_banks",
                schema: "banks",
                table: "banks",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bank_revisions",
                schema: "banks",
                table: "bank_revisions",
                columns: new[] { "id", "bank_id" });
        }
    }
}
