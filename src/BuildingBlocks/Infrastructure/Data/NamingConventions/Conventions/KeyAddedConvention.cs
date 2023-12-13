using App.BuildingBlocks.Infrastructure.Data.NamingConventions.NameRewriters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace App.BuildingBlocks.Infrastructure.Data.NamingConventions.Conventions;

internal class KeyAddedConvention(INameRewriter nameRewriter) : IKeyAddedConvention
{
    public void ProcessKeyAdded(IConventionKeyBuilder keyBuilder, IConventionContext<IConventionKeyBuilder> context)
    {
        if (keyBuilder.Metadata.GetName() is { } keyName)
        {
            keyBuilder.HasName(nameRewriter.RewriteName(keyName));
        }
    }
}
