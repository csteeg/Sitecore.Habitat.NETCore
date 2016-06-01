using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Habitat.Core.ViewModels;
using BoC.CmsHelpers.Sitecore.Services;
using Habitat.Core.ViewModels.Metadata;

namespace Habitat.Core.Repositories.Metadata
{
    public class MetadataRepository : IMetadataRepository
    {
        private readonly ISitecoreService _sitecoreService;

        public MetadataRepository(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }

        public async Task<SiteMetadata> Get(Guid pageId)
        {
            var current = await _sitecoreService.Get<SiteMetadata>(pageId);
            while (current != null && !current.IsDerived(Templates.SiteMetadata.Id))
            {
                current = await _sitecoreService.Get<SiteMetadata>(current.ParentId);
            }
            return current;
        }

        public async Task<IEnumerable<string>> GetKeywords(Guid pageId)
        {
            var item = await _sitecoreService.Get<PageMetadata>(pageId);
            if (item.IsDerived(Templates.PageMetadata.Id))
            {
                var keywordsField = item.MetaKeywords;
                if (keywordsField == null)
                {
                    return Enumerable.Empty<string>();
                }

                return await Task.WhenAll(
                    keywordsField.AsParallel()
                        .Select(async keywrdItem =>
                        {
                            var kw = await _sitecoreService.Get<KeywordItem>(keywrdItem);
                            if (kw != null)
                                return kw.Keyword;
                            return null;
                        }).Where(kw => kw != null));
            }

            return Enumerable.Empty<string>();
        }
    }
}
