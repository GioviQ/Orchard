using Orchard.ContentManagement;

namespace CustomCK.Models
{
	public class CustomCKSettingsPart : ContentPart<CustomCKSettingsPartRecord>
	{
		public string ConfigString
		{
			get { return Record.ConfigString; }
			set { Record.ConfigString = value; }
		}
	}
}



