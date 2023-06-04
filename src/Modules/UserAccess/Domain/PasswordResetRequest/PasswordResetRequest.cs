using App.BuildingBlocks.Domain;
using App.Modules.UserAccess.Domain.PasswordResetRequest.Events;
using App.Modules.UserAccess.Domain.PasswordResetRequest.Rules;
using App.Modules.UserAccess.Domain.Users;

namespace App.Modules.UserAccess.Domain.PasswordResetRequest;

public class PasswordResetRequest : Entity, IAggregateRoot
{
    private static readonly TimeSpan ValidTimeSpan = new(0, 30, 0);
    
    public PasswordResetRequestId Id { get; private set; }

    private string _userEmail;

    private ConfirmationCode _confirmationCode;

    private PasswordResetRequestStatus _status;

    private DateTime _createdAt;

    private DateTime _validTill;

    private DateTime? _confirmedAt = null;

    public static PasswordResetRequest Create(string userEmail, ConfirmationCode confirmationCode, IUsersCounter usersCounter)
    {
        CheckRules(new UserWithGivenEmailMustExistRule(userEmail, usersCounter));
        
        return new PasswordResetRequest(userEmail, confirmationCode);
    }

    public void Confirm(ConfirmationCode confirmationCode)
    {
        CheckRules(
            new PasswordResetRequestCannotBeConfirmedMoreThanOnceRule(_status),
            new PasswordResetRequestCannotBeConfirmedAfterExpirationRule(_validTill),
            new ConfirmationCodeMustMatchRule(confirmationCode, _confirmationCode));
        
        _status = PasswordResetRequestStatus.Confirmed;
        _confirmedAt = DateTime.UtcNow;
        
        AddDomainEvent(new PasswordResetRequestConfirmedDomainEvent(Id));
    }

    public UserId GetUserId(IUserDetailsProvider userDetailsProvider)
    {
        return userDetailsProvider.ProvideUserIdByEmail(_userEmail);
    }

    private PasswordResetRequest(string userEmail, ConfirmationCode confirmationCode)
    {
        Id = new PasswordResetRequestId(Guid.NewGuid());
        
        _userEmail = userEmail;
        _confirmationCode = confirmationCode;
        _status = PasswordResetRequestStatus.WaitingForConfirmation;
        _createdAt = DateTime.UtcNow;
        _validTill = DateTime.UtcNow.Add(ValidTimeSpan);
        
        AddDomainEvent(new PasswordResetRequestedDomainEvent(_userEmail, _confirmationCode, _validTill));
    }

    private PasswordResetRequest() {}
}
