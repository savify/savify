using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.Notifications.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EFMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_user_notification_settings",
                schema: "notifications",
                table: "user_notification_settings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_internal_commands",
                schema: "notifications",
                table: "internal_commands");

            migrationBuilder.DropPrimaryKey(
                name: "PK_inbox_messages",
                schema: "notifications",
                table: "inbox_messages");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_notification_settings",
                schema: "notifications",
                table: "user_notification_settings",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_internal_commands",
                schema: "notifications",
                table: "internal_commands",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_inbox_messages",
                schema: "notifications",
                table: "inbox_messages",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_user_notification_settings",
                schema: "notifications",
                table: "user_notification_settings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_internal_commands",
                schema: "notifications",
                table: "internal_commands");

            migrationBuilder.DropPrimaryKey(
                name: "pk_inbox_messages",
                schema: "notifications",
                table: "inbox_messages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_notification_settings",
                schema: "notifications",
                table: "user_notification_settings",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_internal_commands",
                schema: "notifications",
                table: "internal_commands",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_inbox_messages",
                schema: "notifications",
                table: "inbox_messages",
                column: "id");
        }
    }
}
