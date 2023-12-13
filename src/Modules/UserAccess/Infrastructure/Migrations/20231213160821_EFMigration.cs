using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.UserAccess.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EFMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_roles_users_user_id",
                schema: "user_access",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                schema: "user_access",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_roles",
                schema: "user_access",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_registrations",
                schema: "user_access",
                table: "user_registrations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_password_reset_requests",
                schema: "user_access",
                table: "password_reset_requests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_outbox_messages",
                schema: "user_access",
                table: "outbox_messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_internal_commands",
                schema: "user_access",
                table: "internal_commands");

            migrationBuilder.DropPrimaryKey(
                name: "PK_inbox_messages",
                schema: "user_access",
                table: "inbox_messages");

            migrationBuilder.AlterColumn<string>(
                name: "preferred_language",
                schema: "user_access",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "country",
                schema: "user_access",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                schema: "user_access",
                table: "user_registrations",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "preferred_language",
                schema: "user_access",
                table: "user_registrations",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "country",
                schema: "user_access",
                table: "user_registrations",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "confirmation_code",
                schema: "user_access",
                table: "user_registrations",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                schema: "user_access",
                table: "password_reset_requests",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "confirmation_code",
                schema: "user_access",
                table: "password_reset_requests",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_users",
                schema: "user_access",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_roles",
                schema: "user_access",
                table: "user_roles",
                columns: new[] { "user_id", "role_code" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_registrations",
                schema: "user_access",
                table: "user_registrations",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_password_reset_requests",
                schema: "user_access",
                table: "password_reset_requests",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_outbox_messages",
                schema: "user_access",
                table: "outbox_messages",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_internal_commands",
                schema: "user_access",
                table: "internal_commands",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_inbox_messages",
                schema: "user_access",
                table: "inbox_messages",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_roles_users_user_id",
                schema: "user_access",
                table: "user_roles",
                column: "user_id",
                principalSchema: "user_access",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_roles_users_user_id",
                schema: "user_access",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_users",
                schema: "user_access",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_roles",
                schema: "user_access",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_registrations",
                schema: "user_access",
                table: "user_registrations");

            migrationBuilder.DropPrimaryKey(
                name: "pk_password_reset_requests",
                schema: "user_access",
                table: "password_reset_requests");

            migrationBuilder.DropPrimaryKey(
                name: "pk_outbox_messages",
                schema: "user_access",
                table: "outbox_messages");

            migrationBuilder.DropPrimaryKey(
                name: "pk_internal_commands",
                schema: "user_access",
                table: "internal_commands");

            migrationBuilder.DropPrimaryKey(
                name: "pk_inbox_messages",
                schema: "user_access",
                table: "inbox_messages");

            migrationBuilder.AlterColumn<string>(
                name: "preferred_language",
                schema: "user_access",
                table: "users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "country",
                schema: "user_access",
                table: "users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                schema: "user_access",
                table: "user_registrations",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "preferred_language",
                schema: "user_access",
                table: "user_registrations",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "country",
                schema: "user_access",
                table: "user_registrations",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "confirmation_code",
                schema: "user_access",
                table: "user_registrations",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                schema: "user_access",
                table: "password_reset_requests",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "confirmation_code",
                schema: "user_access",
                table: "password_reset_requests",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                schema: "user_access",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_roles",
                schema: "user_access",
                table: "user_roles",
                columns: new[] { "user_id", "role_code" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_registrations",
                schema: "user_access",
                table: "user_registrations",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_password_reset_requests",
                schema: "user_access",
                table: "password_reset_requests",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_outbox_messages",
                schema: "user_access",
                table: "outbox_messages",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_internal_commands",
                schema: "user_access",
                table: "internal_commands",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_inbox_messages",
                schema: "user_access",
                table: "inbox_messages",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_roles_users_user_id",
                schema: "user_access",
                table: "user_roles",
                column: "user_id",
                principalSchema: "user_access",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
