using System.Reflection;
using App.Modules.UserAccess.Application.Configuration.Commands;
using App.Modules.UserAccess.Application.Contracts;

namespace App.Modules.UserAccess.Infrastructure.Configuration;

internal static class Assemblies
{
    public static readonly Assembly Application = typeof(InternalCommandBase<Result>).Assembly;
    
    public static readonly Assembly Infrastructure = typeof(UserAccessContext).Assembly;
}
