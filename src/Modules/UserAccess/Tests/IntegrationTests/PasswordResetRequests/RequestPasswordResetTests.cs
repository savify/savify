using App.BuildingBlocks.Application.Exceptions;
using App.Modules.UserAccess.Application.Configuration.Data;
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
            UserSampleData.Role.Value,
            UserSampleData.Country.Value
        ));

        var passwordResetRequestId = await UserAccessModule.ExecuteCommandAsync(
            new RequestPasswordResetCommand(UserSampleData.Email));

        var status = await GetPasswordResetRequestStatus(passwordResetRequestId);
        var notification = await GetLastOutboxMessage<PasswordResetRequestedNotification>();

        Assert.That(notification.DomainEvent.UserEmail, Is.EqualTo(UserSampleData.Email));
        Assert.That(status, Is.EqualTo(PasswordResetRequestStatus.WaitingForConfirmation.Value));
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("invalid_email")]
    public void RequestPasswordResetCommand_WhenEmailIsInvalid_ThrowsInvalidCommandException(string email)
    {
        Assert.That(() => UserAccessModule.ExecuteCommandAsync(new RequestPasswordResetCommand(email)),
            Throws.TypeOf<InvalidCommandException>());
    }

    private async Task<string> GetPasswordResetRequestStatus(Guid passwordResetRequestId)
    {
        await using var sqlConnection = new NpgsqlConnection(ConnectionString);

        var sql = $"SELECT status FROM {DatabaseConfiguration.Schema.Name}.password_reset_requests p WHERE p.id = @passwordResetRequestId";

        return (await sqlConnection.QuerySingleOrDefaultAsync<string>(sql, new { passwordResetRequestId }))!;
    }
}
