using App.Modules.Accounts.Application.Contracts;

namespace App.Modules.Accounts.Application.Accounts.DebitAccounts.AddNewDebitAccount;

public class AddNewDebitAccountCommand : CommandBase<Guid>
{
    public Guid UserId { get; }
    public string Title { get; }
    public string Currency { get; }
    public int Balance { get; }

    public AddNewDebitAccountCommand(Guid userId, string title, string currency, int balance)
    {
        UserId = userId;
        Title = title;
        Currency = currency;
        Balance = balance;
    }
}
