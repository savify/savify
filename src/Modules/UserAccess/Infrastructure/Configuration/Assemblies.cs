using System.Reflection;
using App.Modules.UserAccess.Application.Configuration.Commands;

namespace App.Modules.UserAccess.Infrastructure.Configuration;

internal static class Assemblies
{
    public static readonly Assembly Application = typeof(ICommandHandler<>).Assembly;
    
    public static readonly Assembly Infrastructure = typeof(UserAccessContext).Assembly;
}
