using App.Modules.FinanceTracking.Domain.Categories;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Incomes;
using App.Modules.FinanceTracking.Domain.Incomes.Events;
using App.Modules.FinanceTracking.Domain.Incomes.Rules;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets;

namespace App.Modules.FinanceTracking.UnitTests.Incomes;

[TestFixture]
public class IncomeTests : UnitTestBase
{
    [Test]
    public void AddingNewIncome_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"), 100);
        var categoryId = new CategoryId(Guid.NewGuid());
        var amount = Money.From(100, "PLN");
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];

        var income = Income.AddNew(userId, sourceWallet, categoryId, amount, madeOn, comment, tags);

        var incomeAddedDomainEvent = AssertPublishedDomainEvent<IncomeAddedDomainEvent>(income);

        Assert.That(incomeAddedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(incomeAddedDomainEvent.TargetWalletId, Is.EqualTo(sourceWallet.Id));
        Assert.That(incomeAddedDomainEvent.CategoryId, Is.EqualTo(categoryId));
        Assert.That(incomeAddedDomainEvent.Amount, Is.EqualTo(amount));
        Assert.That(incomeAddedDomainEvent.Tags, Is.EquivalentTo(tags));
    }

    [Test]
    public void AddingNewIncome_WhenUserDoesNotOwnSourceWallet_BreaksIncomeSourceWalletMustBeOwnedByUserRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var categoryId = new CategoryId(Guid.NewGuid());
        var sourceWallet = CashWallet.AddNew(new UserId(Guid.NewGuid()), "Cash", Currency.From("PLN"), 100);

        var amount = Money.From(100, "PLN");
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];

        AssertBrokenRule<IncomeTargetWalletMustBeOwnedByUserRule>(() =>
        {
            _ = Income.AddNew(userId, sourceWallet, categoryId, amount, madeOn, comment, tags);
        });
    }

    [Test]
    public void EditingIncome_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"), 100);
        var categoryId = new CategoryId(Guid.NewGuid());
        var amount = Money.From(100, "PLN");
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];

        var income = Income.AddNew(userId, sourceWallet, categoryId, amount, madeOn, comment, tags);

        var newCategoryId = new CategoryId(Guid.NewGuid());
        var newAmount = Money.From(500, "USD");
        var newSourceWallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"), 100);
        var newMadeOn = DateTime.UtcNow;
        var newComment = "Edited comment";
        string[] newTags = ["New Tag1", "New Tag2"];

        income.Edit(newSourceWallet, newCategoryId, newAmount, newMadeOn, newComment, newTags);

        var incomeEditedDomainEvent = AssertPublishedDomainEvent<IncomeEditedDomainEvent>(income);

        Assert.That(incomeEditedDomainEvent.OldTargetWalletId, Is.EqualTo(sourceWallet.Id));
        Assert.That(incomeEditedDomainEvent.NewTargetWalletId, Is.EqualTo(newSourceWallet.Id));

        Assert.That(incomeEditedDomainEvent.OldAmount, Is.EqualTo(amount));
        Assert.That(incomeEditedDomainEvent.NewAmount, Is.EqualTo(newAmount));

        Assert.That(incomeEditedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(incomeEditedDomainEvent.Tags, Is.EqualTo(newTags));
    }

    [Test]
    public void EditingIncome_WhenUserDoesNotOwnNewSourceWallet_BreaksIncomeSourceWalletMustBeOwnedByUserRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"), 100);
        var categoryId = new CategoryId(Guid.NewGuid());
        var amount = Money.From(100, "PLN");
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];

        var income = Income.AddNew(userId, sourceWallet, categoryId, amount, madeOn, comment, tags);

        var newSourceWallet = CashWallet.AddNew(new UserId(Guid.NewGuid()), "Cash", Currency.From("PLN"), 100);
        var newCategoryId = new CategoryId(Guid.NewGuid());
        var newAmount = Money.From(500, "USD");
        var newMadeOn = DateTime.UtcNow;
        var newComment = "Edited comment";
        string[] newTags = ["New Tag1", "New Tag2"];

        AssertBrokenRule<IncomeTargetWalletMustBeOwnedByUserRule>(() =>
        {
            income.Edit(newSourceWallet, newCategoryId, newAmount, newMadeOn, newComment, newTags);
        });
    }

    [Test]
    public void RemovingIncome_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"), 100);
        var categoryId = new CategoryId(Guid.NewGuid());
        var amount = Money.From(100, "PLN");
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];

        var income = Income.AddNew(userId, sourceWallet, categoryId, amount, madeOn, comment, tags);

        income.Remove();

        var incomeRemovedDomainEvent = AssertPublishedDomainEvent<IncomeRemovedDomainEvent>(income);

        Assert.That(incomeRemovedDomainEvent.IncomeId, Is.EqualTo(income.Id));
    }
}
