using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DAWProject
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}/{sortOrder}/{searching}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, sortOrder = UrlParameter.Optional, searching = UrlParameter.Optional },
                namespaces: new[] {"DAWProject.Controllers"}
            );


        }
    }
}
