using App.Modules.Banks.Domain.Users;

namespace App.Modules.Banks.Domain.BanksSynchronisationProcessing;

public class BanksSynchronisationProcessInitiator
{
    public BanksSynchronisationProcessInitiatorType Type { get; }

    public UserId? UserId { get; }

    public static BanksSynchronisationProcessInitiator InternalCommand => new(BanksSynchronisationProcessInitiatorType.InternalCommand);

    public static BanksSynchronisationProcessInitiator User(UserId id) => new(BanksSynchronisationProcessInitiatorType.User, id);

    public BanksSynchronisationProcessInitiator(BanksSynchronisationProcessInitiatorType type, UserId? userId = null)
    {
        Type = type;
        UserId = userId;
    }

    private BanksSynchronisationProcessInitiator() { }
}
