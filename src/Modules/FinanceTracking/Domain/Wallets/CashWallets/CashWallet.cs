using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets.Events;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets.Rules;
using App.Modules.FinanceTracking.Domain.Wallets.Events;

namespace App.Modules.FinanceTracking.Domain.Wallets.CashWallets;

public class CashWallet : Wallet, IAggregateRoot
{
    public UserId UserId { get; private set; }

    private string _title;

    private Currency _currency;

    private int _initialBalance;

    private int _balance;

    private bool _isRemoved;

    public int Balance => _balance;

    public static CashWallet AddNew(UserId userId, string title, Currency currency, int initialBalance = 0)
    {
        return new CashWallet(userId, title, currency, initialBalance);
    }

    public void ChangeTitle(string newTitle)
    {
        CheckRules(new CashWalletCannotBeChangedIfWasRemovedRule(Id, _isRemoved));

        _title = newTitle;
    }

    public void ChangeBalance(int newBalance)
    {
        CheckRules(new CashWalletCannotBeChangedIfWasRemovedRule(Id, _isRemoved));

        if (newBalance < _balance)
        {
            AddDomainEvent(new WalletBalanceDecreasedDomainEvent(Id, Money.From(_balance - newBalance, _currency), newBalance));
        }
        else
        {
            AddDomainEvent(new WalletBalanceIncreasedDomainEvent(Id, Money.From(newBalance - _balance, _currency), newBalance));
        }

        _balance = newBalance;
    }

    public void IncreaseBalance(Money amount)
    {
        CheckRules(new CashWalletCannotBeChangedIfWasRemovedRule(Id, _isRemoved));

        _balance += amount.Amount;

        AddDomainEvent(new WalletBalanceIncreasedDomainEvent(Id, amount, _balance));
    }

    public void DecreaseBalance(Money amount)
    {
        CheckRules(new CashWalletCannotBeChangedIfWasRemovedRule(Id, _isRemoved));

        _balance -= amount.Amount;

        AddDomainEvent(new WalletBalanceDecreasedDomainEvent(Id, amount, _balance));
    }

    public void Remove()
    {
        CheckRules(new CashWalletCannotBeRemovedMoreThanOnceRule(Id, _isRemoved));

        _isRemoved = true;

        AddDomainEvent(new CashWalletRemovedDomainEvent(Id, UserId));
    }

    public new void Load(IEnumerable<IDomainEvent> history)
    {
        _balance = _initialBalance;

        base.Load(history);
    }

    protected override void Apply(IDomainEvent @event)
    {
        this.When((dynamic)@event);
    }

    private void When(WalletBalanceIncreasedDomainEvent @event)
    {
        _balance += @event.Amount.Amount;
    }

    private void When(WalletBalanceDecreasedDomainEvent @event)
    {
        _balance -= @event.Amount.Amount;
    }

    private CashWallet(UserId userId, string title, Currency currency, int initialBalance)
    {
        Id = new WalletId(Guid.NewGuid());
        UserId = userId;
        _title = title;
        _currency = currency;
        _initialBalance = initialBalance;
        _balance = initialBalance;

        AddDomainEvent(new CashWalletAddedDomainEvent(Id, UserId, _currency));
    }

    private CashWallet() { }
}
