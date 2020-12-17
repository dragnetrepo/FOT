using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fot.Admin.Client
{
    public partial class OnlineTests : System.Web.UI.Page
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
                    Response.Redirect(UrlMapper.ManageCampaigns);
                }


            }
        }

        private void LoadForUpdate(int id)
        {
            var item = new CampaignService().GetCampaign(id);

            if (item != null)
            {
                lblCampaignName.Text = item.CampaignName;

                hidId.Value = id.ToString();

            }
            else
            {
                Response.Redirect(UrlMapper.ManageCampaigns);
            }
        }

        public List<OnlineTestViewModel> GetEntries(int startRow, int maxRows, string searchTerm, int campaignId)
        {
            var candidateService = new CandidateService();
            var ctx = candidateService.Context;

            IQueryable<CandidateAssessment> items = ctx.CandidateAssessments.Where(x => x.CampaignEntry.CampaignId == campaignId && x.SaveCount > 0);


 

            if (string.IsNullOrWhiteSpace(searchTerm) == false)
            {
                items = items.Where(
                       x =>
                       (x.CampaignEntry.Candidate.Username.ToLower().Equals(searchTerm.ToLower()) || x.CampaignEntry.Candidate.FirstName.ToLower().Equals(searchTerm.ToLower()) ||
                         x.CampaignEntry.Candidate.LastName.ToLower().Equals(searchTerm.ToLower()) || x.CampaignEntry.Candidate.MobileNo.Equals(searchTerm)));
            }

            var query = items.Select(x => new OnlineTestViewModel
            {
                CampaignEntryId = x.CampaignEntryId,
                Username = x.CampaignEntry.Candidate.Username,
                SaveCount = x.SaveCount,
                FirstUpdated = x.FirstUpdated.Value,
                LastUpdated = x.LastUpdated.Value,
                CurrentAssessment = x.CurrentAssessment,
                TimeRemainingMinutes = x.TimeRemainingMinutes
            });


            if (startRow >= 0)
            {
                query = query.OrderByDescending(x => x.LastUpdated).Skip(startRow).Take(maxRows);
            }


            return query.ToList();
        }


        public int CountEntries(string searchTerm, int campaignId)
        {
            var candidateService = new CandidateService();
            var ctx = candidateService.Context;

            IQueryable<CandidateAssessment> items = ctx.CandidateAssessments.Where(x => x.CampaignEntry.CampaignId == campaignId && x.SaveCount > 0);

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return items.Count();
            }
            else
            {
                
                return items.Count(x =>
                      (x.CampaignEntry.Candidate.Username.ToLower().Equals(searchTerm.ToLower()) || x.CampaignEntry.Candidate.FirstName.ToLower().Equals(searchTerm.ToLower()) ||
                        x.CampaignEntry.Candidate.LastName.ToLower().Equals(searchTerm.ToLower()) || x.CampaignEntry.Candidate.MobileNo.Equals(searchTerm)));

            }


        }

        protected void bttnSearch_Click(object sender, EventArgs e)
        {
            RadGrid1.DataBind();
        }

        protected void bttnBackToCampaignDetails_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlMapper.CampaignDetails + "?id=" + hidId.Value);
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {
            if (e.Argument.Equals("Rebind"))
            {
                RadGrid1.DataBind();
            }
        }
    }
}