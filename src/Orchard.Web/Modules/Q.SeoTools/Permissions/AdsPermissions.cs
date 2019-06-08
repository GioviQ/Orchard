using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;
using System.Collections.Generic;

namespace Q.SeoTools.Permissions
{
    public class AdsPermissions : IPermissionProvider
    {
        public static readonly Permission ConfigureAdsTextFile = new Permission { Description = "Configure Ads.txt", Name = "ConfigureAdsTextFile" };

        public virtual Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions()
        {
            return new[] { ConfigureAdsTextFile };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[] { new PermissionStereotype { Name = "Administrator", Permissions = new[] { ConfigureAdsTextFile } } };
        }
    }
}