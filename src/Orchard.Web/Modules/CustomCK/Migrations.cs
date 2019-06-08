using System.Data;
using Orchard.Data.Migration;

namespace CustomCK
{
	public class Migrations : DataMigrationImpl {
		public int Create() {
			// Creating table CustomCKSettingsPartRecord
			SchemaBuilder.CreateTable("CustomCKSettingsPartRecord", table => table
				.ContentPartRecord()
				.Column("ConfigString", DbType.String, column => column.Unlimited())
			);
			return 1;
		}
	}
}