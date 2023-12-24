using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using App.BuildingBlocks.Application.Exceptions;
using App.Modules.UserAccess.Application.Configuration.Data;
using App.Modules.UserAccess.Application.PasswordResetRequests.ConfirmPasswordReset;
using App.Modules.UserAccess.Application.PasswordResetRequests.RequestPasswordReset;
using App.Modules.UserAccess.Application.Users.CreateNewUser;
using App.Modules.UserAccess.Domain.PasswordResetRequest;
using App.Modules.UserAccess.IntegrationTests.Users;
using Dapper;
using Npgsql;

namespace App.Modules.UserAccess.IntegrationTests.PasswordResetRequests;

[TestFixture]
public class ConfirmPasswordResetTests : TestBase
{
    [Test]
    public async Task ConfirmPasswordResetCommand_Test()
    {
        var userId = await UserAccessModule.ExecuteCommandAsync(new CreateNewUserCommand(
            UserSampleData.Email,
            UserSampleData.PlainPassword,
            UserSampleData.Name,
            UserSampleData.Role.Value,
            UserSampleData.Country.Value
        ));

        var passwordResetRequestId = await UserAccessModule.ExecuteCommandAsync(
            new RequestPasswordResetCommand(UserSampleData.Email));
        var notification = await GetLastOutboxMessage<PasswordResetRequestedNotification>();

        var token = await UserAccessModule.ExecuteCommandAsync(
            new ConfirmPasswordResetCommand(
                passwordResetRequestId,
                notification.DomainEvent.ConfirmationCode.Value));

        var decodedToken = DecodeJwtToken(token);
        var userIdFromToken = decodedToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
        var passwordResetRequestStatus = await GetPasswordResetRequestStatus(passwordResetRequestId);

        Assert.That(Guid.Parse(userIdFromToken), Is.EqualTo(userId));
        Assert.That(passwordResetRequestStatus, Is.EqualTo(PasswordResetRequestStatus.Confirmed.Value));
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    public void ConfirmPasswordResetCommand_WhenConfirmationCodeIsInvalid_ThrowsInvalidCommandException(string confirmationCode)
    {
        Assert.That(() => UserAccessModule.ExecuteCommandAsync(new ConfirmPasswordResetCommand(
            Guid.NewGuid(), confirmationCode)), Throws.TypeOf<InvalidCommandException>());
    }

    private JwtSecurityToken DecodeJwtToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        return handler.ReadJwtToken(token);
    }

    private async Task<string?> GetPasswordResetRequestStatus(Guid passwordResetRequestId)
    {
        await using var sqlConnection = new NpgsqlConnection(ConnectionString);

        var sql = $"SELECT status FROM {DatabaseConfiguration.Schema.Name}.password_reset_requests p WHERE p.id = @passwordResetRequestId";

        return await sqlConnection.QuerySingleOrDefaultAsync<string>(sql, new { passwordResetRequestId });
    }
}
