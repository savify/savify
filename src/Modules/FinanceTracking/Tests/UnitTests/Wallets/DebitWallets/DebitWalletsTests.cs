using App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Events;
using App.Modules.FinanceTracking.Domain.BankConnectionProcessing.Services;
using App.Modules.FinanceTracking.Domain.BankConnections;
using App.Modules.FinanceTracking.Domain.BankConnections.BankAccounts;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets.Events;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets.Rules;
using App.Modules.FinanceTracking.Domain.Wallets.Events;

namespace App.Modules.FinanceTracking.UnitTests.Wallets.DebitWallets;

[TestFixture]
public class DebitWalletsTests : UnitTestBase
{
    [Test]
    public void AddingDebitWallet_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = DebitWallet.AddNew(userId, "Debit", Currency.From("PLN"), 1000);

        var walletAddedDomainEvent = AssertPublishedDomainEvent<DebitWalletAddedDomainEvent>(wallet);

        Assert.That(walletAddedDomainEvent.WalletId, Is.EqualTo(wallet.Id));
        Assert.That(walletAddedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(walletAddedDomainEvent.Currency, Is.EqualTo(Currency.From("PLN")));
    }

    [Test]
    public void ChangingTitle_WhenWalletIsRemoved_BreaksDebitWalletCannotBeChangedIfWasRemovedRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = DebitWallet.AddNew(userId, "Debit", Currency.From("PLN"), 1000);
        wallet.Remove();

