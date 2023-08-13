using App.Modules.Wallets.Domain.Accounts;
using App.Modules.Wallets.Domain.Accounts.AccountViewMetadata;

namespace App.Modules.Accounts.UnitTests.Accounts;

[TestFixture]
public class AccountViewMetadataTests : UnitTestBase
{
    [Test]
    public void AddingAccountViewMetadata_WithDefaultValues_IsSuccessful()
    {
        var accountId = new WalletId(Guid.NewGuid());  
        var accountViewMetadata = WalletViewMetadata.CreateDefaultForAccount(accountId);
        
        Assert.That(accountViewMetadata.WalletId, Is.EqualTo(accountId));
        Assert.That(accountViewMetadata.Icon, Is.Null);
        Assert.That(accountViewMetadata.Color, Is.Null);
        Assert.That(accountViewMetadata.IsConsideredInTotalBalance, Is.True);
    }
    
    [Test]
    public void AddingAccountViewMetadata_IsSuccessful()
    {
        var accountId = new WalletId(Guid.NewGuid());  
        var accountViewMetadata = WalletViewMetadata.CreateForAccount(
            accountId, 
            "#ffffff",
            "https://cdn.savify.localhost/icons/account.png",
            false);
        
        Assert.That(accountViewMetadata.WalletId, Is.EqualTo(accountId));
        Assert.That(accountViewMetadata.Icon, Is.EqualTo("https://cdn.savify.localhost/icons/account.png"));
        Assert.That(accountViewMetadata.Color, Is.EqualTo("#ffffff"));
        Assert.That(accountViewMetadata.IsConsideredInTotalBalance, Is.False);
    }
}
