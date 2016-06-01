using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Links;
using Sitecore.Pipelines;
using Sitecore.Pipelines.RenderField;
using Sitecore.Services.Core.Model;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;

namespace BoC.Sitecore.ApiExtensions
{
    public class ItemModelActionFilter : ActionFilterAttribute
    {
        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            var objectContent = actionExecutedContext.Response.Content as ObjectContent;
            if (objectContent?.Value == null)
                return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
            var currentLanguage = Context.Site.Language;
            if (typeof(ItemModel).IsAssignableFrom(objectContent.ObjectType))
            {
                EnrichItemModel(objectContent.Value as ItemModel, currentLanguage, global::Sitecore.Context.PageMode.IsExperienceEditor);
            }
            else if ((typeof(IEnumerable<>).IsAssignableFrom(objectContent.ObjectType) && typeof(ItemModel).IsAssignableFrom(objectContent.ObjectType.GetGenericArguments()[0]))
                     || (objectContent.ObjectType.IsArray && typeof(ItemModel).IsAssignableFrom(objectContent.ObjectType.GetElementType())))
            {
                foreach (ItemModel model in (IEnumerable) objectContent.Value)
                {
                    EnrichItemModel(model, currentLanguage, global::Sitecore.Context.PageMode.IsExperienceEditor);
                }
            }
            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }

        public static void EnrichItemModel(ItemModel model, string defaultLanguage, bool includeEditMode = false)
        {
            if (model == null)
                return;
            if (model.ContainsKey("TemplateID") && !model.ContainsKey("TemplateIDs"))
            {
                var templateId = new ID((Guid) model["TemplateID"]);
                var template = TemplateManager.GetTemplate(templateId, global::Sitecore.Context.Database);
                if (template != null)
                    model.Add("TemplateIDs", template.GetBaseTemplates().Select(t => t.ID.Guid).Concat(new[] {templateId.Guid}));
            }

            var scItem = global::Sitecore.Context.Database.GetItem(new ID((Guid)model["ItemID"]));//TODO: should we get it again? Better to fix modelfactory instead
            if (scItem == null)
                return;
            if (!model.ContainsKey("ParentIds"))
            {
                model.Add("ParentIds", scItem?.Paths?.LongID?.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries).Select(id => new Guid(id)));
            }
            if (!model.ContainsKey("ItemUrlExpanded"))
            {
                var urlOptions = LinkManager.GetDefaultUrlOptions();
                urlOptions.AlwaysIncludeServerUrl = false;
                urlOptions.SiteResolving = true;
                urlOptions.LanguageEmbedding = scItem.Language.Name == defaultLanguage ? LanguageEmbedding.Never : LanguageEmbedding.Always;
                urlOptions.LowercaseUrls = true;
                model.Add("ItemUrlExpanded", LinkManager.GetItemUrl(scItem, urlOptions));
            }

            if (includeEditMode)
            {
                var editor = new Dictionary<string,string>();
                foreach (var field in model.Keys)
                {
                    var editfield = EditField(field, scItem);
                    if (!string.IsNullOrEmpty(editfield))
                        editor.Add(field, editfield);
                }
                model.Add("FieldEditors", editor);
            }
        }

        public static string EditField(string fieldName, Item item)
        {
            using (new ContextItemSwitcher(item))
            {
                RenderFieldArgs renderFieldArgs = new RenderFieldArgs();
                renderFieldArgs.Item = item;
                renderFieldArgs.FieldName = fieldName;
                renderFieldArgs.DisableWebEdit = false;

                CorePipeline.Run("renderField", (PipelineArgs)renderFieldArgs);

                return renderFieldArgs.Result.ToString();

            }
        }

    }
}