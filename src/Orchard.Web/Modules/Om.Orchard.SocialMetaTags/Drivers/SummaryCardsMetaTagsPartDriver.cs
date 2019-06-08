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
    public class SummaryCardsMetaTagsPartDriver : ContentPartDriver<SummaryCardsMetaTagsPart>
    {
        private readonly IWorkContextAccessor _wca;

        public Localizer T { get; set; }

        public SummaryCardsMetaTagsPartDriver(IWorkContextAccessor wca)
        {
            _wca = wca;
            T = NullLocalizer.Instance;
        }

        protected override string Prefix
        {
            get
            {
                return "summarycardmetatags";
            }
        }

        protected override DriverResult Display(SummaryCardsMetaTagsPart part, string displayType, dynamic shapeHelper)
        {
            if (displayType != "Detail") return null;

            var resourceManager = _wca.GetContext().Resolve<IResourceManager>();
            var summaryCardsTagsSettings = _wca.GetContext().CurrentSite.As<SummaryCardsMetaTagsSettingsPart>();

            string title = part.CardTitle;

            if (String.IsNullOrWhiteSpace(part.CardTitle))
            {
                title = part.ContentItem.As<TitlePart>().Title;

                var pageTitleOverridePart = part.ContentItem.As<PageTitleOverridePart>();

                if (pageTitleOverridePart != null && !String.IsNullOrWhiteSpace(pageTitleOverridePart.PageTitle))
                    title = pageTitleOverridePart.PageTitle;
            }

            resourceManager.SetMeta(SocialMetaTagsHelpers.BuildMetaTag("twitter:title", title));

            var openGraphMetaTagsPart = part.ContentItem.Parts.FirstOrDefault(c => c.GetType() == typeof(OpenGraphMetaTagsPart));

            var ogImageField = openGraphMetaTagsPart == null ? null : openGraphMetaTagsPart.Fields.FirstOrDefault(f => f.Name == "OgImage") as MediaLibraryPickerField;

            if (ogImageField == null || String.IsNullOrWhiteSpace(ogImageField.FirstMediaUrl))
            {
                var p = part.ContentItem.Parts.FirstOrDefault(c => c.GetType() == typeof(ContentPart) && c.Fields.Any(f => f.GetType() == typeof(MediaLibraryPickerField)));

                if (p != null)
                    ogImageField = p.Fields.FirstOrDefault(f => f is MediaLibraryPickerField) as MediaLibraryPickerField;
            }

            string cardImage = part.CardImage;

            if (ogImageField != null && !String.IsNullOrWhiteSpace(ogImageField.FirstMediaUrl))
                cardImage = _wca.GetContext().CurrentSite.BaseUrl + ogImageField.FirstMediaUrl;

            if (!String.IsNullOrWhiteSpace(cardImage))
                resourceManager.SetMeta(SocialMetaTagsHelpers.BuildMetaTag("twitter:image", cardImage));

            resourceManager.SetMeta(SocialMetaTagsHelpers.BuildMetaTag("twitter:card", part.CardType != "select" ? part.CardType : "summary_large_image"));

            var metaTagsPart = part.ContentItem.Parts.FirstOrDefault(c => c.GetType() == typeof(MetaPart)) as MetaPart;

            if ((metaTagsPart != null && !String.IsNullOrWhiteSpace(metaTagsPart.Description)) || !String.IsNullOrWhiteSpace(part.CardDescription))
                resourceManager.SetMeta(SocialMetaTagsHelpers.BuildMetaTag("twitter:description", !String.IsNullOrWhiteSpace(part.CardDescription) ? part.CardDescription : metaTagsPart.Description));

            if (summaryCardsTagsSettings.RenderOutput)
            {
                if (summaryCardsTagsSettings.CardSiteTagEnabled && !String.IsNullOrWhiteSpace(part.CardSite))
                    resourceManager.SetMeta(SocialMetaTagsHelpers.BuildMetaTag("twitter:site", part.CardSite));

                if (summaryCardsTagsSettings.CardCreatorTagEnabled && !String.IsNullOrWhiteSpace(part.CardCreator))
                    resourceManager.SetMeta(SocialMetaTagsHelpers.BuildMetaTag("twitter:creator", part.CardCreator));
            }
            return null;
        }

        protected override DriverResult Editor(SummaryCardsMetaTagsPart part, dynamic shapeHelper)
        {
            part.SummaryCardsTagsSettings = _wca.GetContext().CurrentSite.As<SummaryCardsMetaTagsSettingsPart>();
            return ContentShape("Parts_SummaryCardsMetaTags_Edit",
                    () => shapeHelper.EditorTemplate(
                        TemplateName: "Parts/SummaryCardsMetaTags",
                        Model: part,
                        Prefix: Prefix));
        }

        protected override DriverResult Editor(SummaryCardsMetaTagsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            if (updater.TryUpdateModel(part, Prefix, null, null))
            {

                //Validation as per selections
                if (part.SummaryCardsTagsSettings.CardTypeTagEnabled && part.SummaryCardsTagsSettings.CardTypeTagRequired
                    && part.CardType == "select")
                    updater.AddModelError("_FORM", T("Twitter Card Type is required"));

                if (part.SummaryCardsTagsSettings.CardTitleTagEnabled && part.SummaryCardsTagsSettings.CardTitleTagRequired
                    && String.IsNullOrWhiteSpace(part.CardTitle))
                    updater.AddModelError("_FORM", T("Twitter Card Title field is required"));

                if (part.SummaryCardsTagsSettings.CardDescriptionTagEnabled && part.SummaryCardsTagsSettings.CardDescriptionTagRequired
                    && String.IsNullOrWhiteSpace(part.CardDescription))
                    updater.AddModelError("_FORM", T("Twitter Card Description field is required"));

                if (part.SummaryCardsTagsSettings.CardImageTagEnabled && part.SummaryCardsTagsSettings.CardImageTagRequired
                    && String.IsNullOrWhiteSpace(part.CardImage))
                    updater.AddModelError("_FORM", T("Twitter Card Image Url field is required"));

                if (part.SummaryCardsTagsSettings.CardCreatorTagEnabled && part.SummaryCardsTagsSettings.CardCreatorTagRequired
                    && String.IsNullOrWhiteSpace(part.CardCreator))
                    updater.AddModelError("_FORM", T("Twitter Card Creator field is required"));

                if (part.SummaryCardsTagsSettings.CardSiteTagEnabled && part.SummaryCardsTagsSettings.CardSiteTagRequired
                    && String.IsNullOrWhiteSpace(part.CardSite))
                    updater.AddModelError("_FORM", T("Twitter Card Site field is required"));

            }
            return Editor(part, shapeHelper);
        }
    }
}