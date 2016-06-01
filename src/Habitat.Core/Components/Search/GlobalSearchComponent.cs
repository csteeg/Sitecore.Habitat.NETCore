using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Sitecore.Feature.Navigation.Repositories;

namespace Habitat.Core.Components.Navigation
{
    [ViewComponent(Name= "GlobalSearch: Sitecore.Feature.Search.Controllers.SearchController, Sitecore.Feature.Search")]
    public class GlobalSearchComponent: ViewComponent
    {
        public GlobalSearchComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return Content("");//TODO: Implement component
        }
    }
}
