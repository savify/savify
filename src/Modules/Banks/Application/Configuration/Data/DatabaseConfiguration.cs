using App.BuildingBlocks.Application.Data;

namespace App.Modules.Banks.Application.Configuration.Data;

public static class DatabaseConfiguration
{
    public static DatabaseSchema Schema => new("banks");
}
