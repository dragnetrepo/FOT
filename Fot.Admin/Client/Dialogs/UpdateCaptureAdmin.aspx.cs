using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;

namespace Fot.Admin.Client.Dialogs
{
    public partial class UpdateCaptureAdmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = 0;


                if (Int32.TryParse(Request.QueryString["id"], out id))
                {
                    LoadForUpdate(id);
                }
                else
                {
                    form1.Visible = false;
                }


            }
        }

        private void LoadForUpdate(int id)
        {
            var item = new AdminUserService().GetAdminUser(id);

            if (item != null)
            {
                hidId.Value = id.ToString();
                txtEmail.Text = item.Username;
                txtFirstName.Text = item.Firstname;
                txtLastName.Text = item.Lastname;
                txtMobileNumber.Text = item.MobileNo;
                chkActive.Checked = item.Active;


                reqPassword.Visible = false;

            }
            else
            {
                form1.Visible = false;
            }

        }

        public void RegisterScript()
        {


            String csname1 = "PopupScript";
            Type cstype = this.GetType();

            // Get a ClientScriptManager reference from the Page class.

            ClientScriptManager cs = Page.ClientScript;

            // Check to see if the startup script is already registered.

            if (!cs.IsStartupScriptRegistered(cstype, csname1))
            {
                StringBuilder cstext1 = new StringBuilder();
                // cstext1.Append("alert('Hello World!');");
                var fnName = "refreshGrid";
                cstext1.Append("CallFunctionOnParentPage('" + fnName + "');");


                cs.RegisterStartupScript(cstype, csname1, cstext1.ToString(), true);
            }

        }

       
        protected void bttnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAdmin();
        }

        private void UpdateAdmin()
        {
            var service = new AdminUserService();

            var item = service.GetAdminUser(Int32.Parse(hidId.Value));

            if (item != null)
            {
                item.Username = txtEmail.Text;
                item.Password = string.IsNullOrWhiteSpace(txtPassword.Text) ? string.Empty : txtPassword.Text;
                item.Firstname = txtFirstName.Text;
                item.Lastname = txtLastName.Text;
                item.MobileNo = txtMobileNumber.Text;

                if (item.Active == false && chkActive.Checked)
                {
                    item.FailedLoginAttempts = 0;
                }


                item.Active = chkActive.Checked;


                var app = new AdminUserService().UpdateAdminUser(item);

                if (app.IsDone)
                {

                    RegisterScript();

                    var admin = new AdminUserService().GetCurrentAdmin();

                    new AccessLogService().LogEntry(new AccessLog { AdminId = admin.AdminId, LogEntryType = "Updated a photo capture admin", LogEntryDetails = "User updated a photo capture administrator [" + item.Username + "]", LogDate = DateTime.Now, IpAddress = Request.UserHostAddress, UserAgent = Request.UserAgent });

                }

                lblStatus.ShowMessage(app);

            }

        }
    }
}