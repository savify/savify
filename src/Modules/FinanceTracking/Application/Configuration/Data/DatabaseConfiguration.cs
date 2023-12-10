using App.BuildingBlocks.Application.Data;

namespace App.Modules.FinanceTracking.Application.Configuration.Data;

public static class DatabaseConfiguration
{
    public static DatabaseSchema Schema => new("finance_tracking");
}
