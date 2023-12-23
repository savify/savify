using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.Categories.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoriesSynchronisationProcess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories_synchronisation_processes",
                schema: "categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    finished_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories_synchronisation_processes", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "categories_synchronisation_processes",
                schema: "categories");
        }
    }
}
