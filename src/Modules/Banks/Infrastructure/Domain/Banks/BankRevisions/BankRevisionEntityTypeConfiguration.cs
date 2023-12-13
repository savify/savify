using App.Modules.Banks.Domain.Banks;
using App.Modules.Banks.Domain.Banks.BankRevisions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.Banks.Infrastructure.Domain.Banks.BankRevisions;

public class BankRevisionEntityTypeConfiguration : IEntityTypeConfiguration<BankRevision>
{
    public void Configure(EntityTypeBuilder<BankRevision> builder)
    {
        builder.ToTable("bank_revisions");

        builder.HasKey("Id", "BankId");

        builder.Property<BankId>("BankId");

        builder.Property<string>("_name");
        builder.Property<bool>("_isRegulated");
        builder.Property<int?>("_maxConsentDays");
        builder.Property<string?>("_logoUrl");
        builder.Property<string>("_defaultLogoUrl");
        builder.Property<DateTime>("_createdAt");

        builder.ComplexProperty<BankStatus>("_status", b =>
        {
            b.Property<string>(s => s.Value).HasColumnName("status");
        });

        builder.ComplexProperty<BankRevisionType>("_revisionType", b =>
        {
            b.Property<string>(r => r.Value).HasColumnName("revision_type");
        });

        builder.OwnsOne<BankRevisionCreator>("_createdBy", b =>
        {
            b.Property(c => c.Type).HasColumnName("created_by_type").HasConversion(
                t => t.Value,
                t => new BankRevisionCreatorType(t));

            b.Property(c => c.Id).HasColumnName("created_by_id");
        });
    }
}
