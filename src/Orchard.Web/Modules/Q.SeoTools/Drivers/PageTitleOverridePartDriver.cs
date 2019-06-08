using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Q.SeoTools.Models;
using System.Web;

namespace Q.SeoTools.Drivers
{
    public class PageTitleOverridePartDriver : ContentPartDriver<PageTitleOverridePart>
    {
        private readonly IWorkContextAccessor _wca;

        public PageTitleOverridePartDriver(IWorkContextAccessor wca)
        {
            _wca = wca;
        }

        protected override string Prefix { get { return "PageTitleOverride"; } }

        protected override DriverResult Display(PageTitleOverridePart part, string displayType, dynamic shapeHelper)
        {
            if (displayType == "Detail")
                HttpContext.Current.Cache.Insert("Q.SeoTools.PageTitleOverride.PageTitle", part.PageTitle ?? "");
            return null;
        }

        protected override DriverResult Editor(PageTitleOverridePart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_PageTitleOverride_PageTitleOverride",
                               () => shapeHelper.EditorTemplate(
                                   TemplateName: "Parts/PageTitleOverride.PageTitleOverride",
                                   Model: part,
                                   Prefix: Prefix)
                                );
        }

        protected override DriverResult Editor(PageTitleOverridePart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part.Record, Prefix, null, null);
            return Editor(part, shapeHelper);
        }

        protected override void Exporting(PageTitleOverridePart part, ExportContentContext context)
        {
            context.Element(part.PartDefinition.Name).SetAttributeValue("PageTitle", part.PageTitle);
        }

        protected override void Importing(PageTitleOverridePart part, ImportContentContext context)
        {
            part.PageTitle = context.Attribute(part.PartDefinition.Name, "PageTitle");
        }
    }
}