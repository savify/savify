using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.UserAccess.Domain.UserRegistrations;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.UserAccess.Infrastructure.Domain.UserRegistrations;

public class UserRegistrationRepository : IUserRegistrationRepository
{
    private readonly UserAccessContext _userAccessContext;

    public UserRegistrationRepository(UserAccessContext userAccessContext)
    {
        _userAccessContext = userAccessContext;
    }

    public async Task AddAsync(UserRegistration userRegistration)
    {
        await _userAccessContext.AddAsync(userRegistration);
    }

    public async Task<UserRegistration> GetByIdAsync(UserRegistrationId id)
    {
        var userRegistration = await _userAccessContext.UserRegistrations.FirstOrDefaultAsync(x => x.Id == id);

        if (userRegistration == null)
        {
            throw new NotFoundRepositoryException<UserRegistration>(id.Value);
        }

        return userRegistration;
    }
}
