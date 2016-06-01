using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoC.CmsHelpers.Abstraction;
using BoC.CmsHelpers.Sitecore.Models;
using BoC.CmsHelpers.Sitecore.Services;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace BoC.CmsHelpers.Sitecore
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class PlaceholderRenderer : IPlaceholderRenderer
    {
        private const string BocUniqueId = "__boc_uniqueId";
        private const string BocPlaceholderStack = "__boc_placeholderStack";
        private readonly ICompositeViewEngine _viewEngine;
        private readonly IViewComponentHelper _viewComponentHelper;
        private readonly ISitecoreService _sitecoreService;
        private readonly Uri _baseAddress;

        public PlaceholderRenderer(ICompositeViewEngine viewEngine, IViewComponentHelper viewComponentHelper, ISitecoreService sitecoreService, IOptions<ApiSettings> apiSettings)
        {
            _viewEngine = viewEngine;
            _viewComponentHelper = viewComponentHelper;
            _sitecoreService = sitecoreService;
            _baseAddress = new Uri(new Uri(apiSettings.Value.BaseUrl), apiSettings.Value.PageDataApiPath);
        }

        public async Task DoRender(string placeholderName, bool isDynamic, IHtmlContentBuilder output, ViewContext viewContext)
        {
            if (viewContext?.ViewData == null)
            {
                output.Append("ViewContext/ViewData cannot be null");
                return;
            }
            if (isDynamic && viewContext.ViewData[BocUniqueId] == null)
            {
                output.Append("Nested placeholders must be inside a known component");
                return;
            }
            
            var placeholderStack = viewContext.ViewData[BocPlaceholderStack] as Stack<string>;
            if (placeholderStack == null)
            {
                placeholderStack = (Stack<string>) (viewContext.ViewData[BocPlaceholderStack] = new Stack<string>());
            }
            var usedName = isDynamic ? placeholderName + "_" + (Guid)viewContext.ViewData[BocUniqueId] : placeholderName;
            placeholderStack.Push(usedName);

            var placeHolderKey = placeholderStack.Count > 1 ? "/" + string.Join("/", placeholderStack.ToArray().Reverse()) : usedName;
            var pathandQuery = viewContext.HttpContext.Request.Path.Value + viewContext.HttpContext.Request.QueryString;
            var pageData = await GetPageData(pathandQuery);
            if (pageData == null || string.IsNullOrEmpty(placeHolderKey))
            {
                output.AppendHtml("UNABLE TO LOAD PLACEHOLDER " + placeHolderKey);
                return;
            }

            RenderingChrome? placeholderChrome = null;
            if (viewContext.HttpContext.IsInCmsMode())
            {
                placeholderChrome = await _sitecoreService.Get<RenderingChrome>(pathandQuery + (pathandQuery.Contains("?") ? "&" : "?") + "placeholderKey=" + placeHolderKey, _baseAddress);
                if (placeholderChrome.HasValue)
                {
                    output.AppendHtmlLine(placeholderChrome.Value.Start);
                }
            }

            foreach (var rendering in pageData.Renderings.Where(r => 
                usedName.Equals(r.Placeholder, StringComparison.OrdinalIgnoreCase) ||
                placeHolderKey.Equals(r.Placeholder, StringComparison.OrdinalIgnoreCase)))
            {
                if (viewContext.HttpContext.IsInCmsMode() && rendering.RenderingChrome != null)
                    output.AppendHtmlLine(rendering.RenderingChrome.Value.Start);
                JToken path = null;
                if (rendering.RenderingItem.TryGetValue("Path", out path))
                {
                    await RenderPartialView(path.Value<string>(), rendering, output, viewContext, pageData);
                }
                else
                {
                    JToken controller = null;
                    JToken controllerAction = null;
                    if (rendering.RenderingItem.TryGetValue("Controller", out controller) &&
                        rendering.RenderingItem.TryGetValue("Controller Action", out controllerAction))
                    {
                        await RenderControllerAction(controller.Value<string>(), controllerAction.Value<string>(), rendering, output, viewContext, pageData);
                    }
                }
                if (viewContext.HttpContext.IsInCmsMode() && rendering.RenderingChrome != null)
                    output.AppendHtmlLine(rendering.RenderingChrome.Value.End);
            }

            if (placeholderChrome.HasValue)
            {
                output.AppendHtmlLine(placeholderChrome.Value.End);
            }
            placeholderStack.Pop();
        }

        private async Task RenderControllerAction(string controller, string action, Rendering rendering, IHtmlContentBuilder output, ViewContext viewContext, PageData pageData)
        {
            var context = await GetViewContext(rendering, viewContext, pageData, new NullView(), viewContext.Writer);
            var toContext = _viewComponentHelper as IViewContextAware;
            if (toContext != null)
                toContext.Contextualize(context);
            try
            {
                var result = await _viewComponentHelper.InvokeAsync(action + ": " + controller);
                output.AppendHtml(result);
            }
            catch (InvalidOperationException exc)
            {
                output.AppendHtml($"<div class=\"alert alert-danger\"><strong>{exc.Message}</strong> not found!</div>");
            }
        }

        private async Task RenderPartialView(string viewName, Rendering rendering, IHtmlContentBuilder output, ViewContext viewContext, PageData pageData)
        {
            var actionContext = new ActionContext();
            var partialView = this._viewEngine.GetView(null, viewName, false);
            if (!partialView.Success)
            {
                output.AppendHtml(
                    $"<div class=\"alert alert-danger\"><strong>{viewName}</strong> not found!<br />Searched locations:{string.Join(", ",partialView.SearchedLocations)}</div>");
                return;
            }
            var view = partialView.View;
            using (view as IDisposable)
            {
                var sb = new StringBuilder();
                using (var tw = new StringWriter(sb))
                {
                    var context = await GetViewContext(rendering, viewContext, pageData, view, tw);
                    await partialView.View.RenderAsync(context);
                }
                output.AppendHtml(sb.ToString());
            }
        }

        private async Task<ViewContext> GetViewContext(Rendering rendering, ViewContext viewContext, PageData pageData, IView view, TextWriter tw)
        {
            var viewData = new ViewDataDictionary(viewContext.ViewData);
            viewData[BocUniqueId] = rendering.UniqueId;
            viewData["RenderingParameters"] = pageData.RenderingParameters;
            viewData["PageData"] = pageData;
            viewData["Page"] = pageData.Page;
            if (!string.IsNullOrEmpty(rendering.DataSource))
            {
                Guid id;
                if (Guid.TryParse(rendering.DataSource, out id))
                {
                    viewData["Model"] = viewData.Model = await _sitecoreService.Get<JObject>(id);
                }
                else
                {
                    viewData["Model"] = viewData.Model = rendering.DataSource;
                }
                
            }
            else
            {
                viewData["Model"] = viewData.Model = pageData.Page;
            }
            var context = new ViewContext(viewContext, view, viewData, tw);
            return context;
        }

        private PageData _pageData;//placeholderrenderer is (MUST) be instanced per scope/request, so hold the data

        async Task<PageData> GetPageData(string path)
        {
            if (_pageData != null)
                return _pageData;
            return _pageData = await _sitecoreService.Get<PageData>(path, _baseAddress);
        }
    }
}
