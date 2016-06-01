using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BoC.CmsHelpers.Sitecore.Models;
using BoC.CmsHelpers.Abstraction.Models;

namespace Habitat.Core.ViewModels.Navigation
{

    public class NavigationItem : SitecoreItem
    {
        //public Item Item { get; set; }
        //public string Url { get; set; }
        public bool IsActive { get; set; }
        public int Level { get; set; }
        public string Target { get; set; }
        public bool ShowInNavigation { get; set; }
        public IEnumerable<NavigationItem> Children { get; set; }
        public string NavigationTitle { get; set; }

        //TODO: Subclass for these properties? -> Link & LinkMenuItem
        public bool DividerBefore { get; set; }
        public string Icon { get; set; }
        public LinkField Link { get; set; }
    }
}