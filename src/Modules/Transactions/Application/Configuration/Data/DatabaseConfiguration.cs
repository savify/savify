using App.BuildingBlocks.Application.Data;

namespace App.Modules.Transactions.Application.Configuration.Data;

public static class DatabaseConfiguration
{
    public static DatabaseSchema Schema => new("transactions");
}
