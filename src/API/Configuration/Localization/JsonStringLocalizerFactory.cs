using App.BuildingBlocks.Infrastructure.Localization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace App.API.Configuration.Localization;

public class JsonStringLocalizerFactory : ILocalizerFactory
{
    private readonly IDistributedCache _cache;
    private readonly JsonSerializer _serializer;

    public JsonStringLocalizerFactory(IDistributedCache cache)
    {
        _cache = cache;
        _serializer = new JsonSerializer();
    }

    public IStringLocalizer Create<TResource>() where TResource : ILocalizationResource, new ()
    {
        var resource = new TResource();

        return new JsonStringLocalizer(_cache, _serializer, resource.Module);
    }

    public IStringLocalizer Create(Type resourceSource) => new JsonStringLocalizer(_cache, _serializer);

    public IStringLocalizer Create(string baseName, string location) => new JsonStringLocalizer(_cache, _serializer);
}
