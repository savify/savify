using App.Modules.Categories.Domain.CategoriesSynchronisationProcessing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.Categories.Infrastructure.Domain.CategoriesSynchronisationProcessing;

public class CategoriesSynchronisationProcessEntityTypeConfiguration : IEntityTypeConfiguration<CategoriesSynchronisationProcess>
{
    public void Configure(EntityTypeBuilder<CategoriesSynchronisationProcess> builder)
    {
        builder.ToTable("categories_synchronisation_processes");

        builder.HasKey(x => x.Id);

        builder.Property<DateTime>("_startedAt");
        builder.Property<DateTime?>("_finishedAt");

        builder.ComplexProperty<CategoriesSynchronisationProcessStatus>("_status", b =>
        {
            b.Property<string>(s => s.Value).HasColumnName("status");
        });
    }
}
