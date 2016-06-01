using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sitecore.Feature.Navigation.Repositories;

namespace Habitat.Core.Components.Navigation
{
    [ViewComponent(Name= "LinkMenu: Sitecore.Feature.Navigation.Controllers.NavigationController, Sitecore.Feature.Navigation")]
    public class LinkMenuViewComponent : ViewComponent
    {
        private readonly INavigationRepository _navigationRepository;
        public LinkMenuViewComponent(INavigationRepository navigationRepository)
        {
            _navigationRepository = navigationRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            dynamic model = ViewData.Model;
            if (model == null)
            {
                //TODO: return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryRepository.Get("/navigation/linkmenu/noitems", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
                return null;
            }
            var pageId = ViewBag.Page.ItemID;
            var rootnav = await _navigationRepository.GetNavigationRoot((Guid)model.ItemID);
            if (rootnav == null)
                return null;
            var items = await _navigationRepository.GetLinkMenuItems(rootnav, (Guid)pageId);
            if (items == null)
                return null;

            return View("LinkMenu", await Task.WhenAll(items));
        }
    }
}
