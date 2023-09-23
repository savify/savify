using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.Banks.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBankRevisions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "current_revision_id",
                schema: "banks",
                table: "banks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "bank_revisions",
                schema: "banks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    bank_id = table.Column<Guid>(type: "uuid", nullable: false),
                    banks_synchronisation_process_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    default_logo_url = table.Column<string>(type: "text", nullable: false),
                    is_regulated = table.Column<bool>(type: "boolean", nullable: false),
                    logo_url = table.Column<string>(type: "text", nullable: true),
                    max_consent_days = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false),
                    revision_type = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bank_revisions", x => new { x.id, x.bank_id });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bank_revisions",
                schema: "banks");

            migrationBuilder.DropColumn(
                name: "current_revision_id",
                schema: "banks",
                table: "banks");
        }
    }
}
