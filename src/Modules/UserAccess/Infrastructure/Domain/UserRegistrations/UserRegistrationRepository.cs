using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.UserAccess.Domain.UserRegistrations;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.UserAccess.Infrastructure.Domain.UserRegistrations;

public class UserRegistrationRepository(UserAccessContext userAccessContext) : IUserRegistrationRepository
{
    public async Task AddAsync(UserRegistration userRegistration)
    {
        await userAccessContext.AddAsync(userRegistration);
    }

    public async Task<UserRegistration> GetByIdAsync(UserRegistrationId id)
    {
        var userRegistration = userAccessContext.UserRegistrations.Local.SingleOrDefault(x => x.Id == id) ??
                               await userAccessContext.UserRegistrations.SingleOrDefaultAsync(x => x.Id == id);

        if (userRegistration == null)
        {
            throw new NotFoundRepositoryException<UserRegistration>(id.Value);
        }

        return userRegistration;
    }
}
