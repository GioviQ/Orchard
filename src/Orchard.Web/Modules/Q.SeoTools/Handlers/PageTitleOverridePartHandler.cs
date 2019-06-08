using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Q.SeoTools.Models;

namespace Q.SeoTools.Handlers
{
    public class PageTitleOverridePartHandler : ContentHandler
    {
        public PageTitleOverridePartHandler(IRepository<PageTitleOverridePartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}