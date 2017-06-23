using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Lan.Models;


namespace Fot.Lan.Admin
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
                Session["ADMIN_STATUS"] = new AppMessage
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