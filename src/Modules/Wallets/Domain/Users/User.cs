using App.BuildingBlocks.Domain;

namespace App.Modules.Wallets.Domain.Users;

public class User : Entity, IAggregateRoot
{
    public UserId UserId { get; private set; }

    private string _email;
}
