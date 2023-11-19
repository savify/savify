using App.Modules.Transactions.Domain.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.Transactions.Infrastructure.Domain.Finance;

internal static class OwnsOneMoneyConfiguration
{
    public static void OwnsOneMoney<TOwnerEntity>(this OwnedNavigationBuilder<TOwnerEntity, Money> builder, string amountColumnName, string currencyColumnName)
        where TOwnerEntity : class
    {
        builder.OwnsOne(o => o.Currency, k =>
        {
            k.Property(p => p.Value).HasColumnName(currencyColumnName);
        });
        builder.Property(k => k.Amount).HasColumnName(amountColumnName);
    }
}
