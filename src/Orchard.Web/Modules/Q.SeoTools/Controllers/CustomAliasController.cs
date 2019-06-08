using Orchard;
using Orchard.Localization;
using Q.SeoTools.Services;
using System.Web.Mvc;

namespace Q.SeoTools.Controllers
{

    public class CustomAliasController : Controller
    {
        public IOrchardServices Services { get; set; }
        private readonly ICustomAliasService _customAliasService;
        public CustomAliasController(IOrchardServices services, ICustomAliasService customAliasService)
        {
            Services = services;
            T = NullLocalizer.Instance;
            _customAliasService = customAliasService;
        }

        public Localizer T { get; set; }

        public ActionResult Go(string customAlias)
        {
            var link = _customAliasService.GetByAlias(customAlias);
            if (link != null && link.Enabled)
                return new RedirectResult(link.OriginalUrl, link.Permanent);
            return HttpNotFound();
        }
    }
}
