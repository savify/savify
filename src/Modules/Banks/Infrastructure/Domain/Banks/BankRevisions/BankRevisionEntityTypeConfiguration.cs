using App.Modules.Banks.Domain.Banks;
using App.Modules.Banks.Domain.Banks.BankRevisions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.Banks.Infrastructure.Domain.Banks.BankRevisions;

public class BankRevisionEntityTypeConfiguration : IEntityTypeConfiguration<BankRevision>
{
    public void Configure(EntityTypeBuilder<BankRevision> builder)
    {
        builder.ToTable("bank_revisions", "banks");

        builder.HasKey("Id", "BankId");
        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property<BankId>("BankId").HasColumnName("bank_id");

        builder.Property<string>("_name").HasColumnName("name");
        builder.Property<bool>("_isRegulated").HasColumnName("is_regulated");
        builder.Property<int?>("_maxConsentDays").HasColumnName("max_consent_days");
        builder.Property<string?>("_logoUrl").HasColumnName("logo_url");
        builder.Property<string>("_defaultLogoUrl").HasColumnName("default_logo_url");
        builder.Property<DateTime>("_createdAt").HasColumnName("created_at");

        builder.Property<BankStatus>("_status")
            .HasColumnName("status")
            .HasConversion(
                s => s.Value,
                s => new BankStatus(s));

        builder.Property<BankRevisionType>("_revisionType")
            .HasColumnName("revision_type")
            .HasConversion(
                r => r.Value,
                r => new BankRevisionType(r));

        builder.OwnsOne<BankRevisionCreator>("_createdBy", b =>
        {
            b.Property(c => c.Type).HasColumnName("created_by_type").HasConversion(
                t => t.Value,
                t => new BankRevisionCreatorType(t));

            b.Property(c => c.Id).HasColumnName("created_by_id");
        });
    }
}
