using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Logistic
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DownloadExcel",
                url: "{controller}/{action}/{id}/{ExcelId}",
                defaults: new { controller = "ExcelDownload", action = "Download", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Item",
                url: "{controller}/{action}/{id}/{ExcelId}",
                defaults: new { controller = "Item", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
