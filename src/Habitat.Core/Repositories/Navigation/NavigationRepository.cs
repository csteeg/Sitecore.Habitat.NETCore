using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Habitat.Core.ViewModels;
using Sitecore.Feature.Navigation.Repositories;
using BoC.CmsHelpers.Sitecore.Services;
using Habitat.Core.ViewModels.Navigation;

namespace Habitat.Core.Repositories.Navigation
{

    public class NavigationRepository : INavigationRepository
    {
        private readonly ISitecoreService _sitecoreService;

        public NavigationRepository(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }

        public async Task<NavigationItem> GetNavigationRoot(Guid currentPageId)
        {
            var currentItem = await _sitecoreService.Get<NavigationItem>(currentPageId);
            while (currentItem != null && !currentItem.IsDerived(Templates.NavigationRoot.Id))
            {
                currentItem = await _sitecoreService.Get<NavigationItem>(new Guid(currentItem.ParentId.ToString()));
            }
            return currentItem;
            //return contextItem.GetAncestorOrSelfOfTemplate(Templates.NavigationRoot.ID) ??
            //       Sitecore.Context.Site.GetContextItem(Templates.NavigationRoot.ID);
        }

        public async Task<IEnumerable<NavigationItem>> GetBreadcrumb(Guid currentPageId)
        {
            var currentItem = await _sitecoreService.Get<NavigationItem>(currentPageId);

            var items = await this.GetNavigationHierarchy(currentItem, true);
            items = items.Reverse().ToList();
            var i = -1;
            foreach (var item in items)
            {
                item.IsActive = false;
                item.Level = ++i;
            }
            items.Last().IsActive = true;
            //for (var i = 0; i < items.Count() - 1; i++)
            //{
            //    items[i].Level = i;
            //    items[i].IsActive = i == (items.Count() - 1);
            //}

            return items;
        }

        public async Task<IEnumerable<NavigationItem>> GetPrimaryMenu(Guid currentPageId)
        {
            var navigationRoot = await GetNavigationRoot(currentPageId);
            var currentPage = await _sitecoreService.Get<NavigationItem>(currentPageId);
            var navItems = await this.GetChildNavigationItems(navigationRoot, currentPage, 0, 1);

            return await this.AddRootToPrimaryMenu(navItems, navigationRoot, currentPage);
        }

        private async Task<IEnumerable<NavigationItem>> AddRootToPrimaryMenu(IEnumerable<Task<NavigationItem>> navItems, NavigationItem navigationRoot, NavigationItem currentPage)
        {
            if (navItems == null)
                return null;
            if (!this.IncludeInNavigation(navigationRoot))
            {
                return await Task.WhenAll(navItems);
            }
            var navigationItem = await this.CreateNavigationItem(navigationRoot, currentPage, 0, 0);
            //Root navigation item is only active when we are actually on the root item
            navigationItem.IsActive = currentPage.Id == navigationRoot.Id;
            var items = (await Task.WhenAll(navItems)).ToList();
            items.Insert(0, navigationItem);
            return items;
        }

        private bool IncludeInNavigation(NavigationItem item, bool forceShowInMenu = false)
        {
            return item!= null && item.IsDerived(Templates.Navigable.Id) && (forceShowInMenu || item.ShowInNavigation);
        }

        public async Task<NavigationItem> GetSecondaryMenuItem(Guid currentPageId)
        {
            var rootItem = await this.GetSecondaryMenuRoot(currentPageId);
            var currentPage = await _sitecoreService.Get<NavigationItem>(currentPageId);
            return rootItem == null ? null : await this.CreateNavigationItem(rootItem, currentPage, 0, 3);
        }

        public async Task<IEnumerable<Task<NavigationItem>>> GetLinkMenuItems(NavigationItem menuRoot, Guid currentPageId)
        {
            if (menuRoot == null)
            {
                throw new ArgumentNullException(nameof(menuRoot));
            }

            var currentPage = await _sitecoreService.Get<NavigationItem>(currentPageId);
            return await this.GetChildNavigationItems(menuRoot, currentPage, 0, 0);
        }

        private async Task<NavigationItem> GetSecondaryMenuRoot(Guid currentPageId)
        {
            return await this.FindActivePrimaryMenuItem(currentPageId);
        }

        private async Task<NavigationItem> FindActivePrimaryMenuItem(Guid currentPageId)
        {
            var navigationRoot = await GetNavigationRoot(currentPageId);
            var primaryMenuItems = await this.GetPrimaryMenu(currentPageId);
            //Find the active primary menu item
            return primaryMenuItems?.FirstOrDefault(i => i.Id != navigationRoot.Id && i.IsActive);
        }

        private async Task<IEnumerable<NavigationItem>> GetNavigationHierarchy(NavigationItem page, bool forceShowInMenu = false)
        {
            var result = new List<Task<NavigationItem>>();
            var item = page;
            while (item != null)
            {
                if (this.IncludeInNavigation(item, forceShowInMenu))
                {
                    result.Add(CreateNavigationItem(item, page, 0));
                }

                if (item.ParentId == Guid.Empty)
                    break;
                item = await _sitecoreService.Get<NavigationItem>(item.ParentId);
            }
            return await Task.WhenAll(result);
        }

        private async Task<NavigationItem> CreateNavigationItem(NavigationItem item, NavigationItem currentPage, int level, int maxLevel = -1)
        {
            item.IsActive = this.IsItemActive(item, currentPage);
            item.Children = await Task.WhenAll(await this.GetChildNavigationItems(item, currentPage, level + 1, maxLevel));
            return item;
        }

        private async Task<IEnumerable<Task<NavigationItem>>> GetChildNavigationItems(NavigationItem parentItem, NavigationItem currentPage, int level, int maxLevel)
        {
            if (level > maxLevel || parentItem == null || !parentItem.HasChildren)
            {
                return Enumerable.Empty<Task<NavigationItem>>();
            }
            var children = await _sitecoreService.GetChildren<NavigationItem>(parentItem.Id);
            return children.Where(item => this.IncludeInNavigation(item)).Select(i => CreateNavigationItem(i, currentPage, level, maxLevel));
        }

        private bool IsItemActive(NavigationItem item, NavigationItem currentPage)
        {
            return currentPage.Id == item.Id || currentPage.ParentIds.Contains(item.Id);
        }
    }
}