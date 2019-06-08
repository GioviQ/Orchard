using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Localization;
using Q.SeoTools.Models;

namespace Q.SeoTools.Handlers
{
    public class PageTitleOverrideSettingsPartHandler : ContentHandler
    {
        public PageTitleOverrideSettingsPartHandler()
        {
            T = NullLocalizer.Instance;

            Filters.Add(new ActivatingFilter<PageTitleOverrideSettingsPart>("Site"));
            Filters.Add(new TemplateFilterForPart<PageTitleOverrideSettingsPart>("PageTitleOverrideSettings", "Parts/PageTitleOverride.PageTitleOverrideSettings", "Page Title Override"));
        }

        public Localizer T { get; set; }

        protected override void GetItemMetadata(GetContentItemMetadataContext context)
        {
            if (context.ContentItem.ContentType != "Site")
                return;
            base.GetItemMetadata(context);
            // Add in the menu option for the Settings
            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("Page Title Override")));
        }
    }
}