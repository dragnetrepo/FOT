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
    public partial class AddCampaign : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                txtStartDate.SelectedDate = DateTime.Today;
                txtEndDate.SelectedDate = DateTime.Today.AddDays(7);

                txtStartDate.MinDate = DateTime.Today;
                txtEndDate.MinDate = DateTime.Today;

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
            if (ValidateFields())
            {
                 Add();
            }
           
        }

        private bool ValidateFields()
        {
            if (listCampaignType.SelectedValue.Equals("1"))
            {
                if (!txtStartDate.SelectedDate.HasValue || !txtEndDate.SelectedDate.HasValue)
                {
                    lblStatus.ShowMessage(new AppMessage
                        {
                            IsDone = false,
                            Message = "'Start date' and 'End date' are required.",
                            Status = MessageStatus.Error
                        });

                    return false;
                }

                if (txtStartDate.SelectedDate > txtEndDate.SelectedDate)
                {
                    lblStatus.ShowMessage(new AppMessage
                        {
                            IsDone = false,
                            Message = "'End date' should be greater than or equal to 'Start date'.",
                            Status = MessageStatus.Error
                        });

                    return false;
                }
            }

            return true;
        }

        private void Add()
        {
            
            var item = new Campaign
            {
               CampaignName = txtCampaignName.Text,
               PartnerId = Int32.Parse(listPartners.SelectedValue),
               BundleId = Int32.Parse(listAssessmentBundles.SelectedValue),
               Active = true,
               DateCreated = DateTime.Today,
               IsUnproctored = false
               

            };

            if (listCampaignType.SelectedValue.Equals("1"))
            {
                item.IsUnproctored = true;
                item.StartDate = txtStartDate.SelectedDate;
                item.EndDate = txtEndDate.SelectedDate;

            }

            var ret = new CampaignService().Add(item);

            lblStatus.ShowMessage(ret);

            if (ret.IsDone)
            {
                txtCampaignName.Text = string.Empty;

                RegisterScript();

                var admin = new AdminUserService().GetCurrentAdmin();

                new AccessLogService().LogEntry(new AccessLog { AdminId = admin.AdminId, LogEntryType = "User added a campaign", LogEntryDetails = "User added a campaign [" + item.CampaignName + "]", LogDate = DateTime.Now, IpAddress = Request.UserHostAddress, UserAgent = Request.UserAgent });

            }
        }

    }
}