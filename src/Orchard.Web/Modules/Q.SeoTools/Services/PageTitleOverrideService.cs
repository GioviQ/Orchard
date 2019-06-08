using Orchard;
using Orchard.Caching;
using Orchard.ContentManagement;
using Q.SeoTools.Models;
using System.Web;

namespace Q.SeoTools.Services
{

    public class PageTitleOverrideService : IPageTitleOverrideService
    {
        private readonly IOrchardServices _orchardServices;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;

        public PageTitleOverrideService(IOrchardServices orchardServices, ICacheManager cacheManager, ISignals signals)
        {
            _orchardServices = orchardServices;
            _cacheManager = cacheManager;
            _signals = signals;
        }

        public PageTitleOverrideService(WorkContext workContext)
        {
            _orchardServices = workContext.Resolve<IOrchardServices>();
            _cacheManager = new DefaultCacheManager(this.GetType(), new DefaultCacheHolder(new DefaultCacheContextAccessor()));
            _signals = workContext.Resolve<ISignals>();
        }

        public bool GetIsPageTitleSiteNameLast()
        {
            return bool.Parse(
                _cacheManager.Get(
                    "Q.SeoTools.PageTitleOverride.IsPageTitleSiteNameLast",
                    ctx =>
                    {
                        ctx.Monitor(_signals.When("Q.SeoTools.PageTitleOverride.Changed"));
                        var pageTitleOverrideSettings = _orchardServices.WorkContext.CurrentSite.As<PageTitleOverrideSettingsPart>();
                        return pageTitleOverrideSettings.IsPageTitleSiteNameLast.ToString();
                    })
                );
        }

        public bool GetIsPageTitleHideSiteName()
        {
            return bool.Parse(
                _cacheManager.Get(
                    "Q.SeoTools.PageTitleOverride.IsPageTitleHideSiteName",
                    ctx =>
                    {
                        ctx.Monitor(_signals.When("Q.SeoTools.PageTitleOverride.Changed"));
                        var pageTitleOverrideSettings = _orchardServices.WorkContext.CurrentSite.As<PageTitleOverrideSettingsPart>();
                        return pageTitleOverrideSettings.IsPageTitleHideSiteName.ToString();
                    })
                );
        }

        public string GetPageTitleOverride()
        {
            string pageTitleOverride = "";
            try
            {
                if (HttpContext.Current.Cache["Q.SeoTools.PageTitleOverride.PageTitle"] != null)
                {
                    pageTitleOverride = HttpContext.Current.Cache["Q.SeoTools.PageTitleOverride.PageTitle"].ToString();
                    HttpContext.Current.Cache["Q.SeoTools.PageTitleOverride.PageTitle"] = "";
                }
            }
            catch { }
            return pageTitleOverride;
        }

    }
}