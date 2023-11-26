using App.BuildingBlocks.Application.Data;

namespace App.Modules.UserAccess.Application.Configuration.Data;

public static class DatabaseConfiguration
{
    public static DatabaseSchema Schema => new("user_access");
}
