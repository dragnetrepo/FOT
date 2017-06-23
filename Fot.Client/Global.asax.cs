using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Fot.Client.App_Start;
using Fot.Lan;
using Mindscape.Raygun4Net;
using StructureMap.Web.Pipeline;
using WebSupergoo.ABCpdf8;

namespace Fot.Client
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterOpenAuth();
            RouteConfig.RegisterRoutes(RouteTable.Routes);


            XSettings.InstallLicense("WuJbSzVR9+1XpyYSM/zSObfMSeMZVIAmpMiwnrGyQh5YqX6WOi69N5FqZD0zmwcBbtU19T1PriQV1aoqSrR7yDkZWATfWHBOrCgxVcOyR0LmwU3y8sASXYXiZV2yYWFgGO5IUMBS/gJwaRx8rSiGKSk7XiebHAbAma0VuURyXObz7+c8vTXD3lPMiaEWpm9AcBFHoQ==");

        }

        protected void Application_EndRequest()
        {
            HttpContextLifecycle.DisposeAndClearAll();



        }


        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

            var exception = Server.GetLastError();
            new RaygunClient().Send(exception);

        }
    }
}
