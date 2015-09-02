using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sporty.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    "ExerciseCalendar", // Route name
            //    "Exercise/Calendar/{month}/{year}", // URL with parameters
            //    new { controller = "Exercise", action = "Calendar", month = "", year = "" } // Parameter defaults
            //    );

            routes.MapRoute(
                "PlanCalendar", // Route name
                "Plan/Calendar/{month}/{year}", // URL with parameters
                new { controller = "Plan", action = "Calendar", month = "", year = "" } // Parameter defaults
                );

            routes.MapRoute(
                "ReportMonthly", // Route name
                "Report/Month/{month}/{year}/{displayOnlyData}", // URL with parameters
                new { controller = "Report", action = "Month", month = "", year = "", displayOnlyData = false }
                // Parameter defaults
                );

            routes.MapRoute(
                "Report", // Route name
                "Report/Report/{from}/{to}/{timeUnit}/{reportType}",
                // URL with parameters
                new
                {
                    controller = "Report",
                    action = "Report",
                    from = "",
                    to = "",
                    timeUnit = "",
                    reportType = ""
                } // Parameter defaults
                );
            routes.MapRoute(
                "RefreshAttachment", // Route name
                "Exercise/RefreshAttachment/{exerciseId}/{attachmenId}", // URL with parameters
                new { controller = "Exercise", action = "RefreshAttachment", exerciseId = "", attachmenId = "" }
                // Parameter defaults
                );
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = "" } // Parameter defaults
                );

        }
    }
}