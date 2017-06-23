using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;

namespace Fot.Admin
{
    public partial class Settings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadDefaults();
            }
        }

        private void LoadDefaults()
        {
            var ctx = new ServiceBase().Context;

            var setting = ctx.Settings.FirstOrDefault(x => x.SettingName == "ENABLE_REVIEWING_NON_OWNED_ASSESSMENTS");

            if (setting != null)
            {
                bool enable = Convert.ToBoolean(setting.SettingValue);

                chkAssessmentReview.Checked = enable;
            }
        }

        protected void bttnAddSubject_Click(object sender, EventArgs e)
        {
          
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {
            if (e.Argument.Equals("Rebind"))
            {
                GridFeedback.DataBind();
            }

            if (e.Argument.Equals("Rebind2"))
            {
               GridAuthor.DataBind();
            }
        }

  

        protected void bttnUpdate_Click(object sender, EventArgs e)
        {
            var ctx = new ServiceBase().Context;

            var setting = ctx.Settings.FirstOrDefault(x => x.SettingName == "ENABLE_REVIEWING_NON_OWNED_ASSESSMENTS");

            if (setting != null)
            {
                

                setting.SettingValue = chkAssessmentReview.Checked.ToString();

                ctx.SaveChanges();

                var admin = new AdminUserService().GetCurrentAdmin();

                new AccessLogService().LogEntry(new AccessLog { AdminId = admin.AdminId, LogEntryType = chkAssessmentReview.Checked ? "Enabled Partner Review of Non-Owned Assessments" : "Disabled Partner Review of Non-Owned Assessments", LogEntryDetails = "User Changed The Status of Partner Reviewing of Non-Owned Assessments", LogDate = DateTime.Now, IpAddress = Request.UserHostAddress, UserAgent = Request.UserAgent });


                lblStatus.ShowMessage(new AppMessage{IsDone = true, Message = "Settings udpated successfully.", Status = MessageStatus.Success});
            }
        }

   
    }
}