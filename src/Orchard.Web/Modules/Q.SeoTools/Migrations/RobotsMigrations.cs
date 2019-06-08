using Orchard.Data.Migration;

namespace Q.SeoTools.Migrations
{
    public class RobotsMigrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable("RobotsFileRecord",
                table => table
                    .Column<int>("Id", col => col.PrimaryKey().Identity())
                    .Column<string>("FileContent", col => col.Nullable().Unlimited().WithDefault(@"User-agent: *
Allow: /"))
                );
            return 1;
        }
    }
}