using Orchard.Caching;
using Q.SeoTools.Services;
using System.Text;
using System.Web.Mvc;

namespace SH.Ads.Controllers
{
    public class AdsController : Controller
    {
        private const string ContentType = "text/plain";

        private readonly IAdsService _adsService;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;

        public AdsController(IAdsService adsService, ICacheManager cacheManager, ISignals signals)
        {
            _adsService = adsService;
            _cacheManager = cacheManager;
            _signals = signals;
        }

        public ContentResult Index()
        {
            var content = _cacheManager.Get("AdsFile.Settings",
                              ctx =>
                              {
                                  ctx.Monitor(_signals.When("AdsFile.SettingsChanged"));
                                  var AdsFile = _adsService.Get();
                                  return AdsFile.FileContent;
                              });
            if (string.IsNullOrWhiteSpace(content))
            {
                content = _adsService.Get().FileContent;
            }
            return new ContentResult()
            {
                ContentType = ContentType,
                ContentEncoding = Encoding.UTF8,
                Content = content
            };
        }
    }
}