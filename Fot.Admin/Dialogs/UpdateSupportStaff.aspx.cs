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
    public partial class UpdateSupportStaff : System.Web.UI.Page
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
            var item = new SupportStaffService().GetStaff(id);

            if (item != null)
            {
                hidId.Value = id.ToString();
                txtEmail.Text = item.Email;
                txtFirstName.Text = item.Firstname;
                txtLastName.Text = item.Lastname;
                txtMobileNumber.Text = item.MobileNo;
                chkActive.Checked = item.Active;

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
            var service = new SupportStaffService();

            var item = service.GetStaff(Int32.Parse(hidId.Value));

            if (item != null)
            {
                item.Email = txtEmail.Text;
                item.Firstname = txtFirstName.Text;
                item.Lastname = txtLastName.Text;
                item.MobileNo = txtMobileNumber.Text;


                item.Active = chkActive.Checked;



                var app = new SupportStaffService().UpdateStaff(item);

                if (app.IsDone)
                {

                    RegisterScript();

                    var admin = new AdminUserService().GetCurrentAdmin();

                    new AccessLogService().LogEntry(new AccessLog { AdminId = admin.AdminId, LogEntryType = "Updated a center support staff", LogEntryDetails = "User updated a center support staff [" + item.Email + "]", LogDate = DateTime.Now, IpAddress = Request.UserHostAddress, UserAgent = Request.UserAgent });

                }

                lblStatus.ShowMessage(app);

            }

        }
    }
}