using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BoC.CmsHelpers.Sitecore.JsonConverters
{
    public class MultiListConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is IEnumerable<Guid>)
                writer.WriteValue(String.Join("|", ((IEnumerable<Guid>)value).Select(g => g.ToString("B"))));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                var jarray = serializer.Deserialize(reader) as JArray;
                return jarray.Values<string>().Select(s => new Guid(s));
            }
            var value = reader.Value as string;
            if (string.IsNullOrEmpty(value))
                return Enumerable.Empty<Guid>();

            return value.Split('|').Select(s => new Guid(s));
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IEnumerable<Guid>).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }
    }
}