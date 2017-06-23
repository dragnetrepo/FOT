using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Client.Models;

namespace Fot.Client
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

            Session["STATUS"] = new AppMessage
            {
                IsDone = false,
                Message = "You have logged out successfully.",
                Status = MessageStatus.Info
            };
            Response.Redirect("Default.aspx");
        }
    }
}