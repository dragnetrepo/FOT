using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Fot.Admin.Infrastructure;
using Microsoft.AspNet.FriendlyUrls;



namespace Fot.Admin
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            
            routes.EnableFriendlyUrls(new MyFriendlyUrlResolver());

          
        }
    }
}
