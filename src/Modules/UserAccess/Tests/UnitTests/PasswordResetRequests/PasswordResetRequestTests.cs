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

        var userId = new UserId(Guid.NewGuid());
        var userDetailsProvider = Substitute.For<IUserDetailsProvider>();
        userDetailsProvider.ProvideUserIdByEmail("test@email.com").Returns(userId);

        var passwordResetRequest = PasswordResetRequest.Create(
            "test@email.com",
            ConfirmationCode.From("ABC123"),
            usersCounter,
            userDetailsProvider);

        var passwordResetRequestedDomainEvent = AssertPublishedDomainEvent<PasswordResetRequestedDomainEvent>(passwordResetRequest);
        Assert.That(passwordResetRequestedDomainEvent.UserEmail, Is.EqualTo("test@email.com"));
        Assert.That(passwordResetRequestedDomainEvent.ConfirmationCode, Is.EqualTo(ConfirmationCode.From("ABC123")));
        Assert.That(passwordResetRequestedDomainEvent.ValidTill, Is.GreaterThan(DateTime.UtcNow));
    }

    [Test]
    public void CreatingPasswordResetRequest_WithNonExistingUserEmail_BreaksUserWithGivenEmailMustExistRule()
    {
        var userDetailsProvider = Substitute.For<IUserDetailsProvider>();
        var usersCounter = Substitute.For<IUsersCounter>();
        usersCounter.CountUsersWithEmail("test@email.com").Returns(0);

        AssertBrokenRule<UserWithGivenEmailMustExistRule>(() => PasswordResetRequest.Create(
            "test@email.com",
            ConfirmationCode.From("ABC123"),
            usersCounter,
            userDetailsProvider));
    }

    [Test]
    public void ConfirmingPasswordResetRequest_WithMatchingConfirmationCode_IsSuccessful()
    {
        var userDetailsProvider = Substitute.For<IUserDetailsProvider>();
        var usersCounter = Substitute.For<IUsersCounter>();
        usersCounter.CountUsersWithEmail("test@email.com").Returns(1);

        var passwordResetRequest = PasswordResetRequest.Create(
            "test@email.com",
            ConfirmationCode.From("ABC123"),
            usersCounter,
            userDetailsProvider);

        passwordResetRequest.Confirm(ConfirmationCode.From("ABC123"));

        var passwordResetRequestConfirmedDomainEvent =
            AssertPublishedDomainEvent<PasswordResetRequestConfirmedDomainEvent>(passwordResetRequest);
        Assert.That(passwordResetRequestConfirmedDomainEvent.PasswordResetRequestId, Is.EqualTo(passwordResetRequest.Id));
    }

    [Test]
    public void ConfirmingPasswordResetRequest_WhenAlreadyConfirmed_BreaksPasswordResetRequestCannotBeConfirmedMoreThanOnceRule()
    {
        var userDetailsProvider = Substitute.For<IUserDetailsProvider>();
        var usersCounter = Substitute.For<IUsersCounter>();
        usersCounter.CountUsersWithEmail("test@email.com").Returns(1);

        var passwordResetRequest = PasswordResetRequest.Create(
            "test@email.com",
            ConfirmationCode.From("ABC123"),
            usersCounter,
            userDetailsProvider);

        passwordResetRequest.Confirm(ConfirmationCode.From("ABC123"));

        AssertBrokenRule<PasswordResetRequestCannotBeConfirmedMoreThanOnceRule>(() =>
            passwordResetRequest.Confirm(ConfirmationCode.From("ABC123")));
    }

    [Test]
    public void ConfirmingPasswordResetRequest_WithInvalidConfirmationCode_BreaksConfirmationCodeShouldMatchRule()
    {
        var userDetailsProvider = Substitute.For<IUserDetailsProvider>();
        var usersCounter = Substitute.For<IUsersCounter>();
        usersCounter.CountUsersWithEmail("test@email.com").Returns(1);

        var passwordResetRequest = PasswordResetRequest.Create(
            "test@email.com",
            ConfirmationCode.From("ABC123"),
            usersCounter,
            userDetailsProvider);

        AssertBrokenRule<ConfirmationCodeMustMatchRule>(() =>
            passwordResetRequest.Confirm(ConfirmationCode.From("INVALID")));
    }
}
