using App.BuildingBlocks.Application.Data;

namespace App.Modules.Wallets.Application.Configuration.Data;

public static class DatabaseConfiguration
{
    public static DatabaseSchema Schema => new("wallets");
}
