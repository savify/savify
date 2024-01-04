using App.Modules.FinanceTracking.Domain.Categories;
using App.Modules.FinanceTracking.Domain.Expenses;
using App.Modules.FinanceTracking.Domain.Expenses.Events;
using App.Modules.FinanceTracking.Domain.Expenses.Rules;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using App.Modules.FinanceTracking.Domain.Wallets;

namespace App.Modules.FinanceTracking.UnitTests.Expenses;

[TestFixture]
public class ExpenseTests : UnitTestBase
{
    [Test]
    public void AddingNewExpense_IsSuccessful()
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

        var expense = Expense.AddNew(userId, sourceWalletId, categoryId, amount, madeOn, walletsRepository, comment, tags);

        var expenseAddedDomainEvent = AssertPublishedDomainEvent<ExpenseAddedDomainEvent>(expense);

        Assert.That(expenseAddedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(expenseAddedDomainEvent.SourceWalletId, Is.EqualTo(sourceWalletId));
        Assert.That(expenseAddedDomainEvent.CategoryId, Is.EqualTo(categoryId));
        Assert.That(expenseAddedDomainEvent.Amount, Is.EqualTo(amount));
        Assert.That(expenseAddedDomainEvent.Tags, Is.EquivalentTo(tags));
    }

    [Test]
    public void AddingNewExpense_WhenUserDoesNotOwnSourceWallet_BreaksExpenseSourceWalletMustBeOwnedByUserRule()
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

        AssertBrokenRule<ExpenseSourceWalletMustBeOwnedByUserRule>(() =>
        {
            _ = Expense.AddNew(userId, sourceWalletId, categoryId, amount, madeOn, walletsRepository, comment, tags);
        });
    }

    [Test]
    public void EditingExpense_IsSuccessful()
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

        var expense = Expense.AddNew(userId, sourceWalletId, categoryId, amount, madeOn, walletsRepository, comment, tags);

        var newSourceWalletId = new WalletId(Guid.NewGuid());
        var newCategoryId = new CategoryId(Guid.NewGuid());
        var newAmount = Money.From(500, "USD");
        var newMadeOn = DateTime.UtcNow;
        var newComment = "Edited comment";
        string[] newTags = ["New Tag1", "New Tag2"];
        walletsRepository.ExistsForUserIdAndWalletId(userId, newSourceWalletId).Returns(true);

        expense.Edit(newSourceWalletId, newCategoryId, newAmount, newMadeOn, walletsRepository, newComment, newTags);

        var expenseEditedDomainEvent = AssertPublishedDomainEvent<ExpenseEditedDomainEvent>(expense);

        Assert.That(expenseEditedDomainEvent.OldSourceWalletId, Is.EqualTo(sourceWalletId));
        Assert.That(expenseEditedDomainEvent.NewSourceWalletId, Is.EqualTo(newSourceWalletId));

        Assert.That(expenseEditedDomainEvent.OldAmount, Is.EqualTo(amount));
        Assert.That(expenseEditedDomainEvent.NewAmount, Is.EqualTo(newAmount));

        Assert.That(expenseEditedDomainEvent.UserId, Is.EqualTo(userId));
        Assert.That(expenseEditedDomainEvent.Tags, Is.EqualTo(newTags));
    }

    [Test]
    public void EditingExpense_WhenUserDoesNotOwnNewSourceWallet_BreaksExpenseSourceWalletMustBeOwnedByUserRule()
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

        var expense = Expense.AddNew(userId, sourceWalletId, categoryId, amount, madeOn, walletsRepository, comment, tags);

        var newSourceWalletId = new WalletId(Guid.NewGuid());
        var newCategoryId = new CategoryId(Guid.NewGuid());
        var newAmount = Money.From(500, "USD");
        var newMadeOn = DateTime.UtcNow;
        var newComment = "Edited comment";
        string[] newTags = ["New Tag1", "New Tag2"];
        walletsRepository.ExistsForUserIdAndWalletId(userId, newSourceWalletId).Returns(false);

        AssertBrokenRule<ExpenseSourceWalletMustBeOwnedByUserRule>(() =>
        {
            expense.Edit(newSourceWalletId, newCategoryId, newAmount, newMadeOn, walletsRepository, newComment, newTags);
        });
    }

    [Test]
    public void RemovingExpense_IsSuccessful()
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

        var expense = Expense.AddNew(userId, sourceWalletId, categoryId, amount, madeOn, walletsRepository, comment, tags);

        expense.Remove();

        var expenseRemovedDomainEvent = AssertPublishedDomainEvent<ExpenseRemovedDomainEvent>(expense);

        Assert.That(expenseRemovedDomainEvent.ExpenseId, Is.EqualTo(expense.Id));
    }
}
