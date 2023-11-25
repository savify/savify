using App.Modules.UserAccess.Application.Contracts;

namespace App.Modules.UserAccess.Application.UserRegistrations.GetUserRegistration;

public class GetUserRegistrationQuery : QueryBase<UserRegistrationDto?>
{
    public Guid UserRegistrationId { get; }

    public GetUserRegistrationQuery(Guid userRegistrationId)
    {
        UserRegistrationId = userRegistrationId;
    }
}
