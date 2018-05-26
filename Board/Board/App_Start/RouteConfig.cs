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
              url: "c/{translitCat}/{subcattrans}",
              defaults: new { controller = "Ads", action = "AllAds", translitCat = UrlParameter.Optional, subcattrans = UrlParameter.Optional}
          );

            routes.MapRoute(
            name: "SpecSingleAds",
            url: "c/{catTrans}/{adstrans}/{id}",
            defaults: new
            {
                controller = "Ads",
                action = "SelectAds",
                id = UrlParameter.Optional,
                catTrans = UrlParameter.Optional,
                adstrans = UrlParameter.Optional
            }
        );

            routes.MapRoute(
            name: "SingleAds",
            url: "c/{catTrans}/{subcattrans}/{adstrans}/{id}",
            defaults: new { controller = "Ads", action = "SelectAds", id = UrlParameter.Optional, catTrans = UrlParameter.Optional,
            subcattrans = UrlParameter.Optional, adstrans = UrlParameter.Optional}
        );
            

            routes.MapRoute(
             name: "User",
             url: "User",
             defaults: new { controller = "Manage", action = "Index" }
         );

            routes.MapRoute(
             name: "Password",
             url: "Passwrod",
             defaults: new { controller = "Manage", action = "ChangePassword" }
         );

            routes.MapRoute(
             name: "Phone",
             url: "Phone",
             defaults: new { controller = "Manage", action = "AddPhoneNumber" }
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
