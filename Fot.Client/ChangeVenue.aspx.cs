using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Client.Models;
using Fot.Client.Services;

namespace Fot.Client
{
    public partial class ChangeVenue : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = 0;

                if (Int32.TryParse(Request.QueryString["id"], out id))
                {
                     LoadSchedule(id);
                }
                else
                {
                    Response.Redirect("Details.aspx");
                }
               
            }
        }

        private void LoadSchedule(int id)
        {
            var schedule = new CandidateService().GetCandidateSchedule(id);

            if (schedule != null)
            {
                hidCampaignEntryId.Value = schedule.CampaignEntryId.ToString();
                hidCampaignId.Value = schedule.CampaignId.ToString();

                hidCurrentSessionId.Value = schedule.SessionId.ToString();

                lblLocation.Text = schedule.Location;
                lblCenter.Text = schedule.CenterName;
                lblDateTime.Text = string.Format("{0} @ {1}", schedule.TestDate.ToString("dd-MMM-yyyy"),
                                                 schedule.TimeText);


                if (schedule.TestDate == DateTime.Today)
                {
                    Response.Redirect("Details.aspx");
                }

            }
            else
            {
                Response.Redirect("Details.aspx");
            }
        }

        protected void bttnChangeVenue_Click(object sender, EventArgs e)
        {
            if (listSessions.SelectedIndex >= 0)
            {
                Change();
            }
            else
            {
               lblStatus.ShowMessage(new AppMessage{IsDone = false, Message = "No Date/Time was selected.", Status = MessageStatus.Error});
            }
            
        }

        private void Change()
        {
            int CampaignEntryId = Int32.Parse(hidCampaignEntryId.Value);
            int sessionId = Int32.Parse(listSessions.SelectedValue);

           var app = new CandidateService().UpdateCandidateSchedule(CampaignEntryId, sessionId);

            LoadSchedule(CampaignEntryId);

            lblStatus.ShowMessage(app);

            listLocations.DataBind();

        }

        protected void listLocations_SelectedIndexChanged(object sender, EventArgs e)
        {
            listCenters.DataBind();
            
        }

        protected void listCenters_SelectedIndexChanged(object sender, EventArgs e)
        {
            listSessions.DataBind();
        }

   
    }
}