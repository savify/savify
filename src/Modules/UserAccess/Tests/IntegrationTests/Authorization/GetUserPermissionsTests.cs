using App.Modules.UserAccess.Application.Authorization.GetUserPermissions;
using App.Modules.UserAccess.Application.Configuration.Data;
using App.Modules.UserAccess.Application.Users.CreateNewUser;
using App.Modules.UserAccess.IntegrationTests.Users;
using Dapper;
using Npgsql;

namespace App.Modules.UserAccess.IntegrationTests.Authorization;

[TestFixture]
public class GetUserPermissionsTests : TestBase
{
    [Test]
    public async Task GetUserPermissionsQuery_Test()
    {
        await PreparePermissions();
        var userId = await UserAccessModule.ExecuteCommandAsync(new CreateNewUserCommand(
            UserSampleData.Email,
            UserSampleData.PlainPassword,
            UserSampleData.Name,
            UserSampleData.Role.Value,
            UserSampleData.Country.Value
        ));

        var permissions = await UserAccessModule.ExecuteQueryAsync(new GetUserPermissionsQuery(userId));

        Assert.That(permissions.Count, Is.EqualTo(1));
        Assert.That(permissions.SingleOrDefault(p => p.Code == "SomePermission").Code, Is.Not.Null);
    }

    private async Task PreparePermissions()
    {
        await using var sqlConnection = new NpgsqlConnection(ConnectionString);

        var sql = $"INSERT INTO {DatabaseConfiguration.Schema.Name}.permissions VALUES ('SomePermission', 'SomePermission', 'SomePermission'); " +
                        $"INSERT INTO {DatabaseConfiguration.Schema.Name}.roles_permissions VALUES ('User', 'SomePermission');";

        await sqlConnection.ExecuteScalarAsync(sql);
    }
}
