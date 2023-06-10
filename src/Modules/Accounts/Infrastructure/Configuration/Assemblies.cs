using System.Reflection;
using App.Modules.Accounts.Application.Configuration.Commands;
using App.Modules.Accounts.Application.Contracts;

namespace App.Modules.Accounts.Infrastructure.Configuration;

internal static class Assemblies
{
    public static readonly Assembly Application = typeof(InternalCommandBase<Result>).Assembly;
    
    public static readonly Assembly Infrastructure = typeof(AccountsContext).Assembly;
}
