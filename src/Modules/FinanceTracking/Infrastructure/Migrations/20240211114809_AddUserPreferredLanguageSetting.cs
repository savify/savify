using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.FinanceTracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPreferredLanguageSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "preferred_language",
                schema: "finance_tracking",
                table: "user_finance_tracking_settings",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "preferred_language",
                schema: "finance_tracking",
                table: "user_finance_tracking_settings");
        }
    }
}
