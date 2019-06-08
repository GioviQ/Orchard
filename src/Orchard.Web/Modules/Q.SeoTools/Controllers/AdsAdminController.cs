using Orchard;
using Orchard.Localization;
using Orchard.UI.Admin;
using Orchard.UI.Notify;
using Q.SeoTools.Permissions;
using Q.SeoTools.Services;
using Q.SeoTools.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace Q.SeoTools.Controllers
{
    [Admin]
    [ValidateInput(false)]
    public class AdsAdminController : Controller
    {
        private readonly IAdsService _adsService;

        public AdsAdminController(IAdsService adsService, IOrchardServices orchardServices)
        {
            _adsService = adsService;
            Services = orchardServices;
            T = NullLocalizer.Instance;
        }

        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; }

        public ActionResult Index()
        {
            if (!Authorized())
                return new HttpUnauthorizedResult();
            return View(new AdsFileViewModel() { Text = _adsService.Get().FileContent });
        }

        [HttpPost]
        public ActionResult Index(AdsFileViewModel viewModel)
        {
            if (!Authorized())
                return new HttpUnauthorizedResult();
            var saveResult = _adsService.Save(viewModel.Text);
            if (saveResult.Item1)
                Services.Notifier.Information(T("Ads.txt settings successfully saved"));
            else
            {
                Services.Notifier.Information(T("Ads.txt saved with warnings"));
                saveResult.Item2.ToList().ForEach(error =>
                    Services.Notifier.Warning(T(error))
                );
            }
            return View(viewModel);
        }

        private bool Authorized()
        {
            return Services.Authorizer.Authorize(AdsPermissions.ConfigureAdsTextFile, T("Cannot manage ads.txt file"));
        }
    }
}