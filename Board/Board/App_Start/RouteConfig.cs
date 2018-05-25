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
              name: "CreateAds",
              url: "Create",
              defaults: new { controller = "Ads", action = "AddAds" }
          );

            routes.MapRoute(
              name: "AllAds",
              url: "Category",
              defaults: new { controller = "Ads", action = "AllAds" }
          );

            routes.MapRoute(
              name: "SingleAds",
              url: "Category/{name}/{id}",
              defaults: new { controller = "Ads", action = "SelectAds", name = UrlParameter.Optional, id = UrlParameter.Optional }
          );

            routes.MapRoute(
             name: "Register",
             url: "Registration",
             defaults: new { controller = "Account", action = "Register" }
         );
            routes.MapRoute(
             name: "Auth",
             url: "Auth",
             defaults: new { controller = "Account", action = "Login" }
         );

            routes.MapRoute(
            name: "MyAds",
            url: "My",
            defaults: new { controller = "Ads", action = "MyAds" }
        );



            routes.MapRoute(
              name: "Search",
              url: "Search",
              defaults: new { controller = "Ads", action = "AdsSearch" }
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
