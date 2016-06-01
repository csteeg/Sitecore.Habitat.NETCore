using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BoC.CmsHelpers.Abstraction
{
    public interface IPlaceholderRenderer
    {
        Task DoRender(string placeholderName, bool isDynamic, IHtmlContentBuilder output, ViewContext viewContext);
    }
}