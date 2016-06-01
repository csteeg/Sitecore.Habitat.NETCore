using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Habitat.Core.ViewModels.Faq
{
    public class FaqGroup
    {
        [JsonProperty("Group Member")]
        public virtual string GroupMember { get; set; }
    }
}
