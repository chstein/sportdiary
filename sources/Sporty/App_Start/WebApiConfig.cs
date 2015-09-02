using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace Sporty.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.Routes.MapHttpRoute(
            //    "ExerciseCalendarApi", // Route name
            //    routeTemplate: "Exercise/UpdateSessionDate", // URL with parameters/exercise/api/calendar/UpdateSessionDate/
            //    defaults: new { controller = "Calendar", action = "UpdateSessionDate" } // , month = "", year = "" } // Parameter defaults
            //    );

            config.Routes.MapHttpRoute(
                name: "DefaultExerciseApi",
                routeTemplate: "Exercise/api/calendar/{id}",
                defaults: new { controller ="ExerciseCalendar", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultPlanApi",
                routeTemplate: "Plan/api/calendar/{id}",
                defaults: new { controller = "PlanCalendar", id = RouteParameter.Optional }
            );

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            //config.EnableSystemDiagnosticsTracing();

            // Use camel case for JSON data.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
