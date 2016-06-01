using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BoC.CmsHelpers.Abstraction
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    [HtmlTargetElement("cms-placeholder", Attributes = PlaceholderNameAttributeName)]
    public class CmsPlaceholder : TagHelper
    {
        private readonly IPlaceholderRenderer _renderer;

        public CmsPlaceholder(IPlaceholderRenderer renderer)
        {
            _renderer = renderer;
        }

        private const string PlaceholderNameAttributeName = "name";
        private const string IsNestedNameAttributeName = "isdynamic";

        /// <summary>
        /// An expression to be evaluated against the current model.
        /// </summary>
        [HtmlAttributeName(PlaceholderNameAttributeName)]
        public string PlaceholderName { get; set; }

        [HtmlAttributeName(IsNestedNameAttributeName)]
        public bool IsDynamic { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            await _renderer.DoRender(PlaceholderName, IsDynamic, output.Content, ViewContext);
        }
    }
}
