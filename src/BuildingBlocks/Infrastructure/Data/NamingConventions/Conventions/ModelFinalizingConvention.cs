using App.BuildingBlocks.Infrastructure.Data.NamingConventions.NameRewriters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace App.BuildingBlocks.Infrastructure.Data.NamingConventions.Conventions;

internal class ModelFinalizingConvention(INameRewriter nameRewriter) : IModelFinalizingConvention
{
    public void ProcessModelFinalizing(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
    {
        foreach (var entityType in modelBuilder.Metadata.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                var columnName = property.GetColumnName();
                if (columnName.StartsWith(entityType.ShortName() + '_', StringComparison.Ordinal))
                {
                    property.Builder.HasColumnName(
                        nameRewriter.RewriteName(entityType.ShortName()) + columnName.Substring(entityType.ShortName().Length));
                }

                var storeObject = StoreObjectIdentifier.Create(entityType, StoreObjectType.Table);
                if (storeObject is null)
                {
                    continue;
                }

                var shortName = entityType.ShortName();

                if (property.Builder.CanSetColumnName(null))
                {
                    columnName = property.GetColumnName();
                    if (columnName.StartsWith(shortName + '_', StringComparison.Ordinal))
                    {
                        property.Builder.HasColumnName(
                            nameRewriter.RewriteName(shortName)
                            + columnName.Substring(shortName.Length));
                    }
                }

                if (property.Builder.CanSetColumnName(null, storeObject.Value))
                {
                    columnName = property.GetColumnName(storeObject.Value);
                    if (columnName is not null && columnName.StartsWith(shortName + '_', StringComparison.Ordinal))
                    {
                        property.Builder.HasColumnName(
                            nameRewriter.RewriteName(shortName) + columnName.Substring(shortName.Length),
                            storeObject.Value);
                    }
                }
            }
        }
    }
}
