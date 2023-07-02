using App.Modules.Accounts.Application.Contracts;

namespace App.Modules.Accounts.Application.Accounts.CashAccounts.AddNewCashAccount;

public class AddNewCashAccountCommand : CommandBase<Guid>
{
    public Guid UserId { get; }

    public string Title { get; }

    public string Currency { get; }

    public int Balance { get; }

    public AddNewCashAccountCommand(Guid userId, string title, string currency, int balance)
    {
        UserId = userId;
        Title = title;
        Currency = currency;
        Balance = balance;
    }
}
