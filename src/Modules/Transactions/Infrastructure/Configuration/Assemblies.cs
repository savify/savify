using System.Reflection;
using App.Modules.Transactions.Application.Configuration.Commands;

namespace App.Modules.Transactions.Infrastructure.Configuration;

internal static class Assemblies
{
    public static readonly Assembly Application = typeof(InternalCommandBase).Assembly;

    public static readonly Assembly Infrastructure = typeof(TransactionsContext).Assembly;
}
