using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sitecore.Foundation.Multisite;
using Microsoft.Extensions.Options;
using BoC.CmsHelpers.Sitecore.Services;
using BoC.CmsHelpers.Sitecore;
using System.Linq;

namespace Habitat.Core.Repositories.Multisite
{
    public class MultisiteRepository : IMultisiteRepository
    {
        private readonly ISitecoreService _sitecoreService;
        private ApiSettings _apiSettings;

        public MultisiteRepository(ISitecoreService sitecoreService, IOptions<ApiSettings> apiSettings)
        {
            _sitecoreService = sitecoreService;
            _apiSettings = apiSettings.Value;
        }

        public async Task<IEnumerable<SiteDefinition>> GetAll()
        {
            var children = await _sitecoreService.GetChildren<SiteDefinition>(SitecoreItemIds.ContentRoot);
            foreach (var child in children)
            {
                child.IsCurrent = (child.Name == _apiSettings.ScSiteName);
            }
            return children.Where(i => i.IsDerived(Templates.Site.Id));
        }

        public async Task<SiteDefinition> Get(Guid siteId)
        {
            return await _sitecoreService.Get<SiteDefinition>(siteId);
        }
    }
}
