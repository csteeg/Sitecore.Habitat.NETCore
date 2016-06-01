using System;
using System.Reflection;
using System.Xml.Linq;
using BoC.CmsHelpers.Abstraction.Models;
using BoC.CmsHelpers.Sitecore.Models;
using Newtonsoft.Json;

namespace BoC.CmsHelpers.Sitecore.JsonConverters
{
    public class ImageFieldConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var image = value as ImageField;
            if (image == null)
                return;
            var xelement = new XElement("image");
            xelement.SetAttributeValue("mediapath", image.MediaPath);
            xelement.SetAttributeValue("alt", image.Alt);
            xelement.SetAttributeValue("hspace", image.HSpace);
            xelement.SetAttributeValue("vspace", image.VSpace);
            xelement.SetAttributeValue("mediaid", image.MediaId.ToString("B"));
            xelement.SetAttributeValue("src", image.Src);

            writer.WriteValue(xelement.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value as string;
            if (string.IsNullOrEmpty(value) || !value.StartsWith("<image", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            var element = XElement.Parse(value);
            var height = element.Attribute("height")?.Value;
            var width = element.Attribute("width")?.Value;
            return new ImageField()
            {
                MediaId = Guid.Parse(element.Attribute("mediaid")?.Value ?? Guid.Empty.ToString()),
                Alt = element.Attribute("alt")?.Value,
                Src = element.Attribute("src")?.Value,
                HSpace = element.Attribute("hspace")?.Value,
                MediaPath = element.Attribute("mediapath")?.Value,
                Height = string.IsNullOrEmpty(height) ? (int?)null : int.Parse(height),
                VSpace = element.Attribute("vspace")?.Value,
                Width = string.IsNullOrEmpty(width) ? (int?)null : int.Parse(width)
            };
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(ImageField).GetTypeInfo().IsAssignableFrom(objectType);
        }
    }
}