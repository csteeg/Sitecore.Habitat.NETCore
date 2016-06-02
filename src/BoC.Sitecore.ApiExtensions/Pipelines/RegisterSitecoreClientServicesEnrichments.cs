using System.Web.Http;
using Sitecore.Pipelines;

namespace BoC.Sitecore.ApiExtensions.Pipelines
{
    public class RegisterSitecoreClientServicesEnrichments
    {
        public void Process(PipelineArgs args)
        {
            GlobalConfiguration.Configuration.Filters.Add(new ItemModelActionFilter());
        }
    }
}