using App.BuildingBlocks.Infrastructure.Data.NamingConventions.NameRewriters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace App.BuildingBlocks.Infrastructure.Data.NamingConventions.Conventions;

internal class EntityTypeAnnotationChangedConvention(INameRewriter nameRewriter) : ConversionBase(nameRewriter), IEntityTypeAnnotationChangedConvention
{
    public void ProcessEntityTypeAnnotationChanged(
        IConventionEntityTypeBuilder entityTypeBuilder,
        string name,
        IConventionAnnotation? annotation,
        IConventionAnnotation? oldAnnotation,
        IConventionContext<IConventionAnnotation> context)
    {
        var entityType = entityTypeBuilder.Metadata;

        if (name is RelationalAnnotationNames.ViewName or RelationalAnnotationNames.SqlQuery or RelationalAnnotationNames.FunctionName
            && annotation?.Value is not null
            && entityType.GetTableNameConfigurationSource() == ConfigurationSource.Convention)
        {
            entityType.SetTableName(null);
        }

        if (name != RelationalAnnotationNames.TableName
            || StoreObjectIdentifier.Create(entityType, StoreObjectType.Table) is not { } tableIdentifier)
        {
            return;
        }

        if (entityType.FindPrimaryKey() is { } primaryKey)
        {
            var rootType = entityType.GetRootType();
            var isTpt = rootType.GetDerivedTypes().FirstOrDefault() is { } derivedType
                && derivedType.GetTableName() != rootType.GetTableName();

            if (entityType.FindRowInternalForeignKeys(tableIdentifier).FirstOrDefault() is null && !isTpt)
            {
                if (primaryKey.GetDefaultName() is { } primaryKeyName)
                {
                    primaryKey.Builder.HasName(nameRewriter.RewriteName(primaryKeyName));
                }
            }
            else
            {
                foreach (var type in entityType.GetRootType().GetDerivedTypesInclusive())
                {
                    if (type.FindPrimaryKey() is { } pk)
                    {
                        pk.Builder.HasNoAnnotation(RelationalAnnotationNames.Name);
                    }
                }
            }
        }

        foreach (var foreignKey in entityType.GetForeignKeys())
        {
            if (foreignKey.GetDefaultName() is { } foreignKeyName)
            {
                foreignKey.Builder.HasConstraintName(nameRewriter.RewriteName(foreignKeyName));
            }
        }

        foreach (var index in entityType.GetIndexes())
        {
            if (index.GetDefaultDatabaseName() is { } indexName)
            {
                index.Builder.HasDatabaseName(nameRewriter.RewriteName(indexName));
            }
        }

        if (annotation?.Value is not null
            && entityType.FindOwnership() is { } ownership
            && (string)annotation.Value != ownership.PrincipalEntityType.GetTableName())
        {
            foreach (var property in entityType.GetProperties()
                         .Except(entityType.FindPrimaryKey()?.Properties ?? Array.Empty<IConventionProperty>())
                         .Where(p => p.Builder.CanSetColumnName(null)))
            {
                RewriteColumnName(property.Builder);
            }

            if (entityType.FindPrimaryKey() is { } key
                && key.GetDefaultName() is { } keyName)
            {
                key.Builder.HasName(nameRewriter.RewriteName(keyName));
            }
        }
    }
}
