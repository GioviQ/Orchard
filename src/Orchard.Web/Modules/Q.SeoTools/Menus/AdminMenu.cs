using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Navigation;
using Q.SeoTools.Permissions;

namespace Q.SeoTools.Menus
{
    public class AdminMenu : INavigationProvider
    {
        public Localizer T { get; set; }

        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T("SEO"), "3", BuildMenu);
        }

        private void BuildMenu(NavigationItemBuilder menu)
        {
            menu.LinkToFirstChild(false);

            menu.Add(T("Redirects"), "1", i => i.Action("Index", "CustomAliasAdmin", new { area = "Q.SeoTools" }).Permission(StandardPermissions.SiteOwner));
            menu.Add(T("Robots.txt"), "2", i => i.Action("Index", "RobotsAdmin", new { area = "Q.SeoTools" }).Permission(RobotsPermissions.ConfigureRobotsTextFile));
            menu.Add(T("Ads.txt"), "2", i => i.Action("Index", "AdsAdmin", new { area = "Q.SeoTools" }).Permission(AdsPermissions.ConfigureAdsTextFile));
        }
    }
}