using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;


namespace Fot.Admin
{
    public partial class Denied : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            var app = new AppMessage() { Message = "Access denied to requested resource!", Status = MessageStatus.Error };
            Session["ADMIN_STATUS"] = app;

            Response.Redirect("Default.aspx");
        }
    }
}