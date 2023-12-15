using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets;
using App.Modules.FinanceTracking.Domain.Wallets.CreditWallets.Events;
using App.Modules.FinanceTracking.Domain.Wallets.WalletViewMetadata;

namespace App.Modules.FinanceTracking.UnitTests.Wallets.CreditWallets;

[TestFixture]
public class CreditWalletFactoryTests : UnitTestBase
{
    [Test]
    public async Task CreatingCreditWallet_AddsWallet_And_ViewMetadata()
    {
        var userId = new UserId(Guid.NewGuid());

        var creditWalletRepository = Substitute.For<ICreditWalletRepository>();
        creditWalletRepository.AddAsync(Arg.Is<CreditWallet>(w => w.UserId == userId)).Returns(Task.CompletedTask);

        var walletViewMetadataRepository = Substitute.For<IWalletViewMetadataRepository>();
        walletViewMetadataRepository.AddAsync(Arg.Is<WalletViewMetadata>(m =>
            m.Color == "#000000" &&
            m.Icon == "icon" &&
            m.IsConsideredInTotalBalance == true))
            .Returns(Task.CompletedTask);

        var walletFactory = new CreditWalletFactory(creditWalletRepository, walletViewMetadataRepository);

        var wallet = await walletFactory.Create(
            userId,
            "Cash",
            Currency.From("PLN"),
            1000,
            1000,
            "#000000",
            "icon",
            true);

        Assert.That(wallet, Is.Not.Null);
        Assert.That(wallet.UserId, Is.EqualTo(userId));

        var walletAddedDomainEvent = AssertPublishedDomainEvent<CreditWalletAddedDomainEvent>(wallet);
        Assert.That(walletAddedDomainEvent.WalletId, Is.EqualTo(wallet.Id));
        Assert.That(walletAddedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(walletAddedDomainEvent.Currency, Is.EqualTo(Currency.From("PLN")));
    }
}
