using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets.Events;
using App.Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata;

namespace App.Modules.FinanceTracking.UnitTests.Wallets.CreditWallets;

[TestFixture]
public class CreditWalletEditorTests : UnitTestBase
{
    [Test]
    public async Task EditWallet_WhenWalletExists_ShouldEditWallet()
    {
        var userId = new UserId(Guid.NewGuid());
        var wallet = CreditWallet.AddNew(userId, "Cash", Currency.From("PLN"), 1000, 1000);
        var walletViewMetadata = WalletViewMetadata.CreateForWallet(
            wallet.Id,
            "#000000",
            "https://cdn.savify.localhost/icons/wallet.png",
            true);

        var creditWalletRepository = Substitute.For<ICreditWalletRepository>();
        creditWalletRepository.GetByIdAndUserIdAsync(wallet.Id, userId).Returns(wallet);

        var walletViewMetadataRepository = Substitute.For<IWalletViewMetadataRepository>();
        walletViewMetadataRepository.GetByWalletIdAsync(wallet.Id).Returns(walletViewMetadata);

        var creditWalletEditingService = new CreditWalletEditor(creditWalletRepository, walletViewMetadataRepository);

        await creditWalletEditingService.EditWallet(
            userId,
            wallet.Id,
            "New cash",
            2000,
            2000,
            "#FFFFFF",
            "https://cdn.savify.localhost/icons/new-wallet.png",
            false);

        var walletEditedDomainEvent = AssertPublishedDomainEvent<CreditWalletEditedDomainEvent>(wallet);
        Assert.That(walletEditedDomainEvent.WalletId, Is.EqualTo(wallet.Id));
        Assert.That(walletEditedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(walletEditedDomainEvent.NewCreditLimit, Is.EqualTo(2000));
        Assert.That(walletEditedDomainEvent.NewAvailableBalance, Is.EqualTo(2000));

        Assert.That(walletViewMetadata.Color, Is.EqualTo("#FFFFFF"));
        Assert.That(walletViewMetadata.Icon, Is.EqualTo("https://cdn.savify.localhost/icons/new-wallet.png"));
        Assert.That(walletViewMetadata.IsConsideredInTotalBalance, Is.False);
    }
}
