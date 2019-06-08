using Orchard.ContentManagement.Records;

namespace Q.SeoTools.Models
{
    public class PageTitleOverridePartRecord : ContentPartRecord
    {
        public virtual string PageTitle { get; set; }
    }
}