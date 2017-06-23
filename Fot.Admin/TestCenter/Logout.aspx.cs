using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;


namespace Fot.Admin.TestCenter
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Session.Clear();
           
            var app = new AppMessage(){Message = "Logged out succesfully.", Status = MessageStatus.Info};
            Session["ADMIN_STATUS"] = app;
            
            Response.Redirect("~/Default.aspx");
        }
    }
}