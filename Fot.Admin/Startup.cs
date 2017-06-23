using System;
using System.Threading.Tasks;
using System.Web;
using Fot.Admin.Infrastructure;
using Hangfire;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Fot.Admin.Startup))]

namespace Fot.Admin
{
    public class Startup
    {
        public string SummaryJob = "SummaryJob";
        public void Configuration(IAppBuilder app)
        {

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() },
                AppPath = VirtualPathUtility.ToAbsolute("~/Main.aspx")
            });


            RecurringJob.AddOrUpdate(SummaryJob,  () => Processor.ProcessSummary(), Cron.Daily);

        }
    }
}
