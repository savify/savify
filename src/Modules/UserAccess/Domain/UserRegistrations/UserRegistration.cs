using App.BuildingBlocks.Domain;
using App.Modules.UserAccess.Domain.UserRegistrations.Events;
using App.Modules.UserAccess.Domain.UserRegistrations.Rules;
using App.Modules.UserAccess.Domain.UserRegistrations.Specifications;
using App.Modules.UserAccess.Domain.Users;
using App.Modules.UserAccess.Domain.Users.Rules;

namespace App.Modules.UserAccess.Domain.UserRegistrations;

public class UserRegistration : Entity, IAggregateRoot
{
    public UserRegistrationId Id { get; private set; }

    private string _email;
    
    private string _password;

    private string _name;

    private ConfirmationCode _confirmationCode;
    
    private UserRegistrationStatus _status;
    
    private Language _preferredLanguage;

    private DateTime _createdAt;

    private DateTime? _confirmedAt = null;

    private DateTime? _renewedAt = null;

    public static UserRegistration RegisterNewUser(
        string email,
        string password,
        string name,
        Language preferredLanguage,
        ConfirmationCode confirmationCode,
        IUsersCounter usersCounter
        )
    {
        CheckRules(new UserEmailMustBeUniqueRule(usersCounter, email));

        return new UserRegistration(email, password, name, preferredLanguage, confirmationCode);
    }

    public User CreateUser()
    {
        CheckRules(new UserRegistrationMustBeConfirmedRule(_status));
        
        return User.CreateFromUserRegistration(Id, _email, _password, _name, _preferredLanguage);
    }
    
    public void Confirm(ConfirmationCode confirmationCode, ConfirmationDateSpecification specification)
    {
        CheckRules(new UserRegistrationCannotBeConfirmedMoreThanOnceRule(_status));
        
        if (!specification.IsSatisfiedBy(_createdAt))
        {
            if (_renewedAt == null || !specification.IsSatisfiedBy((DateTime) _renewedAt))
            {
                Expire();   
            }
        }
        
        CheckRules(
            new UserRegistrationCannotBeConfirmedAfterExpirationRule(_status),
            new ConfirmationCodeMustMatchRule(confirmationCode, _confirmationCode)
        );
        
        _status = UserRegistrationStatus.Confirmed;
        _confirmedAt = DateTime.UtcNow;
        
        AddDomainEvent(new UserRegistrationConfirmedDomainEvent(Id, _email));
    }
    
    public void Renew(ConfirmationCode confirmationCode)
    {
        CheckRules(new UserRegistrationCannotBeRenewedWhenAlreadyConfirmedRule(_status));
        
        _status = UserRegistrationStatus.WaitingForConfirmation;
        _confirmationCode = confirmationCode;
        _renewedAt = DateTime.UtcNow;
        
        AddDomainEvent(new UserRegistrationRenewedDomainEvent(
            Id,
            _email,
            _name,
            _preferredLanguage,
            _confirmationCode));
    }
    
    public void Expire()
    {
        CheckRules(
            new UserRegistrationCannotBeExpiredMoreThanOnceRule(_status),
            new UserRegistrationCannotBeExpiredWhenAlreadyConfirmedRule(_status)
        );
        
        _status = UserRegistrationStatus.Expired;
        
        AddDomainEvent(new UserRegistrationExpiredDomainEvent(Id));
    }

    private UserRegistration(
        string email,
        string password,
        string name,
        Language preferredLanguage,
        ConfirmationCode confirmationCode)
    {
        Id = new UserRegistrationId(Guid.NewGuid());
        _email = email;
        _password = password;
        _name = name;
        _preferredLanguage = preferredLanguage;
        _confirmationCode = confirmationCode;
        _status = UserRegistrationStatus.WaitingForConfirmation;
        _createdAt = DateTime.UtcNow;
        
        AddDomainEvent(new NewUserRegisteredDomainEvent(Id, _email, _name, preferredLanguage, _confirmationCode));
    }

    private UserRegistration() {}
}
