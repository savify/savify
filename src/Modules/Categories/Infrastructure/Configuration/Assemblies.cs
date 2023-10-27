using System.Reflection;
using App.Modules.Categories.Application.Configuration.Commands;

namespace App.Modules.Categories.Infrastructure.Configuration;

internal static class Assemblies
{
    public static readonly Assembly Application = typeof(InternalCommandBase).Assembly;

    public static readonly Assembly Infrastructure = typeof(CategoriesContext).Assembly;
}
