using System.Reflection;
using App.Modules.Wallets.Application.Configuration.Commands;
using App.Modules.Wallets.Application.Contracts;

namespace App.Modules.Wallets.Infrastructure.Configuration;

internal static class Assemblies
{
    public static readonly Assembly Application = typeof(InternalCommandBase<Result>).Assembly;
    
    public static readonly Assembly Infrastructure = typeof(WalletsContext).Assembly;
}
