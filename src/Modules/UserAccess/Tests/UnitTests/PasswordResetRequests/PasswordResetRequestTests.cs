using App.Modules.UserAccess.Domain;
using App.Modules.UserAccess.Domain.PasswordResetRequest;
using App.Modules.UserAccess.Domain.PasswordResetRequest.Events;
using App.Modules.UserAccess.Domain.PasswordResetRequest.Rules;
using App.Modules.UserAccess.Domain.Users;

namespace App.Modules.UserAccess.UnitTests.PasswordResetRequests;

[TestFixture]
public class PasswordResetRequestTests : UnitTestBase
{
    [Test]
    public void CreatingPasswordResetRequest_WithExistingUserEmail_IsSuccessful()
    {
        var usersCounter = Substitute.For<IUsersCounter>();
        usersCounter.CountUsersWithEmail("test@email.com").Returns(1);

        var passwordResetRequest = PasswordResetRequest.Create(
            "test@email.com",
            ConfirmationCode.From("ABC123"),
            usersCounter);

        var passwordResetRequestedDomainEvent = AssertPublishedDomainEvent<PasswordResetRequestedDomainEvent>(passwordResetRequest);
        Assert.That(passwordResetRequestedDomainEvent.UserEmail, Is.EqualTo("test@email.com"));
        Assert.That(passwordResetRequestedDomainEvent.ConfirmationCode, Is.EqualTo(ConfirmationCode.From("ABC123")));
        Assert.That(passwordResetRequestedDomainEvent.ValidTill, Is.GreaterThan(DateTime.UtcNow));
    }

    [Test]
    public void CreatingPasswordResetRequest_WithNonExistingUserEmail_BreaksUserWithGivenEmailMustExistRule()
    {
        var usersCounter = Substitute.For<IUsersCounter>();
        usersCounter.CountUsersWithEmail("test@email.com").Returns(0);


        AssertBrokenRule<UserWithGivenEmailMustExistRule>(() => PasswordResetRequest.Create(
            "test@email.com",
            ConfirmationCode.From("ABC123"),
            usersCounter));
    }

    [Test]
    public void ConfirmingPasswordResetRequest_WithMatchingConfirmationCode_IsSuccessful()
    {
        var usersCounter = Substitute.For<IUsersCounter>();
        usersCounter.CountUsersWithEmail("test@email.com").Returns(1);

        var passwordResetRequest = PasswordResetRequest.Create(
            "test@email.com",
            ConfirmationCode.From("ABC123"),
            usersCounter);

        passwordResetRequest.Confirm(ConfirmationCode.From("ABC123"));

        var passwordResetRequestConfirmedDomainEvent =
            AssertPublishedDomainEvent<PasswordResetRequestConfirmedDomainEvent>(passwordResetRequest);
        Assert.That(passwordResetRequestConfirmedDomainEvent.PasswordResetRequestId, Is.EqualTo(passwordResetRequest.Id));
    }

    [Test]
    public void ConfirmingPasswordResetRequest_WhenAlreadyConfirmed_BreaksPasswordResetRequestCannotBeConfirmedMoreThanOnceRule()
    {
        var usersCounter = Substitute.For<IUsersCounter>();
        usersCounter.CountUsersWithEmail("test@email.com").Returns(1);

        var passwordResetRequest = PasswordResetRequest.Create(
            "test@email.com",
            ConfirmationCode.From("ABC123"),
            usersCounter);

        passwordResetRequest.Confirm(ConfirmationCode.From("ABC123"));

        AssertBrokenRule<PasswordResetRequestCannotBeConfirmedMoreThanOnceRule>(() =>
            passwordResetRequest.Confirm(ConfirmationCode.From("ABC123")));
    }

    [Test]
    public void ConfirmingPasswordResetRequest_WithInvalidConfirmationCode_BreaksConfirmationCodeShouldMatchRule()
    {
        var usersCounter = Substitute.For<IUsersCounter>();
        usersCounter.CountUsersWithEmail("test@email.com").Returns(1);

        var passwordResetRequest = PasswordResetRequest.Create(
            "test@email.com",
            ConfirmationCode.From("ABC123"),
            usersCounter);

        AssertBrokenRule<ConfirmationCodeMustMatchRule>(() =>
            passwordResetRequest.Confirm(ConfirmationCode.From("INVALID")));
    }

    [Test]
    public void GettingUserId_FromUserEmail_IsSuccessful()
    {
        var usersCounter = Substitute.For<IUsersCounter>();
        usersCounter.CountUsersWithEmail("test@email.com").Returns(1);

        var userDetailsProvider = Substitute.For<IUserDetailsProvider>();
        userDetailsProvider.ProvideUserIdByEmail("test@email.com").Returns(new UserId(Guid.Parse("32fcdb2e-ce01-4996-94aa-a0dfc6f4c1f6")));

        var passwordResetRequest = PasswordResetRequest.Create(
            "test@email.com",
            ConfirmationCode.From("ABC123"),
            usersCounter);

        var userId = passwordResetRequest.GetUserId(userDetailsProvider);

        Assert.That(userId.Value, Is.EqualTo(Guid.Parse("32fcdb2e-ce01-4996-94aa-a0dfc6f4c1f6")));
    }
}
