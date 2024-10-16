using App.BuildingBlocks.Domain;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Services;
using App.Modules.FinanceTracking.Domain.BankConnections;
using App.Modules.FinanceTracking.Domain.BankConnections.BankAccounts;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.BankAccountConnections;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets.Events;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets.Rules;
using App.Modules.FinanceTracking.Domain.Wallets.Events;
using App.Modules.FinanceTracking.Domain.Wallets.ManualBalanceChanges;
using App.Modules.FinanceTracking.Domain.Wallets.Rules;

namespace App.Modules.FinanceTracking.Domain.Wallets.DebitWallets;

public class DebitWallet : Wallet, IAggregateRoot
{
    private string _title;

    private int _initialBalance;

    private int _balance;

    private BankAccountConnection? _bankAccountConnection;

    private bool _isRemoved;

    public int Balance => _balance;

    public static DebitWallet AddNew(UserId userId, string title, Currency currency, int initialBalance = 0)
    {
        return new DebitWallet(userId, title, currency, initialBalance);
    }

    public void ChangeTitle(string newTitle)
    {
        CheckRules(new DebitWalletCannotBeChangedIfWasRemovedRule(Id, _isRemoved));

        _title = newTitle;
    }

    public void ChangeBalance(int newBalance)
    {
        CheckRules(new WalletFinanceDetailsCannotBeChangedIfBankAccountIsConnectedRule(newBalance, HasConnectedBankAccount));

        if (newBalance < _balance)
        {
            var amount = Money.From(_balance - newBalance, Currency);

            DecreaseBalance(amount);
            AddManualBalanceChange(amount, ManualBalanceChangeType.Decrease);
        }
        else
        {
            var amount = Money.From(newBalance - _balance, Currency);

            IncreaseBalance(amount);
            AddManualBalanceChange(amount, ManualBalanceChangeType.Increase);
        }
    }

    public override void IncreaseBalance(Money amount)
    {
        CheckRules(new DebitWalletCannotBeChangedIfWasRemovedRule(Id, _isRemoved),
            new BalanceChangeAmountMustBeInTheWalletCurrencyRule(amount, Currency));

        _balance += amount.Amount;

        AddDomainEvent(new WalletBalanceIncreasedDomainEvent(Id, amount, _balance));
    }

    public override void DecreaseBalance(Money amount)
    {
        CheckRules(new DebitWalletCannotBeChangedIfWasRemovedRule(Id, _isRemoved),
            new BalanceChangeAmountMustBeInTheWalletCurrencyRule(amount, Currency));

        _balance -= amount.Amount;

        AddDomainEvent(new WalletBalanceDecreasedDomainEvent(Id, amount, _balance));
    }

    public void Remove()
    {
        CheckRules(new DebitWalletCannotBeRemovedMoreThanOnceRule(Id, _isRemoved));

        _isRemoved = true;

        AddDomainEvent(new DebitWalletRemovedDomainEvent(Id, UserId));
    }

    public async Task<BankConnectionProcess> InitiateBankConnectionProcess(BankId bankId, IBankConnectionProcessInitiationService initiationService)
    {
        CheckRules(new BankConnectionProcessCannotBeInitiatedIfBankAccountIsAlreadyConnectedRule(HasConnectedBankAccount));

        return await BankConnectionProcess.Initiate(UserId, bankId, Id, WalletType.Debit, initiationService);
    }

    public void ConnectBankAccount(BankConnectionId bankConnectionId, BankAccountId bankAccountId, int balance, Currency currency)
    {
        CheckRules(new BankAccountCannotBeConnectedToWalletIfItAlreadyHasBankAccountConnectedRule(HasConnectedBankAccount));

        ChangeBalance(balance);
        _bankAccountConnection = new BankAccountConnection(bankConnectionId, bankAccountId);
        Currency = currency;

        AddDomainEvent(new BankAccountWasConnectedToDebitWalletDomainEvent(Id, UserId, bankConnectionId, bankAccountId));
    }

    public bool HasConnectedBankAccount => _bankAccountConnection is not null;

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

    private DebitWallet(UserId userId, string title, Currency currency, int initialBalance)
    {
        Id = new WalletId(Guid.NewGuid());
        UserId = userId;
        _title = title;
        Currency = currency;
        _initialBalance = initialBalance;
        _balance = initialBalance;

        AddDomainEvent(new DebitWalletAddedDomainEvent(Id, userId, Currency));
    }

    private DebitWallet() { }
}
