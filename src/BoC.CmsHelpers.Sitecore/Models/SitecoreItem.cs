using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BoC.CmsHelpers.Sitecore.Models
{
    public class SitecoreItem
    {
        [JsonProperty("ItemID")]
        public virtual Guid Id { get; set; }
        public virtual Guid ParentId { get; set; }
        public virtual Guid TemplateId { get; set; }
        public virtual IEnumerable<Guid> TemplateIds { get; set; }
        public virtual IEnumerable<Guid> ParentIds { get; set; }
        [JsonProperty("ItemPath")]
        public virtual string FullPath { get; set; }
        [JsonProperty("ItemName")]
        public virtual string Name { get; set; }
        [JsonProperty("ItemUrlExpanded")]
        public virtual string Url { get; set; }
        public virtual bool HasChildren { get; set; }
        public virtual JObject FieldEditors { get; set; }

        public virtual bool IsDerived(Guid templateId)
        {
            return TemplateIds?.Contains(templateId) ?? false;
        }
    }
}
