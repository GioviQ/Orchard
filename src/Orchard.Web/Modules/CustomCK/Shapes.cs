using Orchard.DisplayManagement.Descriptors;
using Orchard;
using Orchard.ContentManagement;
using Orchard.DisplayManagement.Implementation;

using CustomCK.Models;

namespace CustomCK
{
    public class Shapes : IShapeTableProvider
	{
		public IOrchardServices Services { get; set; }
		public Shapes(IOrchardServices orchardServices)
		{
			Services = orchardServices;
        }
		public void Discover(ShapeTableBuilder builder)
		{
			builder.Describe("CustomCK_Config").OnDisplaying(CustomCK_Config_Displaying);
		}
        public void CustomCK_Config_Displaying(ShapeDisplayingContext displaying)
		{
            var settings = Services.WorkContext.CurrentSite.As<CustomCKSettingsPart>();
            displaying.Shape.Settings = settings;
		}
	}
}