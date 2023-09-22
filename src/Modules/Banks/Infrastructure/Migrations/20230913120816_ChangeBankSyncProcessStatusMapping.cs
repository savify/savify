using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.Banks.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBankSyncProcessStatusMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "status",
                schema: "banks",
                table: "banks_synchronisation_processes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "status",
                schema: "banks",
                table: "banks_synchronisation_processes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
