using Om.Orchard.SocialMetaTags.Helpers;
using Om.Orchard.SocialMetaTags.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Core.Title.Models;
using Orchard.Localization;
using Orchard.MediaLibrary.Fields;
using Orchard.UI.Resources;
using Q.SeoTools.Models;
using System;
using System.Linq;
using Vandelay.Industries.Models;

namespace Om.Orchard.SocialMetaTags.Drivers
{
    public class OpenGraphMetaTagsPartDriver : ContentPartDriver<OpenGraphMetaTagsPart>
    {
        private readonly IWorkContextAccessor _wca;

        public Localizer T { get; set; }

        public OpenGraphMetaTagsPartDriver(IWorkContextAccessor wca)
        {
            _wca = wca;
            T = NullLocalizer.Instance;
        }

        protected override string Prefix
        {
            get
            {
                return "ogmetatags";
            }
        }

        protected override DriverResult Display(OpenGraphMetaTagsPart part, string displayType, dynamic shapeHelper)
        {
            if (displayType != "Detail") return null;

            var resourceManager = _wca.GetContext().Resolve<IResourceManager>();
            var OpenGraphTagsSettings = _wca.GetContext().CurrentSite.As<OpenGraphMetaTagsSettingsPart>();

            string title = part.OgTitle;

            if (String.IsNullOrWhiteSpace(part.OgTitle))
            {
                title = part.ContentItem.As<TitlePart>().Title;

                var pageTitleOverridePart = part.ContentItem.As<PageTitleOverridePart>();

                if (pageTitleOverridePart != null && !String.IsNullOrWhiteSpace(pageTitleOverridePart.PageTitle))
                    title = pageTitleOverridePart.PageTitle;
            }

            resourceManager.SetMeta(SocialMetaTagsHelpers.BuildPropertyMetaTag("ogtitlekey", "og:title", title));
            resourceManager.SetMeta(SocialMetaTagsHelpers.BuildPropertyMetaTag("ogsitenamekey", "og:site_name", String.IsNullOrWhiteSpace(part.OgSiteName) ? _wca.GetContext().CurrentSite.SiteName : part.OgSiteName));

            var ogImageField = part.Fields.FirstOrDefault(f => f.Name == "OgImage") as MediaLibraryPickerField;

            if (ogImageField == null || String.IsNullOrWhiteSpace(ogImageField.FirstMediaUrl))
            {
                var p = part.ContentItem.Parts.FirstOrDefault(c => c.GetType() == typeof(ContentPart) && c.Fields.Any(f => f.GetType() == typeof(MediaLibraryPickerField)));

                if (p != null)
                    ogImageField = p.Fields.FirstOrDefault(f => f is MediaLibraryPickerField) as MediaLibraryPickerField;
            }

            string ogImage = part.OgImage;

            if (ogImageField != null && !String.IsNullOrWhiteSpace(ogImageField.FirstMediaUrl))
                ogImage = _wca.GetContext().CurrentSite.BaseUrl + ogImageField.FirstMediaUrl;

            if (!String.IsNullOrWhiteSpace(ogImage))
                resourceManager.SetMeta(SocialMetaTagsHelpers.BuildPropertyMetaTag("ogimagekey", "og:image", ogImage));

            var metaTagsPart = part.ContentItem.Parts.FirstOrDefault(c => c.GetType() == typeof(MetaPart)) as MetaPart;

            if ((metaTagsPart != null && !String.IsNullOrWhiteSpace(metaTagsPart.Description)) || !String.IsNullOrWhiteSpace(part.OgDescription))
                resourceManager.SetMeta(SocialMetaTagsHelpers.BuildPropertyMetaTag("ogdesckey", "og:description", !String.IsNullOrWhiteSpace(part.OgDescription) ? part.OgDescription : metaTagsPart.Description));


            if (OpenGraphTagsSettings.RenderOutput)
            {
                if (OpenGraphTagsSettings.OgTypeTagEnabled && part.OgType != "select")
                    resourceManager.SetMeta(SocialMetaTagsHelpers.BuildPropertyMetaTag("ogtypekey", "og:type", part.OgType));

                if (OpenGraphTagsSettings.OgUrlTagEnabled && !String.IsNullOrWhiteSpace(part.OgUrl))
                    resourceManager.SetMeta(SocialMetaTagsHelpers.BuildPropertyMetaTag("ogurlkey", "og:url", part.OgUrl));

                if (OpenGraphTagsSettings.OgLocaleTagEnabled && !String.IsNullOrWhiteSpace(part.OgLocale))
                    resourceManager.SetMeta(SocialMetaTagsHelpers.BuildPropertyMetaTag("oglocalekey", "og:locale", part.OgLocale));

                if (OpenGraphTagsSettings.FbAdminTagEnabled && !String.IsNullOrWhiteSpace(part.FBAdmins))
                    resourceManager.SetMeta(SocialMetaTagsHelpers.BuildPropertyMetaTag("fbadminskey", "fb:admins", part.FBAdmins));
            }
            return null;
        }

