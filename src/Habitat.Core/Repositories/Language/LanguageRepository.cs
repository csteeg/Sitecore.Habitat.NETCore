using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Habitat.Core.Repositories.Multisite;
using Habitat.Core.ViewModels.Language;
using BoC.CmsHelpers.Sitecore.Services;

namespace Sitecore.Feature.Language.Infrastructure.Repositories
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly IMultisiteRepository _multisiteRepository;
        private readonly ISitecoreService _sitecoreService;

        public LanguageRepository(IMultisiteRepository multisiteRepository, ISitecoreService sitecoreService)
        {
            _multisiteRepository = multisiteRepository;
            _sitecoreService = sitecoreService;
        }

        public async Task<IEnumerable<Habitat.Core.ViewModels.Language.Language>> GetSupportedLanguages(string siteName)
        {
            var siteDefinition = (await _multisiteRepository.GetAll()).FirstOrDefault(s => (bool)s?.Name?.Equals(siteName, StringComparison.OrdinalIgnoreCase));
            if (siteDefinition == null)
            {
                return Enumerable.Empty<Habitat.Core.ViewModels.Language.Language>();
            }

            return await Task.WhenAll(
                siteDefinition.SupportedLanguages.AsParallel().Select(async guid => await _sitecoreService.Get<Habitat.Core.ViewModels.Language.Language>(guid)));
        }
    }
}