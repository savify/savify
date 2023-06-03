using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.UserAccess.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddValidTillDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "renewed_at",
                schema: "user_access",
                table: "user_registrations");

            migrationBuilder.AddColumn<DateTime>(
                name: "valid_till",
                schema: "user_access",
                table: "user_registrations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "valid_till",
                schema: "user_access",
                table: "user_registrations");

            migrationBuilder.AddColumn<DateTime>(
                name: "renewed_at",
                schema: "user_access",
                table: "user_registrations",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
