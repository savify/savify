using System.Reflection;
using App.Modules.Notifications.Application.Configuration.Commands;
using App.Modules.Notifications.Application.Contracts;

namespace App.Modules.Notifications.Infrastructure.Configuration;

internal static class Assemblies
{
    public static readonly Assembly Application = typeof(InternalCommandBase<Result>).Assembly;

    public static readonly Assembly Infrastructure = typeof(NotificationsContext).Assembly;
}
