using App.Modules.Accounts.Domain.Accounts;
using App.Modules.Accounts.Domain.Accounts.CreditAccounts;
using App.Modules.Accounts.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.Accounts.Infrastructure.Domain.Accounts.CreditAccounts;

internal class CreditAccountsEntityTypeConfiguration : IEntityTypeConfiguration<CreditAccount>
{
    public void Configure(EntityTypeBuilder<CreditAccount> builder)
    {
        builder.ToTable("credit_accounts", "accounts");

        builder.HasKey(account => account.Id);
        builder.Property(account => account.Id).HasColumnName("id");

        builder.Property<UserId>("UserId").HasColumnName("user_id");
        builder.Property<string>("_title").HasColumnName("title");
        builder.Property<int>("_availableBalance").HasColumnName("available_balance");
        builder.Property<int>("_creditLimit").HasColumnName("credit_limit");
        builder.Property<DateTime>("_createdAt").HasColumnName("created_at");

        builder.OwnsOne<Currency>("_currency", b =>
        {
            b.Property(currency => currency.Value).HasColumnName("currency");
        });
    }
}
