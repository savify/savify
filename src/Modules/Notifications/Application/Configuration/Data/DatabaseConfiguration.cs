using App.BuildingBlocks.Application.Data;

namespace App.Modules.Notifications.Application.Configuration.Data;

public static class DatabaseConfiguration
{
    public static DatabaseSchema Schema => new("notifications");
}
