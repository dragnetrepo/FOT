using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Client.Models;

namespace Fot.Client
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckLogin();
        }

        private void CheckLogin()
        {
            if (Session["LOGIN"] == null || Convert.ToBoolean(Session["LOGIN"]) == false)
            {
                Session["STATUS"] = new AppMessage
                    {
                        IsDone = false,
                        Message = "Your login session has expired. Please re-login.",
                        Status = MessageStatus.Error
                    };
                Response.Redirect("Default.aspx");
            }
        }
    }
}