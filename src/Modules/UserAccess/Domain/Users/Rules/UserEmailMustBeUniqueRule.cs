using App.BuildingBlocks.Domain;

namespace App.Modules.UserAccess.Domain.Users.Rules;

public class UserEmailMustBeUniqueRule : IBusinessRule
{
    private readonly IUsersCounter _usersCounter;

    private readonly string _email;

    internal UserEmailMustBeUniqueRule(IUsersCounter usersCounter, string email)
    {
        _usersCounter = usersCounter;
        _email = email;
    }

    public bool IsBroken() => _usersCounter.CountUsersWithEmail(_email) > 0;

    public string MessageTemplate => "User with email '{0}' already exists";

    public object[] MessageArguments => new object[] { _email };
}
