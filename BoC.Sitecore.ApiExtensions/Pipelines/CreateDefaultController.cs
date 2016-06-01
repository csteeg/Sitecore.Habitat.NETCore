using System.Web.Mvc;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Pipelines.Request.CreateController;

namespace BoC.Sitecore.ApiExtensions
{
    public class CreateDefaultController : CreateControllerProcessor
    {
        public override void Process(CreateControllerArgs args)
        {
            Assert.ArgumentNotNull((object)args, "args");
            if (args.Result != null)
                return;
            args.Result = this.CreateController();
        }

        protected virtual IController CreateController()
        {
            return (IController)new JsonSitecoreController();
        }
    }
}