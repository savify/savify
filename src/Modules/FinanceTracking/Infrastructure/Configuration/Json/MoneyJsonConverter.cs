using App.Modules.FinanceTracking.Domain.Finance;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace App.Modules.FinanceTracking.Infrastructure.Configuration.Json;

public class MoneyJsonConverter : JsonConverter<Money>
{
    public override Money? ReadJson(JsonReader reader, Type objectType, Money? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return null;

        var jsonObject = JObject.Load(reader);

        var amount = jsonObject.GetValue("Amount")!.Value<int>();
        var currency = jsonObject.GetValue("Currency")!.ToObject<Currency>(serializer)!;

        return Money.From(amount, currency);
    }

    public override void WriteJson(JsonWriter writer, Money? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        var jsonObject = new JObject
        {
            { "Amount", value.Amount },
            { "Currency", JToken.FromObject(value.Currency, serializer) }
        };

        jsonObject.WriteTo(writer);
    }
}
