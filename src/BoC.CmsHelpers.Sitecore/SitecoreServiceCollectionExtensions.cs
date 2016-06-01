using BoC.CmsHelpers.Abstraction;
using BoC.CmsHelpers.Sitecore.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BoC.CmsHelpers.Sitecore
{
    public static class SitecoreServiceCollectionExtensions
    {
        public static void AddSitecore(this IServiceCollection services)
        {
            services.AddScoped<IPlaceholderRenderer, PlaceholderRenderer>();
            services.AddTransient<ISitecoreService, SitecoreService>();
        }
    }
}
