using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets;
using App.Modules.FinanceTracking.Domain.Wallets.Events;
using App.Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata;

namespace App.Modules.FinanceTracking.UnitTests.Wallets.CashWallets;

[TestFixture]
public class CashWalletEditorTests : UnitTestBase
{
    [Test]
    public async Task EditWallet_WhenWalletExists_ShouldEditWallet()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"), 1000);
        var walletViewMetadata = WalletViewMetadata.CreateForWallet(
            wallet.Id,
            "#000000",
            "https://cdn.savify.localhost/icons/wallet.png",
            true);

        var cashWalletRepository = Substitute.For<ICashWalletRepository>();
        cashWalletRepository.GetByIdAndUserIdAsync(wallet.Id, userId).Returns(wallet);

        var walletViewMetadataRepository = Substitute.For<IWalletViewMetadataRepository>();
        walletViewMetadataRepository.GetByWalletIdAsync(wallet.Id).Returns(walletViewMetadata);

        var cashWalletEditingService = new CashWalletEditor(cashWalletRepository, walletViewMetadataRepository);

        await cashWalletEditingService.EditWallet(
            userId,
            wallet.Id,
            "New cash",
            2000,
            "#FFFFFF",
            "https://cdn.savify.localhost/icons/new-wallet.png",
            false);

        Assert.That(wallet.Balance, Is.EqualTo(2000));

        var walletBalanceIncreasedDomainEvent = AssertPublishedDomainEvent<WalletBalanceIncreasedDomainEvent>(wallet);
        Assert.That(walletBalanceIncreasedDomainEvent.WalletId, Is.EqualTo(wallet.Id));
        Assert.That(walletBalanceIncreasedDomainEvent.Amount, Is.EqualTo(Money.From(1000, Currency.From("PLN"))));
        Assert.That(walletBalanceIncreasedDomainEvent.NewBalance, Is.EqualTo(2000));

        Assert.That(walletViewMetadata.Color, Is.EqualTo("#FFFFFF"));
        Assert.That(walletViewMetadata.Icon, Is.EqualTo("https://cdn.savify.localhost/icons/new-wallet.png"));
        Assert.That(walletViewMetadata.IsConsideredInTotalBalance, Is.False);
    }
}
