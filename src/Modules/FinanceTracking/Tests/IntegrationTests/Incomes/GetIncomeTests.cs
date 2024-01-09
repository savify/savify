using App.BuildingBlocks.Application.Exceptions;
using App.Modules.FinanceTracking.Application.Incomes.GetIncome;

namespace App.Modules.FinanceTracking.IntegrationTests.Incomes;

[TestFixture]
public class GetIncomeTests : IncomesTestBase
{
    [Test]
    public async Task GetIncomeQuery_WhenIncomeDoesNotExist_ReturnsNull()
    {
        var income = await FinanceTrackingModule.ExecuteQueryAsync(new GetIncomeQuery(Guid.NewGuid(), Guid.NewGuid()));

        Assert.That(income, Is.Null);
    }

    [Test]
    public async Task GetIncomeQuery_WhenIncomeDoesNotBelongToUser_ThrowsAccessDeniedException()
    {
        var userId = Guid.NewGuid();
        var incomeId = await AddNewIncomeAsync(userId);

        await Assert.ThatAsync(() => FinanceTrackingModule.ExecuteQueryAsync(
            new GetIncomeQuery(incomeId, Guid.NewGuid())),
            Throws.TypeOf<AccessDeniedException>());
    }
}
