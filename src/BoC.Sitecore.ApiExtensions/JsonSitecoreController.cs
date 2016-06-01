using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http.Cors;
using System.Web.Mvc;
using Sitecore.Data.Items;
using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Pipelines.Response.RenderPlaceholder;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;
using Sitecore.Mvc.Presentation;
using Sitecore.Services.Core;
using Sitecore.Services.Core.Model;
using Sitecore.Services.Infrastructure.Sitecore.Data;
using Sitecore.Sites;

namespace BoC.Sitecore.ApiExtensions
{
    public class PageData: EntityIdentity
    {
        public ItemModel Page { get; set; }
        public string FullPath { get; set; }
        public IEnumerable<Rendering> Renderings { get; set; }

    }

    public class Rendering
    {
        public ItemModel RenderingItem { get; set; }
        public Dictionary<string,string> RenderingParameters { get; set; }
        public string Placeholder { get; set; }
        public string DataSource { get; set; }
        public Guid UniqueId { get; set; }
        public RenderingChrome? RenderingChrome { get; set; }
    }

    public struct RenderingChrome
    {
        public string Start { get; set; }
        public string End { get; set; }
    }

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class JsonSitecoreController : SitecoreController
    {
        private IModelFactory _modelFactory;

        public JsonSitecoreController() : base()
        {
            _modelFactory = new ModelFactory();
        }

        protected override ActionResult GetDefaultAction()
        {
            if (Request.AcceptTypes != null && Request.AcceptTypes.Contains("application/json")
#if DEBUG
                || "true".Equals(Request["showAsJson"], StringComparison.InvariantCultureIgnoreCase)
#endif
                )
            {
                return Json(Request["placeholderKey"] != null ? (object)GetPlaceHolderChrome(Request["placeholderKey"]) :  GetPageData(), JsonRequestBehavior.AllowGet);
            }

            return base.GetDefaultAction();
        }

        private RenderingChrome GetPlaceHolderChrome(string placeholderKey)
        {
            var result = new RenderingChrome();
            var wrapper = new global::Sitecore.Mvc.ExperienceEditor.Pipelines.Response.RenderPlaceholder.AddWrapper();
            using (var sw = new StringWriter())
            {
                var sb = sw.GetStringBuilder();
                var args = new RenderPlaceholderArgs(placeholderKey, sw) {};
                using (PlaceholderContext.Enter(new PlaceholderContext(args.PlaceholderName)))
                {
                    wrapper.Process(args);
                    result.Start = sb.ToString();
                    sb.Clear();
                    foreach (var disposable in args.Disposables)
                    {
                        disposable.Dispose();
                    }
                }
                result.End = sb.ToString();
            }
            return result;
        }

        public PageData GetPageData()
        {
            var site = global::Sitecore.Context.Site;
            if (site == null)
                return null;

            var pageItem = global::Sitecore.Context.Item;
            if (pageItem == null)
                return null;
            //TODO: Get renderingscripts as habitat does?
            //var pageRendering = PipelineService.Get().RunPipeline<GetPageRenderingArgs, object>(
            //    "mvc.getPageRendering", 
            //    new GetPageRenderingArgs(pageContext.PageDefinition),
            //    a => a.Result) as Rendering;

            return new PageData()
            {
                Page = CreateItemModel(pageItem, site.Language, false, false, true, site.DisplayMode == DisplayMode.Edit),
                FullPath = pageItem.Paths.FullPath,
                Renderings =
                    PageContext.Current.PageDefinition.Renderings.Where(r => r != null).Select(r => new Rendering()
                    {
                        UniqueId = r.UniqueId,
                        DataSource = r.DataSource,
                        Placeholder = r.Placeholder,
                        RenderingParameters = r.Parameters.ToDictionary(p => p.Key, p => p.Value),
                        RenderingItem = CreateItemModel(r.RenderingItem?.InnerItem, site.Language, false, false, false),
                        RenderingChrome = site.DisplayMode == DisplayMode.Edit ? GetRenderingChrome(r) : null
                    })
            };
        }

        private RenderingChrome? GetRenderingChrome(global::Sitecore.Mvc.Presentation.Rendering rendering)
        {
            var result = new RenderingChrome();
            using (var sw = new StringWriter())
            using (RenderingContext.EnterContext(rendering))
            using (PlaceholderContext.Enter(new PlaceholderContext("dummy"))) //todo: can a rendering chrome differ per placeholder?
            {
                var sb = sw.GetStringBuilder();
                var wrapper = new global::Sitecore.Mvc.ExperienceEditor.Pipelines.Response.RenderRendering.AddWrapper();
                var args = new RenderRenderingArgs(rendering, sw);
                wrapper.Process(args);
                result.Start = sb.ToString();
                sb.Clear();
                foreach (var disposable in args.Disposables)
                    disposable.Dispose();
                result.End = sb.ToString();
            }

            return result;
        }

        private ItemModel CreateItemModel(Item innerItem, string defaultLanguage, bool includeStandardTemplateFields, bool includeMetaData, bool enrich = true, bool includeEditMode = false)
        {
            if (innerItem == null)
                return null;
            var model = _modelFactory.Create(innerItem, new GetRequestOptions()
            {
                Fields = new string[0],
                IncludeMetadata = includeMetaData,
                IncludeStandardTemplateFields = includeStandardTemplateFields
            });
            if (enrich)
                ItemModelActionFilter.EnrichItemModel(model, defaultLanguage, includeEditMode);
            return model;
        }
    }
}
