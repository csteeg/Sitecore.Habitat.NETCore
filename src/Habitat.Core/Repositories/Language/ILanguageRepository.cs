using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sitecore.Feature.Language.Infrastructure.Repositories
{
    public interface ILanguageRepository
    {
        Task<IEnumerable<Habitat.Core.ViewModels.Language.Language>> GetSupportedLanguages(string siteName);
    }
}