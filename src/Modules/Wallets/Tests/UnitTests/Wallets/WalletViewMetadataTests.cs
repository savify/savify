using App.Modules.Wallets.Domain.Wallets;
using App.Modules.Wallets.Domain.Wallets.WalletViewMetadata;

namespace App.Modules.Wallets.UnitTests.Wallets;

[TestFixture]
public class WalletViewMetadataTests : UnitTestBase
{
    [Test]
    public void AddingWalletViewMetadata_WithDefaultValues_IsSuccessful()
    {
        var walletId = new WalletId(Guid.NewGuid());
        var walletViewMetadata = WalletViewMetadata.CreateDefaultForWallet(walletId);

        Assert.That(walletViewMetadata.WalletId, Is.EqualTo(walletId));
        Assert.That(walletViewMetadata.Icon, Is.Null);
        Assert.That(walletViewMetadata.Color, Is.Null);
        Assert.That(walletViewMetadata.IsConsideredInTotalBalance, Is.True);
    }

    [Test]
    public void AddingWalletViewMetadata_IsSuccessful()
    {
        var walletId = new WalletId(Guid.NewGuid());
        var walletViewMetadata = WalletViewMetadata.CreateForWallet(
            walletId,
            "#ffffff",
            "https://cdn.savify.localhost/icons/wallet.png",
            false);

        Assert.That(walletViewMetadata.WalletId, Is.EqualTo(walletId));
        Assert.That(walletViewMetadata.Icon, Is.EqualTo("https://cdn.savify.localhost/icons/wallet.png"));
        Assert.That(walletViewMetadata.Color, Is.EqualTo("#ffffff"));
        Assert.That(walletViewMetadata.IsConsideredInTotalBalance, Is.False);
    }
}
