namespace App.Modules.UserAccess.Domain.Users;

public interface IUserRepository
{
    Task AddAsync(User user);

    Task<User> GetByIdAsync(UserId id);
}
