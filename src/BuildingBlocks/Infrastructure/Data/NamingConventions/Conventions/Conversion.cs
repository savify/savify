using App.BuildingBlocks.Infrastructure.Data.NamingConventions.NameRewriters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.BuildingBlocks.Infrastructure.Data.NamingConventions.Conventions;

internal static class Conversion
{
    private static readonly StoreObjectType[] StoreObjectTypes =
    {
        StoreObjectType.Table,
        StoreObjectType.View,
        StoreObjectType.Function,
        StoreObjectType.SqlQuery
    };

    internal static void RewriteColumnName(IConventionPropertyBuilder propertyBuilder, INameRewriter nameRewriter, bool ignoreMigrationsTable)
    {
        var property = propertyBuilder.Metadata;
        var structuralType = property.DeclaringType;

        property.Builder.HasNoAnnotation(RelationalAnnotationNames.ColumnName);

        if (ignoreMigrationsTable && property.DeclaringType.ClrType.FullName == nameof(Microsoft.EntityFrameworkCore.Migrations.HistoryRow))
        {
            return;
        }

        var baseColumnName = StoreObjectIdentifier.Create(structuralType, StoreObjectType.Table) is { } tableIdentifier
            ? property.GetDefaultColumnName(tableIdentifier)
            : property.GetDefaultColumnName();
        if (baseColumnName is not null)
        {
            propertyBuilder.HasColumnName(nameRewriter.RewriteName(baseColumnName));
        }

        foreach (var storeObjectType in StoreObjectTypes)
        {
            var identifier = StoreObjectIdentifier.Create(structuralType, storeObjectType);
            if (identifier is null)
            {
                continue;
            }

            if (property.GetColumnNameConfigurationSource(identifier.Value) == ConfigurationSource.Convention
                && property.GetColumnName(identifier.Value) is { } columnName)
            {
                propertyBuilder.HasColumnName(nameRewriter.RewriteName(columnName), identifier.Value);
            }
        }
    }
}
