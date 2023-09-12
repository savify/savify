using App.Modules.Banks.Domain.BanksSynchronisationProcessing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.Banks.Infrastructure.Domain.BanksSynchronisationProcessing;

public class BanksSynchronisationProcessEntityTypeConfiguration : IEntityTypeConfiguration<BanksSynchronisationProcess>
{
    public void Configure(EntityTypeBuilder<BanksSynchronisationProcess> builder)
    {
        builder.ToTable("banks_synchronisation_processes", "banks");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property<DateTime>("_startedAt").HasColumnName("started_at");
        builder.Property<DateTime?>("_finishedAt").HasColumnName("finished_at");

        builder.OwnsOne<BanksSynchronisationProcessStatus>("_status", b =>
        {
            b.Property(x => x.Value).HasColumnName("status");
        });

        builder.OwnsOne<BanksSynchronisationProcessInitiator>("_initiatedBy", b =>
        {
            b.OwnsOne<BanksSynchronisationProcessInitiatorType>(x => x.Type, t =>
            {
                t.Property(x => x.Value).HasColumnName("initiated_by_type").IsRequired();
            });

            b.Property(x => x.UserId).HasColumnName("initiated_by_user_id");
        }).Navigation("_initiatedBy").IsRequired();
    }
}
