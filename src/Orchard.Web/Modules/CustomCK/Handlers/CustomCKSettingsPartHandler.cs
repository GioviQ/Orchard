using CustomCK.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Localization;
using Orchard.ContentManagement;

namespace CustomCK.Handlers
{
    public class CustomCKSettingsPartHandler : ContentHandler
    {
        public CustomCKSettingsPartHandler(IRepository<CustomCKSettingsPartRecord> repository)
        {
            T = NullLocalizer.Instance;

            Filters.Add(new ActivatingFilter<CustomCKSettingsPart>("Site")); //Add a new Part to the Site content type

            Filters.Add(StorageFilter.For(repository)); //Ensures that Orchard takes care of saving the data

            //Loads Views/EditorTemplates/Parts.CustomCK.CustomCKSettings.cshtml
            Filters.Add(new TemplateFilterForRecord<CustomCKSettingsPartRecord>("CustomCKSettings", "Parts.CustomCK.CustomCKSettings", "CustomCK"));
        }

        public Localizer T { get; set; }

        protected override void GetItemMetadata(GetContentItemMetadataContext context)
        {
            if (context.ContentItem.ContentType != "Site")
                return;
            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("CustomCK"))); //Adds Settings > CustomCK Menu item
        }
    }
}