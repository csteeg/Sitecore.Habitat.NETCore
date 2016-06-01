using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Habitat.Core.ViewModels;
using Habitat.Core.ViewModels.Metadata;

namespace Habitat.Core.Repositories.Metadata
{
    public interface IMetadataRepository
    {
        Task<SiteMetadata> Get(Guid pageId);
        Task<IEnumerable<string>> GetKeywords(Guid pageId);
    }
}