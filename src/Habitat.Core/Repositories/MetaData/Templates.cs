using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Habitat.Core.Repositories.Metadata
{
    public struct Templates
    {
        public struct SiteMetadata
        {
            public static readonly Guid Id = new Guid("{CF38E914-9298-47CC-9205-210553E79F97}");
        }

        public struct PageMetadata
        {
            public static readonly Guid Id = new Guid("{D88CCD80-D851-470D-AF11-701FF23504E7}");
        }
    }
}
