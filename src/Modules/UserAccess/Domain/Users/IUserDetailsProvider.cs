namespace App.Modules.UserAccess.Domain.Users;

public interface IUserDetailsProvider
{
    public UserId ProvideUserIdByEmail(string email);
}
