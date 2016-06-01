using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BoC.CmsHelpers.Abstraction
{
    public static class HttpContextExtensions
    {
        public static bool IsInCmsMode(this HttpContext httpContext)
        {
            return httpContext?.Request?.Headers != null &&
                   httpContext.Request.Headers.ContainsKey("X-REQUESTED-WITH") &&
                   httpContext.Request.Headers["X-REQUESTED-WITH"].Any(v => v.ToLower().Contains("remote-cms"));
        }
    }
}
