using System.Threading.Tasks;
using Habitat.Core.Repositories.Multisite;
using Microsoft.AspNetCore.Mvc;

namespace Habitat.Core.Components.Multisite
{
    [ViewComponent(Name = "SwitchSite: Sitecore.Feature.Multisite.Controllers.MultisiteController, Sitecore.Feature.Multisite")]
    public class SwitchSiteViewComponent: ViewComponent
    {
        private readonly IMultisiteRepository _multisiteRepository;

        public SwitchSiteViewComponent(IMultisiteRepository multisiteRepository)
        {
            _multisiteRepository = multisiteRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var definitions = await _multisiteRepository.GetAll();
            
            return View("SwitchSite", definitions);
        }
    }
}
