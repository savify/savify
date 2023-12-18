using App.Modules.Banks.Domain;
using App.Modules.Banks.Domain.Banks;
using App.Modules.Banks.Domain.Banks.BankRevisions;
using App.Modules.Banks.Domain.ExternalProviders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.Banks.Infrastructure.Domain.Banks;

public class BankEntityTypeConfiguration : IEntityTypeConfiguration<Bank>
{
    public void Configure(EntityTypeBuilder<Bank> builder)
    {
        builder.ToTable("banks");

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.ExternalId).IsUnique();

        builder.Property(x => x.ExternalId);
        builder.Property(x => x.LastBanksSynchronisationProcessId);

        builder.Property<BankRevisionId>("CurrentRevisionId");

        builder.Property<string>("_name");
        builder.Property<bool>("_isRegulated");
        builder.Property<int?>("_maxConsentDays");
        builder.Property<string?>("_logoUrl");
        builder.Property<string>("_defaultLogoUrl");
        builder.Property<DateTime>("_createdAt");
        builder.Property<DateTime?>("_updatedAt");

        builder.ComplexProperty<ExternalProviderName>("_externalProviderName", b =>
        {
            b.Property<string>(n => n.Value).HasColumnName("external_provider_name");
        });

        builder.ComplexProperty<Country>("_country", b =>
        {
            b.Property<string>(c => c.Code).HasColumnName("country_code");
        });

        builder.ComplexProperty<BankStatus>("_status", b =>
        {
            b.Property<string>(s => s.Value).HasColumnName("status");
        });
    }
}
