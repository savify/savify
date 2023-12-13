using App.BuildingBlocks.Infrastructure.Data.NamingConventions.NameRewriters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace App.BuildingBlocks.Infrastructure.Data.NamingConventions.Conventions;

internal class IndexAddedConvention(INameRewriter nameRewriter) : IIndexAddedConvention
{
    public void ProcessIndexAdded(IConventionIndexBuilder indexBuilder, IConventionContext<IConventionIndexBuilder> context)
    {
        if (indexBuilder.Metadata.GetDefaultDatabaseName() is { } indexName)
        {
            indexBuilder.HasDatabaseName(nameRewriter.RewriteName(indexName));
        }
    }
}
