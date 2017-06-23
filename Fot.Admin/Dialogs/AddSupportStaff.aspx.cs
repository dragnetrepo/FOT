using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;
using Fot.DTO;

namespace Fot.Admin.Dialogs
{
    public partial class AddSupportStaff : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = 0;


                if (Int32.TryParse(Request.QueryString["id"], out id))
                {
                    LoadForAdd(id);
                }
                else
                {
                    form1.Visible = false;
                }


            }
        }

        private void LoadForAdd(int id)
        {
            var item = new CenterService().GetCenter(id);

            if (item != null)
            {
                hidId.Value = id.ToString();

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
            var item = new CenterUser
            {
                Email = txtEmail.Text,
                Firstname = txtFirstName.Text,
                Lastname = txtLastName.Text,
                MobileNo = txtMobileNumber.Text,
                Active = chkActive.Checked,
                CenterId = Int32.Parse(hidId.Value),
                DateAdded = DateTime.Now
            };

            var app = new SupportStaffService().AddStaff(item);

            if (app.IsDone)
            {
                txtEmail.Text = string.Empty;
                txtFirstName.Text = string.Empty;
                txtLastName.Text = string.Empty;
                txtMobileNumber.Text = string.Empty;

                RegisterScript();

                var admin = new AdminUserService().GetCurrentAdmin();

                new AccessLogService().LogEntry(new AccessLog { AdminId = admin.AdminId, LogEntryType = "Added a center support staff", LogEntryDetails = "User added a new center support staff [" + item.Email + "]", LogDate = DateTime.Now, IpAddress = Request.UserHostAddress, UserAgent = Request.UserAgent });

            }

            lblStatus.ShowMessage(app);
        }



  
    }
}