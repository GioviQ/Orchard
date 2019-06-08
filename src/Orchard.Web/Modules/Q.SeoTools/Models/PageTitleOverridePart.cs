using Orchard.ContentManagement;

namespace Q.SeoTools.Models
{
    public class PageTitleOverridePart : ContentPart<PageTitleOverridePartRecord>
    {
        public string PageTitle
        {
            get { return Record.PageTitle; }
            set { Record.PageTitle = value; }
        }
    }
}
