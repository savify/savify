using System.Reflection;
using App.Modules.FinanceTracking.Application.Configuration.Commands;

namespace App.Modules.FinanceTracking.Infrastructure.Configuration;

internal static class Assemblies
{
    public static readonly Assembly Application = typeof(InternalCommandBase).Assembly;

    public static readonly Assembly Infrastructure = typeof(FinanceTrackingContext).Assembly;
}
