using BoC.CmsHelpers.Sitecore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Habitat.Core.ViewModels.Metadata
{
    public class PageMetadata: SitecoreItem
    {
        public IEnumerable<Guid> MetaKeywords { get; set; }
    }
}
