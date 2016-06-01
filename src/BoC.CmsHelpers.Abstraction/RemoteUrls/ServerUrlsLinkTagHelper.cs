using System;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Caching.Memory;

namespace BoC.CmsHelpers.Abstraction
{
    [HtmlTargetElement("link", Attributes = "asp-href-include", TagStructure = TagStructure.WithoutEndTag)]
    [HtmlTargetElement("link", Attributes = "asp-href-exclude", TagStructure = TagStructure.WithoutEndTag)]
    [HtmlTargetElement("link", Attributes = "asp-fallback-href", TagStructure = TagStructure.WithoutEndTag)]
    [HtmlTargetElement("link", Attributes = "asp-fallback-href-include", TagStructure = TagStructure.WithoutEndTag)]
    [HtmlTargetElement("link", Attributes = "asp-fallback-href-exclude", TagStructure = TagStructure.WithoutEndTag)]
    [HtmlTargetElement("link", Attributes = "asp-fallback-test-class", TagStructure = TagStructure.WithoutEndTag)]
    [HtmlTargetElement("link", Attributes = "asp-fallback-test-property", TagStructure = TagStructure.WithoutEndTag)]
    [HtmlTargetElement("link", Attributes = "asp-fallback-test-value", TagStructure = TagStructure.WithoutEndTag)]
    [HtmlTargetElement("link", Attributes = "asp-append-version", TagStructure = TagStructure.WithoutEndTag)]
    public class ServerUrlsLinkTagHelper : LinkTagHelper
    {
        static Regex hrefRegex = new Regex("href=\"([^\"]+)\"", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public ServerUrlsLinkTagHelper(IHostingEnvironment hostingEnvironment, IMemoryCache cache, HtmlEncoder htmlEncoder, JavaScriptEncoder javaScriptEncoder, IUrlHelperFactory urlHelperFactory) : base(hostingEnvironment, cache, htmlEncoder, javaScriptEncoder, urlHelperFactory)
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);
            var urlHelper = UrlHelperFactory.GetUrlHelper(ViewContext) as FullUrlHelper;
            if (urlHelper == null)
                return;
            
            if (output.PostElement != null && !output.PostElement.IsEmptyOrWhiteSpace)
            {
                var content = output.PostElement.GetContent();
                output.PostElement.SetHtmlContent(hrefRegex.Replace(content,
                    match => "href=\"" + urlHelper.Content(match.Groups[1].Value) + "\""));
            }

        }
    }

    //// This project can output the Class library as a NuGet Package.
    //// To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    //[HtmlTargetElement("link", Attributes = UseServerUrlName)]
    //public class ServerUrlsTagHelper : TagHelper
    //{
    //    public override int Order { get { return 99999; } }

    //    private const string UseServerUrlName = "boc-include-server-url";

    //    [ViewContext]
    //    public ViewContext ViewContext { get; set; }
    //    /// <summary>
    //    /// An expression to be evaluated against the current model.
    //    /// </summary>
    //    [HtmlAttributeName(UseServerUrlName)]
    //    public bool UseServerUrl { get; set; }

    //    static Regex hrefRegex = new Regex("href=\"([^\"]+)\"", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
    //    public override void Process(TagHelperContext context, TagHelperOutput output)
    //    {
    //        if (!UseServerUrl)
    //            return;
    //        if (output.Attributes.ContainsName("href"))
    //        {
    //            var attrib = output.Attributes["href"];
    //            var sval = attrib?.Value as string;
    //            var val = (sval == null) ? attrib?.Value as HtmlString : new HtmlString(sval);

    //            if (string.IsNullOrEmpty(val?.ToString()) || val.ToString().Contains("://"))
    //                return;
    //            sval = val.ToString();
    //            output.Attributes.Remove(attrib);
    //            output.Attributes.Add(new TagHelperAttribute(
    //                attrib.Name, new HtmlString(new Uri(new Uri(ViewContext.HttpContext.Request.GetEncodedUrl()), sval).ToString())));
    //        }
    //        if (output.PostElement != null && !output.PostElement.IsEmptyOrWhiteSpace)
    //        {
    //            var content = output.PostElement.GetContent();
    //            output.PostElement.SetHtmlContent(hrefRegex.Replace(content,
    //                match => "href=\"" + new Uri(new Uri(ViewContext.HttpContext.Request.GetEncodedUrl()), match.Groups[1].Value).ToString() + "\""));
    //        }
    //    }
    //}
}
