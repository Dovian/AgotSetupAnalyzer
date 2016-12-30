using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AgotSetupAnalyzerWS
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "InitialDataPull",
                routeTemplate: "api/{controller}/InitialDataPull",
                constraints: new { controller = "Update" },
                defaults: new { }
            );

            config.Routes.MapHttpRoute(
                name: "UpdateRangePull",
                routeTemplate: "api/{controller}/UpdateDataPull/{SetCode}",
                constraints: new { controller = "Update" },
                defaults: new { SetCode = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "TestLogMessage",
                routeTemplate: "api/{controller}/LogMessage",
                constraints: new { controller = "Update" },
                defaults: new { }
            );
        }
    }
}
