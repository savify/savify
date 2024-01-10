using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets.Events;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets.Rules;
using App.Modules.FinanceTracking.Domain.Wallets.Events;

namespace App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;

public class CreditWallet : Wallet, IAggregateRoot
{
    public UserId UserId { get; private set; }

    private string _title;

    private int _initialAvailableBalance;

    private int _availableBalance;

    private int _creditLimit;

    private Currency _currency;

    private bool _isRemoved;

    public int AvailableBalance => _availableBalance;

    public int CreditLimit => _creditLimit;

    public static CreditWallet AddNew(UserId userId, string title, Currency currency, int creditLimit, int initialAvailableBalance)
    {
        return new CreditWallet(userId, title, currency, creditLimit, initialAvailableBalance);
    }

    public void ChangeTitle(string newTitle)
    {
        CheckRules(new CreditWalletCannotBeChangedIfWasRemovedRule(Id, _isRemoved));

        _title = newTitle;
    }

    public void ChangeCreditLimit(int newCreditLimit)
    {
        // TODO: check if available balance is not greater than credit limit
        CheckRules(new CreditWalletCannotBeChangedIfWasRemovedRule(Id, _isRemoved));

        _creditLimit = newCreditLimit;
    }

    public void ChangeAvailableBalance(int newAvailableBalance)
    {
        if (newAvailableBalance < _availableBalance)
        {
            DecreaseBalance(Money.From(_availableBalance - newAvailableBalance, _currency));
        }
        else
        {
            IncreaseBalance(Money.From(newAvailableBalance - _availableBalance, _currency));
        }
    }

    public override void IncreaseBalance(Money amount)
    {
        // TODO: check if available balance is not greater than credit limit
        CheckRules(new CreditWalletCannotBeChangedIfWasRemovedRule(Id, _isRemoved));

        _availableBalance += amount.Amount;

        AddDomainEvent(new WalletBalanceIncreasedDomainEvent(Id, amount, _availableBalance));
    }

    public override void DecreaseBalance(Money amount)
    {
        // TODO: check if available balance is not greater than credit limit
        CheckRules(new CreditWalletCannotBeChangedIfWasRemovedRule(Id, _isRemoved));

        _availableBalance -= amount.Amount;

        AddDomainEvent(new WalletBalanceDecreasedDomainEvent(Id, amount, _availableBalance));
    }

    public void Remove()
    {
        CheckRules(new CreditWalletCannotBeRemovedMoreThanOnceRule(Id, _isRemoved));

        _isRemoved = true;

        AddDomainEvent(new CreditWalletRemovedDomainEvent(Id, UserId));
    }

    public new void Load(IEnumerable<IDomainEvent> history)
    {
        _availableBalance = _initialAvailableBalance;

        base.Load(history);
    }

    protected override void Apply(IDomainEvent @event)
    {
        this.When((dynamic)@event);
    }

    private void When(WalletBalanceIncreasedDomainEvent @event)
    {
        _availableBalance += @event.Amount.Amount;
    }

    private void When(WalletBalanceDecreasedDomainEvent @event)
    {
        _availableBalance -= @event.Amount.Amount;
    }

    private CreditWallet(UserId userId, string title, Currency currency, int creditLimit, int initialAvailableBalance)
    {
        Id = new WalletId(Guid.NewGuid());
        UserId = userId;
        _title = title;
        _initialAvailableBalance = initialAvailableBalance;
        _availableBalance = initialAvailableBalance;
        _creditLimit = creditLimit;
        _currency = currency;

        AddDomainEvent(new CreditWalletAddedDomainEvent(Id, UserId, _currency));
    }

    private CreditWallet() { }
}