        AssertBrokenRule<DebitWalletCannotBeChangedIfWasRemovedRule>(() =>
        {
            wallet.ChangeTitle("New debit");
        });
    }

    [Test]
    public void ChangingBalance_WhenNewBalanceIsLower_AddsWalletBalanceDecreasedDomainEvent()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = DebitWallet.AddNew(userId, "Debit", Currency.From("PLN"), 1000);

        wallet.ChangeBalance(100);

        Assert.That(wallet.Balance, Is.EqualTo(100));

        var walletBalanceDecreasedDomainEvent = AssertPublishedDomainEvent<WalletBalanceDecreasedDomainEvent>(wallet);
        Assert.That(walletBalanceDecreasedDomainEvent.WalletId, Is.EqualTo(wallet.Id));
        Assert.That(walletBalanceDecreasedDomainEvent.Amount, Is.EqualTo(Money.From(900, Currency.From("PLN"))));
        Assert.That(walletBalanceDecreasedDomainEvent.NewBalance, Is.EqualTo(100));
    }

    [Test]
    public void ChangingBalance_WhenNewBalanceIsHigher_AddsWalletBalanceDecreasedDomainEvent()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = DebitWallet.AddNew(userId, "Debit", Currency.From("PLN"), 1000);

        wallet.ChangeBalance(2000);

        Assert.That(wallet.Balance, Is.EqualTo(2000));

        var walletBalanceIncreasedDomainEvent = AssertPublishedDomainEvent<WalletBalanceIncreasedDomainEvent>(wallet);
        Assert.That(walletBalanceIncreasedDomainEvent.WalletId, Is.EqualTo(wallet.Id));
        Assert.That(walletBalanceIncreasedDomainEvent.Amount, Is.EqualTo(Money.From(1000, Currency.From("PLN"))));
        Assert.That(walletBalanceIncreasedDomainEvent.NewBalance, Is.EqualTo(2000));
    }

    [Test]
    public void ChangingBalance_WhenWalletIsRemoved_BreaksDebitWalletCannotBeChangedIfWasRemovedRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = DebitWallet.AddNew(userId, "Debit", Currency.From("PLN"), 1000);
        wallet.Remove();

        AssertBrokenRule<DebitWalletCannotBeChangedIfWasRemovedRule>(() =>
        {
            wallet.ChangeBalance(2000);
        });
    }

    [Test]
    public void ChangingBalance_WhenBankAccountIsConnectedToWallet_BreaksWalletFinanceDetailsCannotBeEditedIfBankAccountIsConnectedRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = DebitWallet.AddNew(userId, "Debit", Currency.From("PLN"), 1000);
        wallet.ConnectBankAccount(new BankConnectionId(Guid.NewGuid()), new BankAccountId(Guid.NewGuid()), 10595, Currency.From("PLN"));

        AssertBrokenRule<WalletFinanceDetailsCannotBeChangedIfBankAccountIsConnectedRule>(() =>
        {
            wallet.ChangeBalance(2000);
        });
    }

    [Test]
    public async Task InitiatingBankAccountProcess_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = DebitWallet.AddNew(userId, "Debit", Currency.From("PLN"), 1000);

        var bankId = new BankId(Guid.NewGuid());
        var initiationService = Substitute.For<IBankConnectionProcessInitiationService>();

        var bankConnectionProcess = await wallet.InitiateBankConnectionProcess(bankId, initiationService);

        var domainEvent = AssertPublishedDomainEvent<BankConnectionProcessInitiatedDomainEvent>(bankConnectionProcess);
        Assert.That(domainEvent.BankConnectionProcessId, Is.EqualTo(bankConnectionProcess.Id));
        Assert.That(domainEvent.UserId, Is.EqualTo(wallet.UserId));
        Assert.That(domainEvent.WalletId, Is.EqualTo(wallet.Id));
        Assert.That(domainEvent.BankId, Is.EqualTo(bankId));
    }

    [Test]
    public void InitiatingBankAccountProcess_WhenBankAccountIsAlreadyConnectedToWallet_BreaksBankConnectionProcessCannotBeInitiatedIfBankAccountIsAlreadyConnectedRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = DebitWallet.AddNew(userId, "Debit", Currency.From("PLN"), 1000);
        wallet.ConnectBankAccount(new BankConnectionId(Guid.NewGuid()), new BankAccountId(Guid.NewGuid()), 10595, Currency.From("PLN"));

        AssertBrokenRuleAsync<BankConnectionProcessCannotBeInitiatedIfBankAccountIsAlreadyConnectedRule>(async Task () =>
        {
            var bankId = new BankId(Guid.NewGuid());
            var initiationService = Substitute.For<IBankConnectionProcessInitiationService>();

            await wallet.InitiateBankConnectionProcess(bankId, initiationService);
        });
    }

    [Test]
    public void ConnectingBankAccount_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = DebitWallet.AddNew(userId, "Debit", Currency.From("PLN"), 1000);

        var bankConnectionId = new BankConnectionId(Guid.NewGuid());
        var bankAccountId = new BankAccountId(Guid.NewGuid());

        wallet.ConnectBankAccount(bankConnectionId, bankAccountId, 10595, Currency.From("PLN"));
        var domainEvent = AssertPublishedDomainEvent<BankAccountWasConnectedToDebitWalletDomainEvent>(wallet);

        Assert.That(wallet.HasConnectedBankAccount, Is.True);
        Assert.That(domainEvent.WalletId, Is.EqualTo(wallet.Id));
        Assert.That(domainEvent.UserId, Is.EqualTo(wallet.UserId));
        Assert.That(domainEvent.BankConnectionId, Is.EqualTo(bankConnectionId));
        Assert.That(domainEvent.BankAccountId, Is.EqualTo(bankAccountId));
    }

    [Test]
    public void ConnectingBankAccount_WhenBankAccountIsAlreadyConnectedToWallet_BreaksBankAccountCannotBeConnectedToWalletIfItAlreadyHasBankAccountConnectedRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = DebitWallet.AddNew(userId, "Debit", Currency.From("PLN"), 1000);
        wallet.ConnectBankAccount(new BankConnectionId(Guid.NewGuid()), new BankAccountId(Guid.NewGuid()), 10595, Currency.From("PLN"));

        AssertBrokenRule<BankAccountCannotBeConnectedToWalletIfItAlreadyHasBankAccountConnectedRule>(() =>
        {
            wallet.ConnectBankAccount(new BankConnectionId(Guid.NewGuid()), new BankAccountId(Guid.NewGuid()), 111, Currency.From("USD"));
        });
    }

    [Test]
    public void RemovingDebitWallet_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = DebitWallet.AddNew(userId, "Debit", Currency.From("PLN"), 1000);

        wallet.Remove();

        var walletRemovedDomainEvent = AssertPublishedDomainEvent<DebitWalletRemovedDomainEvent>(wallet);
        Assert.That(walletRemovedDomainEvent.WalletId, Is.EqualTo(wallet.Id));
        Assert.That(walletRemovedDomainEvent.UserId, Is.EqualTo(wallet.UserId));
    }

    [Test]
    public void RemovingDebitWallet_WhenWasAlreadyRemoved_BreaksDebitWalletCannotBeRemovedMoreThanOnceRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = DebitWallet.AddNew(userId, "Debit", Currency.From("PLN"), 1000);

        wallet.Remove();

        AssertBrokenRule<DebitWalletCannotBeRemovedMoreThanOnceRule>(() =>
        {
            wallet.Remove();
        });
    }
}
