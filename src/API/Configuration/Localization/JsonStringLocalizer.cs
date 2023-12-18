using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace App.API.Configuration.Localization;

public class JsonStringLocalizer : IStringLocalizer
{
    private readonly IDistributedCache _cache;
    private readonly JsonSerializer _serializer;
    private readonly string? _module;

    public JsonStringLocalizer(IDistributedCache cache, JsonSerializer serializer)
    {
        _cache = cache;
        _serializer = serializer;
    }

    public JsonStringLocalizer(IDistributedCache cache, JsonSerializer serializer, string module)
    {
        _cache = cache;
        _serializer = serializer;
        _module = module;
    }

    public LocalizedString this[string name]
    {
        get
        {
            string? value = GetString(name);

            return new LocalizedString(name, value ?? name, value == null);
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var value = this[name];

            if (value.ResourceNotFound)
            {
                return value;
            }

            return new LocalizedString(name, string.Format(value.Value, arguments), false);
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        var filePath = ResolveFilePath();

        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var streamReader = new StreamReader(stream);
        using var reader = new JsonTextReader(streamReader);

        while (reader.Read())
        {
            if (reader.TokenType != JsonToken.PropertyName) continue;

            var key = (reader.Value as string)!;
            reader.Read();

            var value = _serializer.Deserialize<string>(reader)!;

            yield return new LocalizedString(key, value, false);
        }
    }

    private string? GetString(string key)
    {
        var currentCulture = Thread.CurrentThread.CurrentCulture.Name;

        var filePath = ResolveFilePath();
        if (!File.Exists(filePath)) return default;

        var cacheKey = $"locale_{currentCulture}_{key}";
        var cacheValue = _cache.GetString(cacheKey);

        if (!string.IsNullOrEmpty(cacheValue))
        {
            return cacheValue;
        }

        var result = GetValueFromJson(key, filePath);

        if (!string.IsNullOrEmpty(result))
        {
            _cache.SetString(cacheKey, result);
        }

        return result;

    }

    private string? GetValueFromJson(string propertyName, string filePath)
    {
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var streamReader = new StreamReader(stream);
        using var reader = new JsonTextReader(streamReader);

        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.PropertyName && reader.Value as string == propertyName)
            {
                reader.Read();

                return _serializer.Deserialize<string>(reader);
            }
        }

        return default;
    }

    private string ResolveFilePath()
    {
        var currentCulture = Thread.CurrentThread.CurrentCulture.Name;
        var relativeFilePath = $"Resources/{currentCulture}.json";

        if (_module != null)
        {
            relativeFilePath = $"Resources/Modules/{_module}/{currentCulture}.json";
        }

        return Path.GetFullPath(relativeFilePath);
    }
}
