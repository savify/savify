using App.Modules.UserAccess.Domain.UserRegistrations;
using App.Modules.UserAccess.Domain.UserRegistrations.Events;
using App.Modules.UserAccess.Domain.UserRegistrations.Rules;
using App.Modules.UserAccess.Domain.Users;
using App.Modules.UserAccess.Domain.Users.Events;
using App.Modules.UserAccess.Domain.Users.Rules;
using App.Modules.UserAccess.Domain;

namespace App.Modules.UserAccess.UnitTests.Domain.UserRegistrations;

[TestFixture]
public class UserRegistrationsTests : UnitTestBase
{
    [Test]
    public void NewUserRegistration_WithUniqueEmail_IsSuccessful()
    {
        var usersCounter = Substitute.For<IUsersCounter>();
        usersCounter.CountUsersWithEmail("test@email.com").Returns(0);

        var userRegistration = UserRegistration.RegisterNewUser(
            "test@email.com",
            "password",
            "Name",
            Language.From("en"),
            ConfirmationCode.From("ABC123"),
            usersCounter);

        var newUserRegisteredDomainEvent = AssertPublishedDomainEvent<NewUserRegisteredDomainEvent>(userRegistration);
        Assert.That(newUserRegisteredDomainEvent.UserRegistrationId, Is.EqualTo(userRegistration.Id));
        Assert.That(newUserRegisteredDomainEvent.Email, Is.EqualTo("test@email.com"));
        Assert.That(newUserRegisteredDomainEvent.Name, Is.EqualTo("Name"));
        Assert.That(newUserRegisteredDomainEvent.PreferredLanguage, Is.EqualTo(Language.From("en")));
        Assert.That(newUserRegisteredDomainEvent.ConfirmationCode, Is.EqualTo(ConfirmationCode.From("ABC123")));
    }
    
    [Test]
    public void NewUserRegistration_WithExistingEmail_BreaksUserEmailMustBeUniqueRule()
    {
        var usersCounter = Substitute.For<IUsersCounter>();
        usersCounter.CountUsersWithEmail("test@email.com").Returns(1);

        AssertBrokenRule<UserEmailMustBeUniqueRule>(() =>
        {
            UserRegistration.RegisterNewUser(
                "test@email.com",
                "password",
                "Name",
                Language.From("en"),
                ConfirmationCode.From("ABC123"),
                usersCounter); 
        });
    }
    
    [Test]
    public void ConfirmingUserRegistration_WhenWaitingForConfirmation_IsSuccessful()
    {
        var usersCounter = Substitute.For<IUsersCounter>();
        usersCounter.CountUsersWithEmail("test@email.com").Returns(0);

        var userRegistration = UserRegistration.RegisterNewUser(
            "test@email.com",
            "password",
            "Name",
            Language.From("en"),
            ConfirmationCode.From("ABC123"),
            usersCounter);
        
        userRegistration.Confirm(ConfirmationCode.From("ABC123"));

        var userRegistrationConfirmedDomainEvent = AssertPublishedDomainEvent<UserRegistrationConfirmedDomainEvent>(userRegistration);
        Assert.That(userRegistrationConfirmedDomainEvent.UserRegistrationId, Is.EqualTo(userRegistration.Id));
        Assert.That(userRegistrationConfirmedDomainEvent.Email, Is.EqualTo("test@email.com"));
        Assert.That(userRegistrationConfirmedDomainEvent.Name, Is.EqualTo("Name"));
        Assert.That(userRegistrationConfirmedDomainEvent.PreferredLanguage, Is.EqualTo(Language.From("en")));
    }
    
    [Test]
    public void UserRegistration_WhenIsConfirmed_CannotBeConfirmedAgain()
    {
        var usersCounter = Substitute.For<IUsersCounter>();
        usersCounter.CountUsersWithEmail("test@email.com").Returns(0);

        var userRegistration = UserRegistration.RegisterNewUser(
            "test@email.com",
            "password",
            "Name",
            Language.From("en"),
            ConfirmationCode.From("ABC123"),
            usersCounter);
        
        userRegistration.Confirm(ConfirmationCode.From("ABC123"));

        AssertBrokenRule<UserRegistrationCannotBeConfirmedMoreThanOnceRule>(() =>
        {
            userRegistration.Confirm(ConfirmationCode.From("ABC123"));
        });
    }

