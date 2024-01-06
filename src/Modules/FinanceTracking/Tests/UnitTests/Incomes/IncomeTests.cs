using App.Modules.FinanceTracking.Domain.Categories;
using App.Modules.FinanceTracking.Domain.Incomes;
using App.Modules.FinanceTracking.Domain.Incomes.Events;
using App.Modules.FinanceTracking.Domain.Incomes.Rules;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.UnitTests.Incomes;

[TestFixture]
public class IncomeTests : UnitTestBase
{
    [Test]
    public void AddingNewIncome_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWalletId = new WalletId(Guid.NewGuid());
        var categoryId = new CategoryId(Guid.NewGuid());
        var amount = Money.From(100, "PLN");
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];
        var walletsRepository = Substitute.For<IWalletsRepository>();
        walletsRepository.ExistsForUserIdAndWalletId(userId, sourceWalletId).Returns(true);

        var income = Income.AddNew(userId, sourceWalletId, categoryId, amount, madeOn, walletsRepository, comment, tags);

        var incomeAddedDomainEvent = AssertPublishedDomainEvent<IncomeAddedDomainEvent>(income);

        Assert.That(incomeAddedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(incomeAddedDomainEvent.TargetWalletId, Is.EqualTo(sourceWalletId));
        Assert.That(incomeAddedDomainEvent.CategoryId, Is.EqualTo(categoryId));
        Assert.That(incomeAddedDomainEvent.Amount, Is.EqualTo(amount));
        Assert.That(incomeAddedDomainEvent.Tags, Is.EquivalentTo(tags));
    }

    [Test]
    public void AddingNewIncome_WhenUserDoesNotOwnSourceWallet_BreaksIncomeSourceWalletMustBeOwnedByUserRule()
    {
        var sourceWalletId = new WalletId(Guid.NewGuid());
        var categoryId = new CategoryId(Guid.NewGuid());

        var userId = new UserId(Guid.NewGuid());
        var amount = Money.From(100, "PLN");
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];
        var walletsRepository = Substitute.For<IWalletsRepository>();
        walletsRepository.ExistsForUserIdAndWalletId(userId, sourceWalletId).Returns(false);

        AssertBrokenRule<IncomeTargetWalletMustBeOwnedByUserRule>(() =>
        {
            _ = Income.AddNew(userId, sourceWalletId, categoryId, amount, madeOn, walletsRepository, comment, tags);
        });
    }

    [Test]
    public void EditingIncome_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWalletId = new WalletId(Guid.NewGuid());
        var categoryId = new CategoryId(Guid.NewGuid());
        var amount = Money.From(100, "PLN");
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];
        var walletsRepository = Substitute.For<IWalletsRepository>();
        walletsRepository.ExistsForUserIdAndWalletId(userId, sourceWalletId).Returns(true);

        var income = Income.AddNew(userId, sourceWalletId, categoryId, amount, madeOn, walletsRepository, comment, tags);

        var newSourceWalletId = new WalletId(Guid.NewGuid());
        var newCategoryId = new CategoryId(Guid.NewGuid());
        var newAmount = Money.From(500, "USD");
        var newMadeOn = DateTime.UtcNow;
        var newComment = "Edited comment";
        string[] newTags = ["New Tag1", "New Tag2"];
        walletsRepository.ExistsForUserIdAndWalletId(userId, newSourceWalletId).Returns(true);

        income.Edit(newSourceWalletId, newCategoryId, newAmount, newMadeOn, walletsRepository, newComment, newTags);

        var incomeEditedDomainEvent = AssertPublishedDomainEvent<IncomeEditedDomainEvent>(income);

        Assert.That(incomeEditedDomainEvent.OldTargetWalletId, Is.EqualTo(sourceWalletId));
        Assert.That(incomeEditedDomainEvent.NewTargetWalletId, Is.EqualTo(newSourceWalletId));

        Assert.That(incomeEditedDomainEvent.OldAmount, Is.EqualTo(amount));
        Assert.That(incomeEditedDomainEvent.NewAmount, Is.EqualTo(newAmount));

        Assert.That(incomeEditedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(incomeEditedDomainEvent.Tags, Is.EqualTo(newTags));
    }

    [Test]
    public void EditingIncome_WhenUserDoesNotOwnNewSourceWallet_BreaksIncomeSourceWalletMustBeOwnedByUserRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWalletId = new WalletId(Guid.NewGuid());
        var categoryId = new CategoryId(Guid.NewGuid());
        var amount = Money.From(100, "PLN");
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];
        var walletsRepository = Substitute.For<IWalletsRepository>();
        walletsRepository.ExistsForUserIdAndWalletId(userId, sourceWalletId).Returns(true);

        var income = Income.AddNew(userId, sourceWalletId, categoryId, amount, madeOn, walletsRepository, comment, tags);

        var newSourceWalletId = new WalletId(Guid.NewGuid());
        var newCategoryId = new CategoryId(Guid.NewGuid());
        var newAmount = Money.From(500, "USD");
        var newMadeOn = DateTime.UtcNow;
        var newComment = "Edited comment";
        string[] newTags = ["New Tag1", "New Tag2"];
        walletsRepository.ExistsForUserIdAndWalletId(userId, newSourceWalletId).Returns(false);

        AssertBrokenRule<IncomeTargetWalletMustBeOwnedByUserRule>(() =>
        {
            income.Edit(newSourceWalletId, newCategoryId, newAmount, newMadeOn, walletsRepository, newComment, newTags);
        });
    }

    [Test]
    public void RemovingIncome_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWalletId = new WalletId(Guid.NewGuid());
        var categoryId = new CategoryId(Guid.NewGuid());
        var amount = Money.From(100, "PLN");
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];
        var walletsRepository = Substitute.For<IWalletsRepository>();
        walletsRepository.ExistsForUserIdAndWalletId(userId, sourceWalletId).Returns(true);

        var income = Income.AddNew(userId, sourceWalletId, categoryId, amount, madeOn, walletsRepository, comment, tags);

        income.Remove();

        var incomeRemovedDomainEvent = AssertPublishedDomainEvent<IncomeRemovedDomainEvent>(income);

        Assert.That(incomeRemovedDomainEvent.IncomeId, Is.EqualTo(income.Id));
    }
}
