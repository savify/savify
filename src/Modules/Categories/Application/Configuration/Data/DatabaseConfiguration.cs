using App.BuildingBlocks.Application.Data;

namespace App.Modules.Categories.Application.Configuration.Data;

public static class DatabaseConfiguration
{
    public static DatabaseSchema Schema => new("categories");
}
