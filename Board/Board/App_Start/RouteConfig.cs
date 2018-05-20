using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Board
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
               name: "Ads",
               url: "ads",
               defaults: new { controller = "Ads", action = "SelectAds" }
           );

            routes.MapRoute(
               name: "Contact",
               url: "Contact",
               defaults: new { controller = "Home", action = "Contact" }
           );

            routes.MapRoute(
                name: "AddBanner",
                url: "Admin/AddBanner",
                defaults: new { controller = "Banner", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
