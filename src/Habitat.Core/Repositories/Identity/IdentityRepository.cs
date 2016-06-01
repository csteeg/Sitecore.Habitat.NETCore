using System;
using System.Threading.Tasks;
using Habitat.Core.ViewModels.Identity;
using BoC.CmsHelpers.Sitecore.Services;

namespace Habitat.Core.Repositories.Identity
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly ISitecoreService _sitecoreService;

        public IdentityRepository(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }

        public async Task<ViewModels.Identity.Identity> Get(Guid currentPageId)
        {
            var current = await _sitecoreService.Get<ViewModels.Identity.Identity>(currentPageId);
            while (current != null && !current.IsDerived(Templates.IdentityTemplate.Id))
            {
                current = await _sitecoreService.Get<ViewModels.Identity.Identity>(current.ParentId);
            }
            return current;
        }
    }
}
