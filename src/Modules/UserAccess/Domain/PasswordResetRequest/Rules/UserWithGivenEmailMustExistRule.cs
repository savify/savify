using App.BuildingBlocks.Domain;
using App.Modules.UserAccess.Domain.Users;

namespace App.Modules.UserAccess.Domain.PasswordResetRequest.Rules;

public class UserWithGivenEmailMustExistRule : IBusinessRule
{
    private readonly string _userEmail;

    private readonly IUsersCounter _usersCounter;

    public UserWithGivenEmailMustExistRule(string userEmail, IUsersCounter usersCounter)
    {
        _userEmail = userEmail;
        _usersCounter = usersCounter;
    }

    public bool IsBroken() => _usersCounter.CountUsersWithEmail(_userEmail) == 0;

    public string MessageTemplate => "User with email '{0}' does not exist";

    public object[] MessageArguments => new object[] { _userEmail };
}
