using Orchard.Mvc.Routes;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace Q.SeoTools.Routes
{

    public class CustomAliasRoutes : IRouteProvider
    {
        private readonly ICustomAliasConstraint _customAliasSlugConstraint;
        public CustomAliasRoutes(ICustomAliasConstraint customAliasSlugConstraint)
        {
            _customAliasSlugConstraint = customAliasSlugConstraint;
        }

        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {
                             new RouteDescriptor {
                                                    Priority = 85 /*higher than alias*/,
                                                    Route = new Route(
                                                         "{*customAlias}",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Q.SeoTools"},
                                                                                      {"controller", "CustomAlias"},
                                                                                      {"action", "Go"},
                                                                                      {"customAlias", ""}
                                                                                  },
                                                         new RouteValueDictionary {
                                                                                      {"customAlias", _customAliasSlugConstraint}
                                                                                  },
                                                         new RouteValueDictionary {
                                                                                      {"area", "Q.SeoTools"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 }
                         };
        }
    }
}