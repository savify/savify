using App.BuildingBlocks.Infrastructure.InternalCommands;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.Wallets.Infrastructure.InternalCommands;

public class InternalCommandEntityTypeConfiguration : IEntityTypeConfiguration<InternalCommand>
{
    public void Configure(EntityTypeBuilder<InternalCommand> builder)
    {
        builder.ToTable("internal_commands", "wallets");
        
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasColumnName("id");
        builder.Property(b => b.Id).ValueGeneratedNever();

        builder.Property<string>("Type").HasColumnName("type");
        builder.Property<string>("Data").HasColumnName("data");
        builder.Property<DateTime>("EnqueueDate").HasColumnName("enqueue_date");
        builder.Property<DateTime?>("ProcessedDate").HasColumnName("processed_date");
        builder.Property<string?>("Error").HasColumnName("error");
    }
}
