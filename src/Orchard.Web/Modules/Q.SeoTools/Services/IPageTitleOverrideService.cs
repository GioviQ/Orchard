using Orchard;

namespace Q.SeoTools.Services
{
    public interface IPageTitleOverrideService : IDependency {
        bool GetIsPageTitleSiteNameLast();
        bool GetIsPageTitleHideSiteName();
        string GetPageTitleOverride();
    }
}