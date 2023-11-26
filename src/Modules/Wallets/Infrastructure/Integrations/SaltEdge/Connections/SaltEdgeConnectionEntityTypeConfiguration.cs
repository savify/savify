using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Modules.Wallets.Infrastructure.Integrations.SaltEdge.Connections;

public class SaltEdgeConnectionEntityTypeConfiguration : IEntityTypeConfiguration<SaltEdgeConnection>
{
    public void Configure(EntityTypeBuilder<SaltEdgeConnection> builder)
    {
        builder.ToTable("salt_edge_connections");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property<Guid>("InternalConnectionId").HasColumnName("internal_connection_id");
        builder.Property<string>("ProviderCode").HasColumnName("provider_code");
        builder.Property<string>("CountryCode").HasColumnName("country_code");
        builder.Property<string>("LastConsentId").HasColumnName("last_consent_id");
        builder.Property<string>("CustomerId").HasColumnName("customer_id");
        builder.Property<string>("Status").HasColumnName("status");
    }
}
