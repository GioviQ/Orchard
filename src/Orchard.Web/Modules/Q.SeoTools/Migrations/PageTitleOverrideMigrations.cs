using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace Q.SeoTools.Migrations
{
    public class PageTitleOverrideMigrations : DataMigrationImpl
    {

        public int Create()
        {
            SchemaBuilder.CreateTable(
                "PageTitleOverridePartRecord", table => table
                    .ContentPartRecord()
                    .Column<string>("PageTitle")
            );

            // Allow Content Part to be utilized on other Content Types
            ContentDefinitionManager.AlterPartDefinition("PageTitleOverridePart", builder => builder
                .Attachable()
            );

            // Add to Page Content Type
            ContentDefinitionManager.AlterTypeDefinition("Page", builder => builder
                .WithPart("PageTitleOverridePart")
            );

            // Add a Widget Content Type
            ContentDefinitionManager.AlterTypeDefinition("PageTitleOverrideWidget", builder => builder
                .WithPart("PageTitleOverridePart")
                .WithPart("CommonPart")
                .WithPart("WidgetPart")
                .WithSetting("Stereotype", "Widget")
            );
            return 1;
        }

    }
}