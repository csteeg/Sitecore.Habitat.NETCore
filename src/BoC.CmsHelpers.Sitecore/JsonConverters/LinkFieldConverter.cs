using System;
using System.Reflection;
using System.Xml.Linq;
using BoC.CmsHelpers.Abstraction.Models;
using BoC.CmsHelpers.Sitecore.Models;
using Newtonsoft.Json;

namespace BoC.CmsHelpers.Sitecore.JsonConverters
{
    public class LinkFieldConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var link = value as LinkField;
            if (link == null)
                return;
            var xelement = new XElement("image");
            xelement.SetAttributeValue("url", link.Url);
            xelement.SetAttributeValue("anchor", link.Anchor);
            xelement.SetAttributeValue("target", link.Target);
            xelement.SetAttributeValue("title", link.Title);

            writer.WriteValue(xelement.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value as string;
            if (string.IsNullOrEmpty(value) || !value.StartsWith("<link", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            var element = XElement.Parse(value);
            return new LinkField()
            {
                Url = element.Attribute("url")?.Value,
                Target = element.Attribute("target")?.Value,
                Anchor = element.Attribute("anchor")?.Value,
                Title = element.Attribute("title")?.Value
            };
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(LinkField).GetTypeInfo().IsAssignableFrom(objectType);
        }
    }
}