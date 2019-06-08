using Orchard.Data.Migration;

namespace Q.SeoTools.Migrations
{
    public class CustomAliasMigrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable("CustomAliasRecord",
                table => table
                    .Column<int>("Id", c => c.PrimaryKey().Identity())
                    .Column<string>("Alias", c => c.WithLength(2048))
                    .Column<string>("OriginalUrl", column => column.Unlimited())
                    .Column<bool>("Permanent")
                    .Column<bool>("Enabled")
                );

            return 2;
        }
        public int UpdateFrom1()
        {
            SchemaBuilder.AlterTable("CustomAliasRecord",
                table => table
                    .AddColumn<bool>("Permanent")
                );
            return 2;
        }
    }
}