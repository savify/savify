using App.Modules.Banks.Domain.Users;

namespace App.Modules.Banks.Domain.BankSynchronisationProcessing;

public record Initiator(InitiatorType Type, UserId? UserId = null)
{
    public static Initiator InternalCommand = new(InitiatorType.InternalCommand);

    public static Initiator User(UserId id) => new(InitiatorType.User, id);
}
