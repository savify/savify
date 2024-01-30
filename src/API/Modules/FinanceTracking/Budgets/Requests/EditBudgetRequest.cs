namespace App.API.Modules.FinanceTracking.Budgets.Requests;

public class EditBudgetRequest
{
    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public IDictionary<Guid, int> CategoriesBudget { get; set; }

    public string Currency { get; set; }
}
