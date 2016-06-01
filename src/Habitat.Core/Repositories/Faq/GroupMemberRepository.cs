using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoC.CmsHelpers.Sitecore.Services;
using Habitat.Core.ViewModels.Faq;

namespace Habitat.Core.Repositories.Faq
{
    public class GroupMemberRepository : IGroupMemberRepository
    {
        private readonly ISitecoreService _sitecoreService;

        public GroupMemberRepository(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }

        public IEnumerable<FaqItem> Get(Guid id)
        {
            var faqGroup = _sitecoreService.Get<FaqGroup>(id).Result;
            return Task.WhenAll(
                from guid in faqGroup.GroupMember.Split('|')
                select _sitecoreService.Get<FaqItem>(new Guid(guid))
            ).Result;
        }

    }
}
