using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace BoC.CmsHelpers.Abstraction
{
    public class FullUrlHelper : UrlHelper
    {
        public FullUrlHelper(ActionContext actionContext) : base(actionContext)
        {
        }

        public override string Content(string contentPath)
        {
            return expandUrl(base.Content(contentPath));
        }

        private string expandUrl(string contentPath)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(HttpContext.Request.Scheme);
            stringBuilder.Append("://");
            stringBuilder.Append(HttpContext.Request.Host.Value);
            stringBuilder.Append(contentPath);
            return stringBuilder.ToString();
        }

        public override string Action(UrlActionContext actionContext)
        {
            return expandUrl(base.Action(actionContext));
        }

        public override string Link(string routeName, object values)
        {
            return expandUrl(base.Link(routeName, values));
        }

        public override string RouteUrl(UrlRouteContext routeContext)
        {
            return expandUrl(base.RouteUrl(routeContext));
        }
    }
}
