using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace BoC.CmsHelpers.Sitecore.Models
{
    public class PageData
    {
        public string FullPath { get; set; }
        public IEnumerable<Rendering> Renderings { get; set; }
        public JObject Page { get; set; }
        public JObject RenderingParameters { get; set; }
    }

    public class Rendering
    {
        public JObject RenderingItem { get; set; }
        //public Dictionary<string, string> Parameters { get; set; }
        public string Placeholder { get; set; }
        public string DataSource { get; set; }
        public Guid UniqueId { get; set; }
        public RenderingChrome? RenderingChrome { get; set; }

    }

}
