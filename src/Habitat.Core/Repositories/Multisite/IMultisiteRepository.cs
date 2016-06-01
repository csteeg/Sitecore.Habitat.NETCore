using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sitecore.Foundation.Multisite;

namespace Habitat.Core.Repositories.Multisite
{
    public interface IMultisiteRepository
    {
        Task<IEnumerable<SiteDefinition>> GetAll();
        Task<SiteDefinition> Get(Guid siteId);
    }
}