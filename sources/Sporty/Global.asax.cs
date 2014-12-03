using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Sporty.Common;
using Sporty.Helper;
using Sporty.App_Start;
using System.Web.Optimization;
using System.Web.Http;

namespace Sporty
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            ModelBinders.Binders.Add(typeof(TimeSpan?), new TimeSpanModelBinder());
            ModelBinders.Binders.Add(typeof(TimeSpan), new TimeSpanModelBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeModelBinder());
            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeModelBinder());
            
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        //protected void Session_Start(object sender, EventArgs e)
        //{
        //    //// Code that runs when a new session is started
        //    ////is need for upload flash plugin !
        //    //string sessionId = Session.SessionID;

        //    Thread.CurrentThread.CurrentCulture = CultureHelper.DefaultCulture;
        //    Thread.CurrentThread.CurrentUICulture = CultureHelper.DefaultCulture;
        //}
    }
}