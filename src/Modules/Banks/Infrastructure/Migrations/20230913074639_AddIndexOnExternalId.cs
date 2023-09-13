using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.Banks.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexOnExternalId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_banks",
                schema: "banks",
                table: "banks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_banks",
                schema: "banks",
                table: "banks",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_banks_external_id",
                schema: "banks",
                table: "banks",
                column: "external_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_banks",
                schema: "banks",
                table: "banks");

            migrationBuilder.DropIndex(
                name: "IX_banks_external_id",
                schema: "banks",
                table: "banks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_banks",
                schema: "banks",
                table: "banks",
                columns: new[] { "id", "external_id" });
        }
    }
}
