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
    [ViewComponent(Name= "SecondaryMenu: Sitecore.Feature.Navigation.Controllers.NavigationController, Sitecore.Feature.Navigation")]
    public class SecondaryMenuViewComponent: ViewComponent
    {
        private readonly INavigationRepository _navigationRepository;

        public SecondaryMenuViewComponent(INavigationRepository navigationRepository)
        {
            _navigationRepository = navigationRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            dynamic model = ViewData.Model;
            var result = await _navigationRepository.GetSecondaryMenuItem((Guid)model.ItemID);
            return View("SecondaryMenu", result);
        }
    }
}
