using Orchard.Data.Migration;

namespace Q.SeoTools.Migrations
{
    public class AdsMigrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable("AdsFileRecord",
                table => table
                    .Column<int>("Id", col => col.PrimaryKey().Identity())
                    .Column<string>("FileContent", col => col.Nullable().Unlimited().WithDefault(@"google.com, pub-0000000000000000, DIRECT, f08c47fec0942fa0"))
                );
            return 1;
        }
    }
}