using App.Modules.Accounts.Application.Contracts;

namespace App.Modules.Accounts.Application.CreditAccounts.AddNewCreditAccount;
public class AddNewCreditAccountCommand : CommandBase<Guid>
{
    public Guid UserId { get; }
    public string Title { get; }
    public int AvailableBalance { get; }
    public int CreditLimit { get; }
    public string Currency { get; }

    public AddNewCreditAccountCommand(Guid userId, string title, int availableBalance, int creditLimit, string currency)
    {
        UserId = userId;
        Title = title;
        AvailableBalance = availableBalance;
        CreditLimit = creditLimit;
        Currency = currency;
    }
}
