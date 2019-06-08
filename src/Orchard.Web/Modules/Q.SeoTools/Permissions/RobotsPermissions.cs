using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;
using System.Collections.Generic;

namespace Q.SeoTools.Permissions
{
    public class RobotsPermissions : IPermissionProvider
    {
        public static readonly Permission ConfigureRobotsTextFile = new Permission { Description = "Configure Robots.txt", Name = "ConfigureRobotsTextFile" };

        public virtual Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions()
        {
            return new[] { ConfigureRobotsTextFile };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[] { new PermissionStereotype { Name = "Administrator", Permissions = new[] { ConfigureRobotsTextFile } } };
        }
    }
}