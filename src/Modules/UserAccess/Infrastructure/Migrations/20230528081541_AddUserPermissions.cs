using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Modules.UserAccess.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "permissions",
                schema: "user_access",
                columns: table => new
                {
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => x.code);
                });
            
            migrationBuilder.CreateTable(
                name: "roles_permissions",
                schema: "user_access",
                columns: table => new
                {
                    role_code = table.Column<string>(type: "text", nullable: false),
                    permission_code = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles_permissions", x => new { x.role_code, x.permission_code });
                });

            migrationBuilder.Sql("CREATE VIEW user_access.user_permissions_view AS " +
                                 "SELECT DISTINCT user_roles.user_id, roles_permissions.permission_code " +
                                 "FROM user_access.user_roles INNER JOIN user_access.roles_permissions ON user_roles.role_code = roles_permissions.role_code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("permissions", "user_access");
            migrationBuilder.DropTable("roles_permissions", "user_access");
            migrationBuilder.Sql("DROP VIEW user_access.user_permissions_view");
        }
    }
}
