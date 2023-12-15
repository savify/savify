using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets;
using App.Modules.FinanceTracking.Domain.Wallets.DebitWallets.Events;
using App.Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata;

namespace App.Modules.FinanceTracking.UnitTests.Wallets.DebitWallets;

[TestFixture]
public class DebitWalletEditingServiceTests : UnitTestBase
{
    [Test]
    public async Task EditWallet_WhenWalletExists_ShouldEditWallet()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = DebitWallet.AddNew(userId, "Cash", Currency.From("PLN"), 1000);
        var walletViewMetadata = WalletViewMetadata.CreateForWallet(
            wallet.Id,
            "#000000",
            "https://cdn.savify.localhost/icons/wallet.png",
            true);

        var debitWalletRepository = Substitute.For<IDebitWalletRepository>();
        debitWalletRepository.GetByIdAndUserIdAsync(wallet.Id, userId).Returns(wallet);

        var walletViewMetadataRepository = Substitute.For<IWalletViewMetadataRepository>();
        walletViewMetadataRepository.GetByWalletIdAsync(wallet.Id).Returns(walletViewMetadata);

        var debitWalletEditingService = new DebitWalletEditingService(debitWalletRepository, walletViewMetadataRepository);

        await debitWalletEditingService.EditWallet(
            userId,
            wallet.Id,
            "New cash",
            new Currency("GBP"),
            2000,
            "#FFFFFF",
            "https://cdn.savify.localhost/icons/new-wallet.png",
            false);

        var walletEditedDomainEvent = AssertPublishedDomainEvent<DebitWalletEditedDomainEvent>(wallet);
        Assert.That(walletEditedDomainEvent.WalletId, Is.EqualTo(wallet.Id));
        Assert.That(walletEditedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(walletEditedDomainEvent.NewCurrency, Is.EqualTo(new Currency("GBP")));
        Assert.That(walletEditedDomainEvent.NewBalance, Is.EqualTo(2000));

        Assert.That(walletViewMetadata.Color, Is.EqualTo("#FFFFFF"));
        Assert.That(walletViewMetadata.Icon, Is.EqualTo("https://cdn.savify.localhost/icons/new-wallet.png"));
        Assert.That(walletViewMetadata.IsConsideredInTotalBalance, Is.False);
    }
}
