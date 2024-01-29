using App.BuildingBlocks.Domain;
using App.Modules.UserAccess.Domain.UserRegistrations.Events;
using App.Modules.UserAccess.Domain.UserRegistrations.Rules;
using App.Modules.UserAccess.Domain.Users;
using App.Modules.UserAccess.Domain.Users.Rules;

namespace App.Modules.UserAccess.Domain.UserRegistrations;

public class UserRegistration : Entity, IAggregateRoot
{
    private static readonly TimeSpan ValidTimeSpan = new(0, 30, 0);

    public UserRegistrationId Id { get; private set; }

    private string _email;

    private string _password;

    private string _name;

    private ConfirmationCode _confirmationCode;

    private UserRegistrationStatus _status;

    private Country _country;

    private Language _preferredLanguage;

    private DateTime _createdAt;

    private DateTime _validTill;

    private DateTime? _confirmedAt;

    public static UserRegistration RegisterNewUser(
        string email,
        string password,
        string name,
        Country country,
        Language preferredLanguage,
        ConfirmationCode confirmationCode,
        IUsersCounter usersCounter
        )
    {
        CheckRules(new UserEmailMustBeUniqueRule(usersCounter, email));

        return new UserRegistration(email, password, name, country, preferredLanguage, confirmationCode);
    }

    public User CreateUser()
    {
        CheckRules(new UserRegistrationMustBeConfirmedRule(_status));

        return User.CreateFromUserRegistration(Id, _email, _password, _name, _country, _preferredLanguage);
    }

    public void Confirm(ConfirmationCode confirmationCode)
    {
        CheckRules(
            new UserRegistrationCannotBeConfirmedMoreThanOnceRule(_status),
            new UserRegistrationCannotBeConfirmedAfterExpirationRule(_validTill),
            new ConfirmationCodeMustMatchRule(confirmationCode, _confirmationCode)
        );

        _status = UserRegistrationStatus.Confirmed;
        _confirmedAt = DateTime.UtcNow;

        AddDomainEvent(new UserRegistrationConfirmedDomainEvent(Id, _email, _name, _country, _preferredLanguage));
    }

    public void Renew(ConfirmationCode confirmationCode)
    {
        CheckRules(new UserRegistrationCannotBeRenewedWhenAlreadyConfirmedRule(_status));

        _status = UserRegistrationStatus.WaitingForConfirmation;
        _confirmationCode = confirmationCode;
        _validTill = DateTime.UtcNow.Add(ValidTimeSpan);

        AddDomainEvent(new UserRegistrationRenewedDomainEvent(
            Id,
            _email,
            _name,
            _preferredLanguage,
            _confirmationCode));
    }

    private UserRegistration(
        string email,
        string password,
        string name,
        Country country,
        Language preferredLanguage,
        ConfirmationCode confirmationCode)
    {
        Id = new UserRegistrationId(Guid.NewGuid());
        _email = email;
        _password = password;
        _name = name;
        _country = country;
        _preferredLanguage = preferredLanguage;
        _confirmationCode = confirmationCode;
        _status = UserRegistrationStatus.WaitingForConfirmation;
        _createdAt = DateTime.UtcNow;
        _validTill = DateTime.UtcNow.Add(ValidTimeSpan);

        AddDomainEvent(new NewUserRegisteredDomainEvent(Id, _email, _name, country, preferredLanguage, _confirmationCode));
    }

    private UserRegistration() { }
}