        protected override DriverResult Editor(OpenGraphMetaTagsPart part, dynamic shapeHelper)
        {
            part.OpenGraphTagsSettings = _wca.GetContext().CurrentSite.As<OpenGraphMetaTagsSettingsPart>();
            return ContentShape("Parts_OpenGraphMetaTags_Edit",
                    () => shapeHelper.EditorTemplate(
                        TemplateName: "Parts/OpenGraphMetaTags",
                        Model: part,
                        Prefix: Prefix));
        }

        protected override DriverResult Editor(OpenGraphMetaTagsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            if (updater.TryUpdateModel(part, Prefix, null, null))
            {
                part.OpenGraphTagsSettings = _wca.GetContext().CurrentSite.As<OpenGraphMetaTagsSettingsPart>();

                //Validation as per selections
                if (part.OpenGraphTagsSettings.OgTitleTagEnabled && part.OpenGraphTagsSettings.OgTitleTagRequired
                    && String.IsNullOrWhiteSpace(part.OgTitle))
                    updater.AddModelError("_FORM", T("Open Graph Title field is required"));

                if (part.OpenGraphTagsSettings.OgTypeTagEnabled && part.OpenGraphTagsSettings.OgTypeTagRequired
                    && part.OgType == "select")
                    updater.AddModelError("_FORM", T("Open Grpah Type field is required"));

                if (part.OpenGraphTagsSettings.OgImageTagEnabled && part.OpenGraphTagsSettings.OgImageTagRequired
                    && String.IsNullOrWhiteSpace(part.OgImage))
                    updater.AddModelError("_FORM", T("Open Graph Image field is required"));

                if (part.OpenGraphTagsSettings.OgUrlTagEnabled && part.OpenGraphTagsSettings.OgUrlTagRequired
                    && String.IsNullOrWhiteSpace(part.OgUrl))
                    updater.AddModelError("_FORM", T("Open Graph Url field is required"));

                if (part.OpenGraphTagsSettings.OgDescriptionTagEnabled && part.OpenGraphTagsSettings.OgDescriptionTagRequired
                    && String.IsNullOrWhiteSpace(part.OgDescription))
                    updater.AddModelError("_FORM", T("Open Graph Description field is required"));

                if (part.OpenGraphTagsSettings.OgLocaleTagEnabled && part.OpenGraphTagsSettings.OgLocaleTagRequired
                    && String.IsNullOrWhiteSpace(part.OgLocale))
                    updater.AddModelError("_FORM", T("Open Graph Locale field is required"));

                if (part.OpenGraphTagsSettings.OgSiteNameTagEnabled && part.OpenGraphTagsSettings.OgSiteNameTagRequired
                    && String.IsNullOrWhiteSpace(part.OgSiteName))
                    updater.AddModelError("_FORM", T("Open Graph Site Name field is required"));

                if (part.OpenGraphTagsSettings.FbAdminTagEnabled && part.OpenGraphTagsSettings.FbAdminTagRequired
                    && String.IsNullOrWhiteSpace(part.FBAdmins))
                    updater.AddModelError("_FORM", T("FB Admins field is required"));

            }
            return Editor(part, shapeHelper);
        }
    }
}