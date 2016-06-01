using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sitecore.Feature.Navigation.Repositories;

namespace Habitat.Core.Components.Navigation
{
    [ViewComponent(Name= "NavigationLinks: Sitecore.Feature.Navigation.Controllers.NavigationController, Sitecore.Feature.Navigation")]
    public class NavigationLinksViewComponent : ViewComponent
    {
        private readonly INavigationRepository _navigationRepository;
        public NavigationLinksViewComponent(INavigationRepository navigationRepository)
        {
            _navigationRepository = navigationRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            dynamic model = ViewData.Model;
            if (model == null)
                return null;

            var rootnav = await _navigationRepository.GetNavigationRoot((Guid)model.ItemID);
            if (rootnav == null)
                return null;
            var items = await _navigationRepository.GetLinkMenuItems(rootnav, (Guid)model.ItemID);
            if (items == null)
                return null;

            return View("NavigationLinks", await Task.WhenAll(items));
        }
    }
}
