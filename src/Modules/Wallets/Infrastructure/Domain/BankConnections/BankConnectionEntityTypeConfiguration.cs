using App.Modules.Wallets.Domain;
using App.Modules.Wallets.Domain.BankConnections;
using App.Modules.Wallets.Domain.BankConnections.BankAccounts;
using App.Modules.Wallets.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.Wallets.Infrastructure.Domain.BankConnections;

public class BankConnectionEntityTypeConfiguration : IEntityTypeConfiguration<BankConnection>
{
    public void Configure(EntityTypeBuilder<BankConnection> builder)
    {
        builder.ToTable("bank_connections", "wallets");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property<BankId>("_bankId").HasColumnName("bank_id");
        builder.Property<UserId>("_userId").HasColumnName("user_id");
        builder.Property<DateTime>("_createdAt").HasColumnName("created_at");
        builder.Property<DateTime?>("_refreshedAt").HasColumnName("refreshed_at");

        builder.OwnsMany<BankAccount>("_accounts", b =>
        {
            b.WithOwner().HasForeignKey("BankConnectionId");
            b.ToTable("bank_accounts", "wallets");

            b.Property<BankAccountId>("Id").HasColumnName("id");
            b.Property<BankConnectionId>("BankConnectionId").HasColumnName("bank_connection_id");
            b.HasKey("Id", "BankConnectionId");

            b.Property<string>("_externalId").HasColumnName("external_id");
            b.Property<string>("_name").HasColumnName("name");
            b.Property<int>("_amount").HasColumnName("amount");

            b.OwnsOne<Currency>("_currency", c =>
            {
                c.Property(x => x.Value).HasColumnName("currency");
            });
        });

        builder.OwnsOne<Consent>("_consent", b =>
        {
            b.Property(x => x.ExpiresAt).HasColumnName("consent_expires_at");
        });
    }
}
