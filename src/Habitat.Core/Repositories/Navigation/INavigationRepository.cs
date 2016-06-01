using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Habitat.Core.ViewModels;
using Habitat.Core.ViewModels.Navigation;

namespace Sitecore.Feature.Navigation.Repositories
{
    public interface INavigationRepository
    {
        Task<NavigationItem> GetNavigationRoot(Guid currentPageId);
        Task<IEnumerable<NavigationItem>> GetBreadcrumb(Guid currentPageId);
        Task<IEnumerable<NavigationItem>> GetPrimaryMenu(Guid currentPageId);
        Task<NavigationItem> GetSecondaryMenuItem(Guid currentPageId);
        Task<IEnumerable<Task<NavigationItem>>> GetLinkMenuItems(NavigationItem menuRoot, Guid currentPageId);
    }
}