using App.BuildingBlocks.Infrastructure.Localization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace App.API.Configuration.Localization;

public class JsonStringLocalizerFactory(IDistributedCache cache) : ILocalizerFactory
{
    private readonly JsonSerializer _serializer = new();

    public IStringLocalizer Create<TResource>() where TResource : ILocalizationResource, new()
    {
        var resource = new TResource();

        return new JsonStringLocalizer(cache, _serializer, resource.Module);
    }

    public IStringLocalizer Create(Type resourceSource) => new JsonStringLocalizer(cache, _serializer);

    public IStringLocalizer Create(string baseName, string location) => new JsonStringLocalizer(cache, _serializer);
}
