using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fot.Admin.Models;
using Hangfire.Dashboard;
using Microsoft.Owin;


namespace Fot.Admin.Infrastructure
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {

        public bool Authorize(DashboardContext context)
        {
            var ctx = new OwinContext(context.GetOwinEnvironment());

            return  ctx.Authentication.User.Identity.IsAuthenticated && ctx.Authentication.User.IsInRole(RoleModel.Admin);

             

        }
    }
}