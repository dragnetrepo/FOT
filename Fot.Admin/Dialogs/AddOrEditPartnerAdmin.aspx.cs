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

namespace Fot.Admin.Dialogs
{
    public partial class AddOrEditPartnerAdmin : System.Web.UI.Page
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
                listPartners.SelectedValue = item.PartnerId.ToString();


                listPartners.Enabled = false;

                bttnAdd.Visible = false;
                bttnUpdate.Visible = true;

                this.Title = "Update Partner Admin";

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

        protected void bttnAdd_Click(object sender, EventArgs e)
        {
            AddAdmin();
        }

        private void AddAdmin()
        {
            var item = new AdminUser
            {
                Username = txtEmail.Text,
                Password = txtPassword.Text,
                Firstname = txtFirstName.Text,
                Lastname = txtLastName.Text,
                MobileNo = txtMobileNumber.Text,
                Active = chkActive.Checked,
                IsPartnerAdmin = true,
                HasUsersAccess = true,
                HasCenterUsersAccess = true,
                CanAuthor = true,
                CanSchedule = true,
                HasResultsAccess = true,

                PartnerId = Int32.Parse(listPartners.SelectedValue)

            };

            var app = new AdminUserService().AddPartnerAdminUser(item);

            if (app.IsDone)
            {
                txtEmail.Text = string.Empty;
                txtFirstName.Text = string.Empty;
                txtPassword.Text = string.Empty;
                txtLastName.Text = string.Empty;
                txtMobileNumber.Text = string.Empty;

                RegisterScript();

                var admin = new AdminUserService().GetCurrentAdmin();

                new AccessLogService().LogEntry(new AccessLog { AdminId = admin.AdminId, LogEntryType = "Added a partner admin", LogEntryDetails = "User added a new partner administrator [" + item.Username + "]", LogDate = DateTime.Now, IpAddress = Request.UserHostAddress, UserAgent = Request.UserAgent });

            }

            lblStatus.ShowMessage(app);
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
                item.PartnerId = Int32.Parse(listPartners.SelectedValue);

                var app = new AdminUserService().UpdateAdminUser(item);

                if (app.IsDone)
                {

                    RegisterScript();

                    var admin = new AdminUserService().GetCurrentAdmin();

                    new AccessLogService().LogEntry(new AccessLog { AdminId = admin.AdminId, LogEntryType = "Updated a partner admin", LogEntryDetails = "User updated a partner administrator [" + item.Username + "]", LogDate = DateTime.Now, IpAddress = Request.UserHostAddress, UserAgent = Request.UserAgent });

                }

                lblStatus.ShowMessage(app);

            }

        }
    }
}