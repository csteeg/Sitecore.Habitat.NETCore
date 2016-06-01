using System;
using System.Collections.Generic;
using Habitat.Core.ViewModels.Faq;

namespace Habitat.Core.Repositories.Faq
{
    public interface IGroupMemberRepository
    {
        IEnumerable<FaqItem> Get(Guid id);
    }
}