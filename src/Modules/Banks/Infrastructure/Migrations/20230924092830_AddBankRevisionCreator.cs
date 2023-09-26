using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.Banks.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBankRevisionCreator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "banks_synchronisation_process_id",
                schema: "banks",
                table: "bank_revisions");

            migrationBuilder.AddColumn<Guid>(
                name: "created_by_id",
                schema: "banks",
                table: "bank_revisions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "created_by_type",
                schema: "banks",
                table: "bank_revisions",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_by_id",
                schema: "banks",
                table: "bank_revisions");

            migrationBuilder.DropColumn(
                name: "created_by_type",
                schema: "banks",
                table: "bank_revisions");

            migrationBuilder.AddColumn<Guid>(
                name: "banks_synchronisation_process_id",
                schema: "banks",
                table: "bank_revisions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
