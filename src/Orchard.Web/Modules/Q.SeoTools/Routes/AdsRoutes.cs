using Orchard.Mvc.Routes;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace Q.SeoTools.Routes
{

    public class AdssRoutes : IRouteProvider
    {
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {
                                new RouteDescriptor {   Priority = 5,
                                                        Route = new Route(
                                                            "ads.txt",
                                                            new RouteValueDictionary {
                                                                                        {"area", "Q.SeoTools"},
                                                                                        {"controller", "Ads"},
                                                                                        {"action", "Index"}
                                                            },
                                                            new RouteValueDictionary(),
                                                            new RouteValueDictionary {
                                                                                        {"area", "Q.SeoTools"}
                                                            },
                                                            new MvcRouteHandler())
                                },
                            };
        }
    }
}