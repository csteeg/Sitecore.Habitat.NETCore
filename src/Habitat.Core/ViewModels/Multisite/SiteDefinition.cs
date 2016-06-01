using System;
using System.Collections.Generic;
using Habitat.Core.ViewModels;
using BoC.CmsHelpers.Sitecore.Models;

namespace Sitecore.Foundation.Multisite
{

    public class SiteDefinition : SitecoreItem
    {
        public string HostName { get; set; }
        public string Title { get; set; }
        public bool IsCurrent { get; set; }
        public IEnumerable<Guid> SupportedLanguages { get; set; }
    }
}