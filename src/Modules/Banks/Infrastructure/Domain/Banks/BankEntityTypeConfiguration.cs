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

        builder.HasKey("Id");
        builder.HasIndex(x => x.ExternalId).IsUnique();

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

        builder.Property<ExternalProviderName>("_externalProviderName")
            .HasColumnName("external_provider_name")
            .HasConversion(
                n => n.Value,
                n => new ExternalProviderName(n));

        builder.Property<Country>("_country")
            .HasColumnName("country_code")
            .HasConversion(
                c => c.Code,
                c => Country.From(c));

        builder.Property<BankStatus>("_status")
            .HasColumnName("status")
            .HasConversion(
                s => s.Value,
                s => new BankStatus(s));
    }
}
