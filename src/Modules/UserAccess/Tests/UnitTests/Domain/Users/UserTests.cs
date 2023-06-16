using App.Modules.UserAccess.Domain.Users;
using App.Modules.UserAccess.Domain.Users.Events;
using App.Modules.UserAccess.Domain.Users.Rules;

namespace App.Modules.UserAccess.UnitTests.Domain.Users;

[TestFixture]
public class UserTests : UnitTestBase
{
    [Test]
    public void CreateUser_WithUniqueEmail_IsSuccessful()
    {
        var usersCounter = Substitute.For<IUsersCounter>();
        usersCounter.CountUsersWithEmail("test@email.com").Returns(0);

        var user = User.Create(
            "test@email.com",
            "hashed_password",
            "Test",
            UserRole.User, 
            Country.From("PL"),
            usersCounter);
        
        var userCreatedDomainEvent = AssertPublishedDomainEvent<UserCreatedDomainEvent>(user);
        Assert.That(userCreatedDomainEvent.UserId, Is.EqualTo(user.Id));
        Assert.That(userCreatedDomainEvent.Email, Is.EqualTo("test@email.com"));
        Assert.That(userCreatedDomainEvent.Name, Is.EqualTo("Test"));
        Assert.That(userCreatedDomainEvent.UserRole, Is.EqualTo(UserRole.User));
        Assert.That(userCreatedDomainEvent.Country, Is.EqualTo(Country.From("PL")));
        Assert.That(userCreatedDomainEvent.PreferredLanguage, Is.EqualTo(Language.From("en")));
    }

    [Test]
    public void CreateUser_WithExistingEmail_BreaksUserEmailMustBeUniqueRule()
    {
        var usersCounter = Substitute.For<IUsersCounter>();
        usersCounter.CountUsersWithEmail("test@email.com").Returns(1);

        AssertBrokenRule<UserEmailMustBeUniqueRule>(() => User.Create(
            "test@email.com",
            "hashed_password",
            "Test",
            UserRole.User, 
            Country.From("PL"),
            usersCounter));
    }
    
    [Test]
    public void SettingNewPassword_IsSuccessful()
    {
        var usersCounter = Substitute.For<IUsersCounter>();
        usersCounter.CountUsersWithEmail("test@email.com").Returns(0);

        var user = User.Create(
            "test@email.com",
            "hashed_password",
            "Test",
            UserRole.User, 
            Country.From("PL"),
            usersCounter);
        
        user.SetNewPassword("hashed_password");
        
        var newPasswordWasSetDomainEvent = AssertPublishedDomainEvent<NewPasswordWasSetDomainEvent>(user);
        Assert.That(newPasswordWasSetDomainEvent.UserId, Is.EqualTo(user.Id));
        Assert.That(newPasswordWasSetDomainEvent.Email, Is.EqualTo("test@email.com"));
        Assert.That(newPasswordWasSetDomainEvent.Name, Is.EqualTo("Test"));
        Assert.That(newPasswordWasSetDomainEvent.PreferredLanguage, Is.EqualTo(Language.From("en")));
    }
}
