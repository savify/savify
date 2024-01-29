using App.Modules.FinanceTracking.Domain.Categories;
using App.Modules.FinanceTracking.Domain.Expenses;
using App.Modules.FinanceTracking.Domain.Expenses.Events;
using App.Modules.FinanceTracking.Domain.Expenses.Rules;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets.CashWallets;

namespace App.Modules.FinanceTracking.UnitTests.Expenses;

[TestFixture]
public class ExpenseTests : UnitTestBase
{
    [Test]
    public void AddingNewExpense_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"));

        var categoryId = new CategoryId(Guid.NewGuid());
        var amount = Money.From(100, Currency.From("PLN"));
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];

        var expense = Expense.AddNew(userId, sourceWallet, categoryId, amount, madeOn, comment, tags);

        var expenseAddedDomainEvent = AssertPublishedDomainEvent<ExpenseAddedDomainEvent>(expense);

        Assert.That(expenseAddedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(expenseAddedDomainEvent.SourceWalletId, Is.EqualTo(sourceWallet.Id));
        Assert.That(expenseAddedDomainEvent.CategoryId, Is.EqualTo(categoryId));
        Assert.That(expenseAddedDomainEvent.Amount, Is.EqualTo(amount));
        Assert.That(expenseAddedDomainEvent.Tags, Is.EquivalentTo(tags));
    }

    [Test]
    public void AddingNewExpense_WhenUserDoesNotOwnSourceWallet_BreaksExpenseSourceWalletMustBeOwnedByUserRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWallet = CashWallet.AddNew(new UserId(Guid.NewGuid()), "Cash", Currency.From("PLN"));
        var categoryId = new CategoryId(Guid.NewGuid());

        var amount = Money.From(100, Currency.From("PLN"));
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];

        AssertBrokenRule<ExpenseSourceWalletMustBeOwnedByUserRule>(() =>
        {
            _ = Expense.AddNew(userId, sourceWallet, categoryId, amount, madeOn, comment, tags);
        });
    }

    [Test]
    public void EditingExpense_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"));
        var categoryId = new CategoryId(Guid.NewGuid());
        var amount = Money.From(100, Currency.From("PLN"));
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];

        var expense = Expense.AddNew(userId, sourceWallet, categoryId, amount, madeOn, comment, tags);

        var newSourceWallet = CashWallet.AddNew(userId, "Cash", Currency.From("USD"));
        var newCategoryId = new CategoryId(Guid.NewGuid());
        var newAmount = Money.From(500, Currency.From("USD"));
        var newMadeOn = DateTime.UtcNow;
        var newComment = "Edited comment";
        string[] newTags = ["New Tag1", "New Tag2"];

        expense.Edit(newSourceWallet, newCategoryId, newAmount, newMadeOn, newComment, newTags);

        var expenseEditedDomainEvent = AssertPublishedDomainEvent<ExpenseEditedDomainEvent>(expense);

        Assert.That(expenseEditedDomainEvent.OldSourceWalletId, Is.EqualTo(sourceWallet.Id));
        Assert.That(expenseEditedDomainEvent.NewSourceWalletId, Is.EqualTo(newSourceWallet.Id));

        Assert.That(expenseEditedDomainEvent.OldAmount, Is.EqualTo(amount));
        Assert.That(expenseEditedDomainEvent.NewAmount, Is.EqualTo(newAmount));

        Assert.That(expenseEditedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(expenseEditedDomainEvent.Tags, Is.EqualTo(newTags));
    }

    [Test]
    public void EditingExpense_WhenUserDoesNotOwnNewSourceWallet_BreaksExpenseSourceWalletMustBeOwnedByUserRule()
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"));
        var categoryId = new CategoryId(Guid.NewGuid());
        var amount = Money.From(100, Currency.From("PLN"));
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];

        var expense = Expense.AddNew(userId, sourceWallet, categoryId, amount, madeOn, comment, tags);

        var newSourceWallet = CashWallet.AddNew(new UserId(Guid.NewGuid()), "Cash", Currency.From("USD"));
        var newCategoryId = new CategoryId(Guid.NewGuid());
        var newAmount = Money.From(500, Currency.From("USD"));
        var newMadeOn = DateTime.UtcNow;
        var newComment = "Edited comment";
        string[] newTags = ["New Tag1", "New Tag2"];

        AssertBrokenRule<ExpenseSourceWalletMustBeOwnedByUserRule>(() =>
        {
            expense.Edit(newSourceWallet, newCategoryId, newAmount, newMadeOn, newComment, newTags);
        });
    }

    [Test]
    public void RemovingExpense_IsSuccessful()
    {
        var userId = new UserId(Guid.NewGuid());
        var sourceWallet = CashWallet.AddNew(userId, "Cash", Currency.From("PLN"));
        var categoryId = new CategoryId(Guid.NewGuid());
        var amount = Money.From(100, Currency.From("PLN"));
        var madeOn = DateTime.UtcNow;
        var comment = "Some comment";
        string[] tags = ["Tag1", "Tag2"];

        var expense = Expense.AddNew(userId, sourceWallet, categoryId, amount, madeOn, comment, tags);

        expense.Remove();

        var expenseRemovedDomainEvent = AssertPublishedDomainEvent<ExpenseRemovedDomainEvent>(expense);

        Assert.That(expenseRemovedDomainEvent.ExpenseId, Is.EqualTo(expense.Id));
        Assert.That(expenseRemovedDomainEvent.SourceWalletId, Is.EqualTo(sourceWallet.Id));
        Assert.That(expenseRemovedDomainEvent.Amount, Is.EqualTo(amount));
    }
}
