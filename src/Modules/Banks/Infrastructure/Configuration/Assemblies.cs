using System.Reflection;
using App.Modules.Banks.Application.Configuration.Commands;

namespace App.Modules.Banks.Infrastructure.Configuration;

internal static class Assemblies
{
    public static readonly Assembly Application = typeof(InternalCommandBase).Assembly;

    public static readonly Assembly Infrastructure = typeof(BanksContext).Assembly;
}
