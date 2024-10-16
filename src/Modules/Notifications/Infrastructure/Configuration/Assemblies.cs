using System.Reflection;
using App.Modules.Notifications.Application.Configuration.Commands;

namespace App.Modules.Notifications.Infrastructure.Configuration;

internal static class Assemblies
{
    public static readonly Assembly Application = typeof(InternalCommandBase).Assembly;

    public static readonly Assembly Infrastructure = typeof(NotificationsContext).Assembly;
}
