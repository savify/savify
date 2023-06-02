namespace App.Modules.UserAccess.Domain.Users;

public interface IUsersCounter
{
    int CountUsersWithEmail(string email);
}
