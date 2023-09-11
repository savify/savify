using System.Reflection;
using App.Modules.Banks.Application.Configuration.Commands;
using App.Modules.Banks.Application.Contracts;

namespace App.Modules.Banks.Infrastructure.Configuration;

internal static class Assemblies
{
    public static readonly Assembly Application = typeof(InternalCommandBase<Result>).Assembly;

    public static readonly Assembly Infrastructure = typeof(BanksContext).Assembly;
}