    [Test]
    public void ConfirmingUserRegistration_WithInvalidConfirmationCode_WillFail()
    {
        var usersCounter = Substitute.For<IUsersCounter>();
        usersCounter.CountUsersWithEmail("test@email.com").Returns(0);

        var userRegistration = UserRegistration.RegisterNewUser(
            "test@email.com",
            "password",
            "Name",
            Language.From("en"),
            ConfirmationCode.From("ABC123"),
            usersCounter);

        AssertBrokenRule<ConfirmationCodeMustMatchRule>(() =>
        {
            userRegistration.Confirm(ConfirmationCode.From("INVALID"));
        });
    }

    [Test]
    public void RenewingUserRegistration_WhenNotConfirmed_IsSuccessful()
    {
        var usersCounter = Substitute.For<IUsersCounter>();
        usersCounter.CountUsersWithEmail("test@email.com").Returns(0);

        var userRegistration = UserRegistration.RegisterNewUser(
            "test@email.com",
            "password",
            "Name",
            Language.From("en"),
            ConfirmationCode.From("ABC123"),
            usersCounter);

        userRegistration.Renew(ConfirmationCode.From("NEW123"));
        
        var userRegistrationRenewedDomainEvent = AssertPublishedDomainEvent<UserRegistrationRenewedDomainEvent>(userRegistration);
        Assert.That(userRegistrationRenewedDomainEvent.UserRegistrationId, Is.EqualTo(userRegistration.Id));
        Assert.That(userRegistrationRenewedDomainEvent.Email, Is.EqualTo("test@email.com"));
        Assert.That(userRegistrationRenewedDomainEvent.Name, Is.EqualTo("Name"));
        Assert.That(userRegistrationRenewedDomainEvent.PreferredLanguage, Is.EqualTo(Language.From("en")));
        Assert.That(userRegistrationRenewedDomainEvent.ConfirmationCode, Is.EqualTo(ConfirmationCode.From("NEW123")));
    }
    
    [Test]
    public void RenewingUserRegistration_WhenIsConfirmed_CannotBeRenewed()
    {
        var usersCounter = Substitute.For<IUsersCounter>();
        usersCounter.CountUsersWithEmail("test@email.com").Returns(0);

        var userRegistration = UserRegistration.RegisterNewUser(
            "test@email.com",
            "password",
            "Name",
            Language.From("en"),
            ConfirmationCode.From("ABC123"),
            usersCounter);
        
        userRegistration.Confirm(ConfirmationCode.From("ABC123"));
        
        AssertBrokenRule<UserRegistrationCannotBeRenewedWhenAlreadyConfirmedRule>(() =>
        {
            userRegistration.Renew(ConfirmationCode.From("ABC123"));
        });
    }
    
    [Test]
    public void CreatingUser_WhenRegistrationIsConfirmed_WillCreateNewUser()
    {
        var usersCounter = Substitute.For<IUsersCounter>();
        usersCounter.CountUsersWithEmail("test@email.com").Returns(0);

        var userRegistration = UserRegistration.RegisterNewUser(
            "test@email.com",
            "password",
            "Name",
            Language.From("en"),
            ConfirmationCode.From("ABC123"),
            usersCounter);
        
        userRegistration.Confirm(ConfirmationCode.From("ABC123"));
        
        var user = userRegistration.CreateUser();

        var userCreatedDomainEvent = AssertPublishedDomainEvent<UserCreatedDomainEvent>(user);
        Assert.That(userCreatedDomainEvent.UserId, Is.EqualTo(user.Id));
        Assert.That(userCreatedDomainEvent.Email, Is.EqualTo("test@email.com"));
        Assert.That(userCreatedDomainEvent.Name, Is.EqualTo("Name"));
        Assert.That(userCreatedDomainEvent.PreferredLanguage, Is.EqualTo(Language.From("en")));
        Assert.That(userCreatedDomainEvent.UserRole, Is.EqualTo(UserRole.User));
    }
    
    [Test]
    public void CreatingUser_WhenRegistrationIsNotConfirmed_BreaksUserRegistrationShouldBeConfirmedRule()
    {
        var usersCounter = Substitute.For<IUsersCounter>();
        usersCounter.CountUsersWithEmail("test@email.com").Returns(0);

        var userRegistration = UserRegistration.RegisterNewUser(
            "test@email.com",
            "password",
            "Name",
            Language.From("en"),
            ConfirmationCode.From("ABC123"),
            usersCounter);

        AssertBrokenRule<UserRegistrationMustBeConfirmedRule>(() =>
        {
            userRegistration.CreateUser();
        });
    }
}
