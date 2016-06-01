using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BoC.CmsHelpers.Sitecore.Services
{
    public interface ISitecoreService
    {
        Task<T> Get<T>(Guid id);
        Task<T> Get<T>(string requestPath, Uri baseAddress = null);
        Task<IEnumerable<T>> GetChildren<T>(Guid id);
    }
}