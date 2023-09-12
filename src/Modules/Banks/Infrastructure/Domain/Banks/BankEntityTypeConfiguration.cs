using App.Modules.Banks.Domain;
using App.Modules.Banks.Domain.Banks;
using App.Modules.Banks.Domain.ExternalProviders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.Banks.Infrastructure.Domain.Banks;

public class BankEntityTypeConfiguration : IEntityTypeConfiguration<Bank>
{
    public void Configure(EntityTypeBuilder<Bank> builder)
    {
        builder.ToTable("banks", "banks");

        builder.HasKey("Id", "ExternalId");
        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.ExternalId).HasColumnName("external_id");
        builder.Property(x => x.LastBanksSynchronisationProcessId).HasColumnName("last_banks_synchronisation_process_id");

        builder.Property<string>("_name").HasColumnName("name");
        builder.Property<bool>("_isRegulated").HasColumnName("is_regulated");
        builder.Property<int?>("_maxConsentDays").HasColumnName("max_consent_days");
        builder.Property<string?>("_logoUrl").HasColumnName("logo_url");
        builder.Property<string>("_defaultLogoUrl").HasColumnName("default_logo_url");
        builder.Property<DateTime>("_createdAt").HasColumnName("created_at");
        builder.Property<DateTime?>("_updatedAt").HasColumnName("updated_at");

        builder.OwnsOne<ExternalProviderName>("_externalProviderName", b =>
        {
            b.Property(x => x.Value).HasColumnName("external_provider_name");
        });

        builder.OwnsOne<Country>("_country", b =>
        {
            b.Property(x => x.Code).HasColumnName("country_code");
        });

        builder.OwnsOne<BankStatus>("_status", b =>
        {
            b.Property(x => x.Value).HasColumnName("status");
        });
    }
}
