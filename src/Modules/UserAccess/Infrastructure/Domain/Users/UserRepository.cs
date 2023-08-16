using App.BuildingBlocks.Infrastructure.Exceptions;
using App.Modules.UserAccess.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.UserAccess.Infrastructure.Domain.Users;

public class UserRepository : IUserRepository
{
    private readonly UserAccessContext _userAccessContext;

    public UserRepository(UserAccessContext userAccessContext)
    {
        _userAccessContext = userAccessContext;
    }

    public async Task AddAsync(User user)
    {
        await _userAccessContext.AddAsync(user);
    }

    public async Task<User> GetByIdAsync(UserId id)
    {
        var user = await _userAccessContext.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
        {
            throw new NotFoundRepositoryException<User>(id.Value);
        }

        return user;
    }
}
