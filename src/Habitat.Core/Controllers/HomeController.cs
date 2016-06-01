using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Habitat.Core.Repositories.Faq;
using Microsoft.AspNetCore.Mvc;
using BoC.CmsHelpers.Abstraction;

namespace Habitat.Core.Controllers
{
    public class HomeController : Controller
    {
        private static readonly Uri _baseAddress = new Uri("http://habitat.localhost.chrisvandesteeg.nl/");

        public IActionResult Index()
        {
            if (HttpContext.IsInCmsMode())
            {
                //TODO: move to better place?
                HttpContext.Items[typeof(IUrlHelper)] = new FullUrlHelper(this.ControllerContext);
            }
            return View();
        }

        [Route("media/{*mediaUrl}")]
        public async Task<IActionResult> Media(string mediaUrl = null, Guid mediaId = default(Guid))
        {
            var url = mediaUrl;
            if (mediaId != default(Guid))
            {
                url = $"-/media/{mediaId:N}.ashx";
            }
            using (var httpClient = new HttpClient {BaseAddress = _baseAddress})
            {
                using (var response = await httpClient.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var stream = await response.Content.ReadAsByteArrayAsync();
                            return File(stream, response.Content.Headers.ContentType.MediaType);
                    }

                    return NotFound();
                }
            }
        }
    }
}
