using App.Modules.UserAccess.Application.Contracts;

namespace App.Modules.UserAccess.Application.UserRegistrations.GetUserRegistration;

public class GetUserRegistrationQuery(Guid userRegistrationId) : QueryBase<UserRegistrationDto?>
{
    public Guid UserRegistrationId { get; } = userRegistrationId;
}
