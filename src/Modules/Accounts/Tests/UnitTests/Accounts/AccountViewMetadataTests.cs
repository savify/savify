using App.Modules.Accounts.Domain.Accounts;
using App.Modules.Accounts.Domain.Accounts.AccountViewMetadata;

namespace App.Modules.Accounts.UnitTests.Accounts;

[TestFixture]
public class AccountViewMetadataTests : UnitTestBase
{
    [Test]
    public void AddingAccountViewMetadata_WithDefaultValues_IsSuccessful()
    {
        var accountId = new AccountId(Guid.NewGuid());  
        var accountViewMetadata = AccountViewMetadata.CreateForAccount(accountId);
        
        Assert.That(accountViewMetadata.AccountId, Is.EqualTo(accountId));
        Assert.That(accountViewMetadata.Icon, Is.Null);
        Assert.That(accountViewMetadata.Color, Is.Null);
        Assert.That(accountViewMetadata.IsConsideredInTotalBalance, Is.True);
    }
    
    [Test]
    public void AddingAccountViewMetadata_IsSuccessful()
    {
        var accountId = new AccountId(Guid.NewGuid());  
        var accountViewMetadata = AccountViewMetadata.CreateForAccount(
            accountId, 
            "#ffffff",
            "https://cdn.savify.localhost/icons/account.png",
            false);
        
        Assert.That(accountViewMetadata.AccountId, Is.EqualTo(accountId));
        Assert.That(accountViewMetadata.Icon, Is.EqualTo("https://cdn.savify.localhost/icons/account.png"));
        Assert.That(accountViewMetadata.Color, Is.EqualTo("#ffffff"));
        Assert.That(accountViewMetadata.IsConsideredInTotalBalance, Is.False);
    }
}
