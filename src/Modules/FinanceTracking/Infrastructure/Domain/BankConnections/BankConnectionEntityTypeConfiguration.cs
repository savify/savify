using App.Modules.FinanceTracking.Domain.BankConnections;
using App.Modules.FinanceTracking.Domain.BankConnections.BankAccounts;
using App.Modules.FinanceTracking.Domain.Finance;
using App.Modules.FinanceTracking.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.FinanceTracking.Infrastructure.Domain.BankConnections;

public class BankConnectionEntityTypeConfiguration : IEntityTypeConfiguration<BankConnection>
{
    public void Configure(EntityTypeBuilder<BankConnection> builder)
    {
        builder.ToTable("bank_connections");

        builder.HasKey(x => x.Id);

        builder.Property<BankId>("_bankId");
        builder.Property<UserId>("_userId");
        builder.Property<DateTime>("_createdAt");
        builder.Property<DateTime?>("_refreshedAt");

        builder.OwnsMany<BankAccount>("_accounts", b =>
        {
            b.WithOwner().HasForeignKey("BankConnectionId");
            b.ToTable("bank_accounts");

            b.Property<BankAccountId>("Id");
            b.Property<BankConnectionId>("BankConnectionId");
            b.HasKey("Id", "BankConnectionId");

            b.Property<string>("_externalId");
            b.Property<string>("_name");
            b.Property<int>("Balance");

            b.OwnsOne<Currency>("Currency", c =>
            {
                c.Property(x => x.Value).HasColumnName("currency");
            });
        });

        builder.ComplexProperty<Consent>("_consent", b =>
        {
            b.Property(x => x.ExpiresAt).HasColumnName("consent_expires_at");
        });
    }
}
