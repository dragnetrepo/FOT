using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using Fot.Admin;
using Fot.Admin.Infrastructure;
using Mindscape.Raygun4Net;
using WebSupergoo.ABCpdf8;

namespace Fot.Admin
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

          //  ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.WebForms;

            XSettings.InstallLicense("WuJbSzVR9+1XpyYSM/zSObfMSeMZVIAmpMiwnrGyQh5YqX6WOi69N5FqZD0zmwcBbtU19T1PriQV1aoqSrR7yDkZWATfWHBOrCgxVcOyR0LmwU3y8sASXYXiZV2yYWFgGO5IUMBS/gJwaRx8rSiGKSk7XiebHAbAma0VuURyXObz7+c8vTXD3lPMiaEWpm9AcBFHoQ==");

            //HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();

            HangfireBootstrapper.Instance.Start();

        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown
            HangfireBootstrapper.Instance.Stop();

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

            var err = Server.GetLastError();


            new RaygunClient().Send(err);

            if (err is SecurityException)
            {

                Response.Redirect("Denied.aspx");

            }

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends.
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer
            // or SQLServer, the event is not raised.

        }
    }
}
