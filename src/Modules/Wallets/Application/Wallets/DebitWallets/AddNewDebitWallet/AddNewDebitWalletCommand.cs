using App.Modules.Wallets.Application.Contracts;

namespace App.Modules.Wallets.Application.Wallets.DebitWallets.AddNewDebitWallet;

public class AddNewDebitWalletCommand : CommandBase<Guid>
{
    public Guid UserId { get; }

    public string Title { get; }

    public string Currency { get; }

    public int Balance { get; }

    public AddNewDebitWalletCommand(Guid userId, string title, string currency, int balance)
    {
        UserId = userId;
        Title = title;
        Currency = currency;
        Balance = balance;
    }
}
