using System;
using System.Threading.Tasks;

namespace Habitat.Core.Repositories.Identity
{
    public interface IIdentityRepository
    {
        Task<ViewModels.Identity.Identity> Get(Guid currentPageId);
    }
}