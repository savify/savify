using App.Modules.UserAccess.Application.PasswordResetRequests.RequestPasswordReset;
using App.Modules.UserAccess.Application.Users.CreateNewUser;
using App.Modules.UserAccess.Domain.PasswordResetRequest;
using App.Modules.UserAccess.IntegrationTests.Users;
using Dapper;
using Npgsql;

namespace App.Modules.UserAccess.IntegrationTests.PasswordResetRequests;

[TestFixture]
public class RequestPasswordResetTests : TestBase
{
    [Test]
    public async Task RequestPasswordResetCommand_Test()
    {
        await UserAccessModule.ExecuteCommandAsync(new CreateNewUserCommand(
            UserSampleData.Email,
            UserSampleData.PlainPassword,
            UserSampleData.Name,
            UserSampleData.Role.Value
        ));
        
        var passwordResetRequestId = await UserAccessModule.ExecuteCommandAsync(
            new RequestPasswordResetCommand(UserSampleData.Email));

        var status = await GetPasswordResetRequestStatus(passwordResetRequestId);
        var notification = await GetLastOutboxMessage<PasswordResetRequestedNotification>();
        
        Assert.That(notification.DomainEvent.UserEmail, Is.EqualTo(UserSampleData.Email));
        Assert.That(status, Is.EqualTo(PasswordResetRequestStatus.WaitingForConfirmation.Value));
    }
    
    private async Task<string> GetPasswordResetRequestStatus(Guid passwordResetRequestId)
    {
        await using var sqlConnection = new NpgsqlConnection(ConnectionString);

        var sql = "SELECT status FROM user_access.password_reset_requests p WHERE p.id = @passwordResetRequestId";

        return await sqlConnection.QuerySingleOrDefaultAsync<string>(sql, new {passwordResetRequestId});
    }
}
