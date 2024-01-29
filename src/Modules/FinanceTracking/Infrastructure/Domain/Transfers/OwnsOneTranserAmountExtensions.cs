using System.Linq.Expressions;
using App.Modules.FinanceTracking.Domain.Transfers;
using App.Modules.FinanceTracking.Infrastructure.Domain.Finance;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.Transfers;

internal static class OwnsOneTranserAmountExtensions
{
    public static void OwnsOneTransactionAmount<TEntity>(this EntityTypeBuilder<TEntity> builder, string navigationName,
        Action<TransactionAmountOptions>? configure = null)
        where TEntity : class
    {
        builder.OwnsOne<TransferAmount>(navigationName, b =>
        {
            b.ConfigureTransactionAmount(configure);
        });

        builder.Navigation(navigationName).IsRequired();
    }

    public static void OwnsOneTransactionAmount<TOwnerEntity, TDependentEntity>(this OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> builder, string navigationName, Action<TransactionAmountOptions>? configure = null)
        where TOwnerEntity : class
        where TDependentEntity : class
    {
        builder.OwnsOne<TransferAmount>(navigationName, b =>
        {
            b.ConfigureTransactionAmount(configure);
        });

        builder.Navigation(navigationName).IsRequired();
    }

    public static void OwnsOneTransactionAmount<TOwnerEntity, TDependentEntity>(this OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> builder, Expression<Func<TDependentEntity, TransferAmount?>> navigationExpression, Action<TransactionAmountOptions>? configure = null)
        where TOwnerEntity : class
        where TDependentEntity : class
    {
        builder.OwnsOne(navigationExpression, b =>
        {
            b.ConfigureTransactionAmount(configure);
        });

        builder.Navigation(navigationExpression).IsRequired();
    }

    private static void ConfigureTransactionAmount<TOwnerEntity>(this OwnedNavigationBuilder<TOwnerEntity, TransferAmount> builder, Action<TransactionAmountOptions>? configure)
        where TOwnerEntity : class
    {
        var options = new TransactionAmountOptions();
        configure?.Invoke(options);

        builder.OwnsOneMoney(p => p.Source, options.SourceMoneyAmountColumnName, options.SourceMoneyCurrencyColumnName);
        builder.OwnsOneMoney(p => p.Target, options.TargetMoneyAmountColumnName, options.TargetMoneyCurrencyColumnName);
        builder.OwnsOneExchangeRate(p => p.ExchangeRate, options.ExchangeRateFromColumnName, options.ExchangeRateToColumnName, options.ExchangeRateRateColumnName);
    }
}

class TransactionAmountOptions
{
    public string SourceMoneyAmountColumnName { get; set; } = "source_amount";

    public string SourceMoneyCurrencyColumnName { get; set; } = "source_currency_code";

    public string TargetMoneyAmountColumnName { get; set; } = "target_amount";

    public string TargetMoneyCurrencyColumnName { get; set; } = "target_currency_code";

    public string ExchangeRateFromColumnName { get; set; } = "exchange_rate_from_currency_code";

    public string ExchangeRateToColumnName { get; set; } = "exchange_rate_to_currency_code";

    public string ExchangeRateRateColumnName { get; set; } = "exchange_rate_rate";
}
