using LALoDep.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LALoDep
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);


            routes.MapRoute(
              name: "Default2",
              url: "g/{guid}/{controller}/{action}/{id}",
              defaults: new { controller = "Case", action = "Search", id = UrlParameter.Optional },

                namespaces: new string[] { "LALoDep.Controllers" }
          );



            routes.Add("Default", new GuidRoute(
"{controller}/{action}/{id}",
new { controller = "Case", action = "Search", guid = "", id = UrlParameter.Optional },
   namespaces: new string[] { "LALoDep.Controllers" }));

            routes.Add("GuidRoute", new GuidRoute(
                "g/{guid}/{controller}/{action}/{id}",
                new { controller = "Case", action = "Search", guid = "", id = UrlParameter.Optional },
              namespaces: new string[] { "LALoDep.Controllers" }));
           
        }
    }
}
