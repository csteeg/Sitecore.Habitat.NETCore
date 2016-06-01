using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Configuration;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Pipelines.Response.GetRenderer;
using Sitecore.Mvc.Presentation;
using Sitecore.Web;

namespace BoC.Sitecore.ApiExtensions.Pipelines
{
    public class GetViewOrUrlRenderer: GetViewRenderer
    {
        protected override Renderer GetRenderer(global::Sitecore.Mvc.Presentation.Rendering rendering, GetRendererArgs args)
        {
            string viewPath = this.GetViewPath(rendering, args);
            if (viewPath.IsWhiteSpaceOrNull())
                return (Renderer)null;
            if (WebUtil.IsExternalUrl(viewPath))
            {
                if (viewPath.Contains("{0}") && args.PageContext != null && args.PageContext.RequestContext != null &&
                    args.PageContext.RequestContext.HttpContext != null &&
                    args.PageContext.RequestContext.HttpContext.Request != null)
                {
                    var pathAndQuery = args.PageContext.RequestContext.HttpContext.Request.Url.PathAndQuery;
                    if (pathAndQuery.Contains("?") && viewPath.Contains("?"))
                    {
                        viewPath = ReplaceFirst(viewPath, "?", "&");
                    }
                    viewPath = string.Format(viewPath, pathAndQuery);
                }
                return new UrlWithCookiesRenderer()
                {
                    Url = viewPath
                };
            }
            return (Renderer)new ViewRenderer()
            {
                ViewPath = viewPath,
                Rendering = rendering
            };
        }

        string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
    }

    public class UrlWithCookiesRenderer : UrlRenderer
    {
        public override void Render(TextWriter writer)
        {
            string url = this.Url;
            if (string.IsNullOrWhiteSpace(url))
                return;
            string cookie = null;
            var context = HttpContext.Current;
            if (context != null)
            {
                cookie = context.Request.Headers["Cookie"];
            }
            var webClient = new WebClient();
            webClient.Headers.Add("X-REQUESTED-WITH", "Remote-CMS, Sitecore");
            if (cookie != null)
            {
                if (global::Sitecore.Context.PageMode.IsNormal)
                {
                    webClient.Headers.Add(HttpRequestHeader.Cookie, cookie);
                }
                else
                {
                    CookieHeaderValue cookieHeader;
                    if (CookieHeaderValue.TryParse(cookie, out cookieHeader))
                    {
                        //remove analytics and session cookie, httphandler of mvc/sitecore has IRequireSessionState, leading to a deadlock on the sessionstate:
                        //http://stackoverflow.com/questions/2526168/asp-net-ihttpasynchandler-and-irequiressessionstate-not-working
                        var sessionCookieName = GetSessionIdCookieName();
                        var analyticscookies = cookieHeader.Cookies.Where(c =>
                            c.Name == sessionCookieName ||
                            c.Name.StartsWith("SC_ANALYTICS_GLOBAL_COOKIE")).ToArray();
                        foreach (var c in analyticscookies)
                        {
                            cookieHeader.Cookies.Remove(c);
                        }
                        webClient.Headers.Add(HttpRequestHeader.Cookie, cookieHeader.ToString());
                    }
                    else
                    {
                        webClient.Headers.Add(HttpRequestHeader.Cookie, cookie);
                    }
                }
            }
            var result = webClient.DownloadString(url);
            
            if (string.IsNullOrWhiteSpace(result))
                return;
            writer.Write(result);
        }

        internal string GetSessionIdCookieName()
        {
            SessionStateSection sessionStateSection = (SessionStateSection)ConfigurationManager.GetSection("system.web/sessionState");
            return sessionStateSection != null ? sessionStateSection.CookieName : "ASP.NET_SessionId";
        }
    }
}