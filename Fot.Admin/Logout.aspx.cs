using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;


namespace Fot.Admin
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var item = new AdminUserService().GetCurrentAdmin();

            new AccessLogService().LogEntry(new AccessLog { AdminId = item.AdminId, LogEntryType = "User Logged Out", LogEntryDetails = "User Logged Out", LogDate = DateTime.Now, IpAddress = Request.UserHostAddress, UserAgent = Request.UserAgent });

            FormsAuthentication.SignOut();
            Session.Clear();
           
            var app = new AppMessage(){Message = "Logged out succesfully.", Status = MessageStatus.Info};
            Session["ADMIN_STATUS"] = app;
            
            Response.Redirect(UrlMapper.Default);
        }
    }
}