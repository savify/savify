using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.UserAccess.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.UserAccess.Infrastructure.Domain.Users;

public class UserRepository(UserAccessContext userAccessContext) : IUserRepository
{
    public async Task AddAsync(User user)
    {
        await userAccessContext.AddAsync(user);
    }

    public async Task<User> GetByIdAsync(UserId id)
    {
        var user = await userAccessContext.Users.SingleOrDefaultAsync(x => x.Id == id);

        if (user == null)
        {
            throw new NotFoundRepositoryException<User>(id.Value);
        }

        return user;
    }
}
