using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Fot.DTO;
using Fot.Lan.Models;
using Fot.Lan.Services;
using Telerik.Web.UI;

namespace Fot.Lan.Admin
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Utilities.CheckSerial();


            if (!Page.IsPostBack)
            {
                ProcessLogout();
            }
        }

        private void ProcessLogout()
        {
            if (Session["ADMIN_STATUS"] != null)
            {
                var app = (AppMessage)Session["ADMIN_STATUS"];

                lblStatus.ShowMessage(app);

                Session["ADMIN_STATUS"] = null;
            }
        }

        protected void bttnLogin_Click(object sender, EventArgs e)
        {
            DoLogin();
        }

        public void DoLogin()
        {

            var ctx = new ServiceBase().Context;

            var admin = ctx.AdminUsers.FirstOrDefault(x => x.Username == txtUsername.Text && x.IsSupportStaff == false && x.IsCaptureAdmin == false);

            if (admin != null && admin.Password.Equals(FotSecurity<string>.Hash(txtPassword.Text)))
            {





                Session["LOGIN"] = true;
                Session["USERID"] = admin.ActualUserId;

              
                 Response.Redirect("Status.aspx");



            }
            else
            {
                var app = new AppMessage() { Status = MessageStatus.Error, Message = "Invalid login details" };


                lblStatus.ShowMessage(app);
            }
        }




    }
}