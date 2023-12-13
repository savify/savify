using System.Globalization;
using App.BuildingBlocks.Infrastructure.Data.NamingConventions.Conventions;
using App.BuildingBlocks.Infrastructure.Data.NamingConventions.NameRewriters;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace App.BuildingBlocks.Infrastructure.Data.NamingConventions;

internal class NamingConventionSetPlugin(IDbContextOptions options) : IConventionSetPlugin
{
    public ConventionSet ModifyConventions(ConventionSet conventionSet)
    {
        var extension = options.FindExtension<NamingConventionsOptionsExtension>()!;
        var namingStyle = extension.NamingConvention;
        var culture = extension.Culture;
        var ignoreMigrationsTable = extension.IgnoreMigrationsTable;

        if (namingStyle == NamingConvention.None)
        {
            return conventionSet;
        }

        var namingRewriter = namingStyle switch
        {
            NamingConvention.SnakeCase => new SnakeCaseNameRewriter(culture ?? CultureInfo.InvariantCulture),
            _ => throw new ArgumentOutOfRangeException("Unhandled enum value: " + namingStyle)
        };

        conventionSet.EntityTypeAddedConventions.Add(new EntityTypeAddedConvention(namingRewriter));
        conventionSet.EntityTypeAnnotationChangedConventions.Add(new EntityTypeAnnotationChangedConvention(namingRewriter, ignoreMigrationsTable));
        conventionSet.PropertyAddedConventions.Add(new PropertyAddedConvention(namingRewriter, ignoreMigrationsTable));
        conventionSet.ForeignKeyOwnershipChangedConventions.Add(new ForeignKeyOwnershipChangedConvention(namingRewriter, ignoreMigrationsTable));
        conventionSet.KeyAddedConventions.Add(new KeyAddedConvention(namingRewriter));
        conventionSet.ForeignKeyAddedConventions.Add(new ForeignKeyAddedConvention(namingRewriter));
        conventionSet.IndexAddedConventions.Add(new IndexAddedConvention(namingRewriter));
        conventionSet.EntityTypeBaseTypeChangedConventions.Add(new EntityTypeTypeChangedConvention(namingRewriter));
        conventionSet.ModelFinalizingConventions.Add(new ModelFinalizingConvention(namingRewriter));

        return conventionSet;
    }
}
