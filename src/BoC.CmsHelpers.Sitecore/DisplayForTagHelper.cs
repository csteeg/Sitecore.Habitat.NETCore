using System;
using System.Linq.Expressions;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

namespace BoC.CmsHelpers.Abstraction
{
    [HtmlTargetElement(Attributes = "cms-display-*")]
    public class DisplayForTagHelper : TagHelper
    {
        private readonly IModelMetadataProvider _modelMetadataProvider;
        private readonly IViewEngine _viewEngine;
        private readonly IViewBufferScope _viewBufferScope;

        public DisplayForTagHelper(IModelMetadataProvider modelMetadataProvider, ICompositeViewEngine viewEngine,
            IViewBufferScope viewBufferScope)
        {
            _modelMetadataProvider = modelMetadataProvider;
            _viewEngine = viewEngine;
            _viewBufferScope = viewBufferScope;
        }

        [HtmlAttributeName("cms-display-for")]
        public Expression<Func<object, object>> For { get; set; }

        [HtmlAttributeName("cms-display-property")]
        public string ForPropertyName { get; set; }

        [HtmlAttributeName("cms-display-ashtml")]
        public bool AsHtml { get; set; }

        [HtmlAttributeName("cms-display-disable-editing")]
        public bool DisableEditing { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }
            if (output.TagName == "cms-display")
                output.TagName = null;

            ModelExplorer fieldEditor = null;
            ModelExplorer expression = null;
            var viewData = ViewContext.ViewData;
            DisableEditing = DisableEditing || !ViewContext.HttpContext.IsInCmsMode();
            if (For != null)
            {
                if (!DisableEditing)
                { 
                    var memExpression = For.Body as MemberExpression;
                    if (memExpression != null)
                    {
                        var target = Expression.Lambda(memExpression.Expression).Compile().DynamicInvoke();
                        if (target != null)
                        {
                            viewData = new ViewDataDictionary(_modelMetadataProvider, new ModelStateDictionary()) { Model = target };
                            fieldEditor = DisableEditing ? null : ExpressionMetadataProvider.FromStringExpression("FieldEditors", viewData, _modelMetadataProvider);
                            if (fieldEditor?.Model != null)
                            {
                                viewData = new ViewDataDictionary(_modelMetadataProvider, new ModelStateDictionary()) { Model = fieldEditor.Model };
                                expression = ExpressionMetadataProvider.FromStringExpression(memExpression.Member.Name, viewData, _modelMetadataProvider);
                            }
                            if (expression?.Model != null)
                            {
                                expression = ExpressionMetadataProvider.FromStringExpression(memExpression.Member.Name, viewData, _modelMetadataProvider);
                            }
                        }
                    }
                }
                if (expression?.Model != null)
                {
                    AsHtml = true;
                }
                else
                {
                    expression = ExpressionMetadataProvider.FromLambdaExpression<object, object>(For,
                        new ViewDataDictionary<object>(ViewContext.ViewData), _modelMetadataProvider);
                    viewData = ViewContext.ViewData;
                }
            }
            else
            {
                fieldEditor = DisableEditing ? null : ExpressionMetadataProvider.FromStringExpression("FieldEditors", viewData, _modelMetadataProvider);
                if (fieldEditor != null && fieldEditor.Model != null)
                {
                    viewData = new ViewDataDictionary(_modelMetadataProvider, new ModelStateDictionary()){Model = fieldEditor.Model};
                    expression = ExpressionMetadataProvider.FromStringExpression(ForPropertyName, viewData, _modelMetadataProvider);
                }
                if (expression?.Model != null)
                {
                    AsHtml = true;
                }
                else
                {
                    expression = ExpressionMetadataProvider.FromStringExpression(ForPropertyName, ViewContext.ViewData, _modelMetadataProvider);
                    viewData = ViewContext.ViewData;
                }
            }
            IHtmlContent content = null;
            if (expression != null && expression.Model != null)
            {
                content = new TemplateBuilder(
                    _viewEngine,
                    _viewBufferScope,
                    this.ViewContext,
                    viewData,
                    expression,
                    null,//ForPropertyName ?? ExpressionHelper.GetExpressionText((LambdaExpression)For),
                    null, true, null).Build();
                if (content is ViewBuffer) //displaytemplate
                    AsHtml = true;
            }
            if (content == null && output.TagName != null)
            {
                var childContent = await output.GetChildContentAsync();
                if (string.IsNullOrEmpty(childContent?.GetContent()))
                {
                    output.TagName = null;
                    output.SuppressOutput();
                }

                return;
            }
            output.TagMode = TagMode.StartTagAndEndTag;
            if (content == null)
                return;
            if (AsHtml)
            {
                output.PreContent.SetHtmlContent(content);
            }
            else
            {
                using (var writer = new System.IO.StringWriter())
                {
                    content.WriteTo(writer, HtmlEncoder.Default);
                    output.PreContent.SetContent(writer.ToString());
                }
            }
        }
    }
}
