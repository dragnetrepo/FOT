using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Lan.Models;


namespace Fot.Lan.Admin
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ProcessLogout();
        }

        private void ProcessLogout()
        {
            Session["LOGIN"] = null;
            Session["USERID"] = null;

            Session["ADMIN_STATUS"] = new AppMessage
            {
                IsDone = false,
                Message = "You have logged out successfully.",
                Status = MessageStatus.Info
            };
            Response.Redirect("Default.aspx");
        }
    }
}