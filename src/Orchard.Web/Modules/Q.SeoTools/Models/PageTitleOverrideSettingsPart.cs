using Orchard.ContentManagement;

namespace Q.SeoTools.Models
{
    public class PageTitleOverrideSettingsPart : ContentPart
    {
        public bool IsPageTitleSiteNameLast
        {
            get { return this.Retrieve(x => x.IsPageTitleSiteNameLast); }
            set { this.Store(x => x.IsPageTitleSiteNameLast, value); }
        }

        public bool IsPageTitleHideSiteName
        {
            get { return this.Retrieve(x => x.IsPageTitleHideSiteName); }
            set { this.Store(x => x.IsPageTitleHideSiteName, value); }
        }
    }
}
