using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;

namespace Fot.Admin.Client
{
   [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Users)]
   [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Schedule)]
    public partial class Scheduling : System.Web.UI.Page
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
            var campaignService = new PartnerCampaignService();

            var item = campaignService.GetCampaign(id);

            if (item != null)
            {
                if (item.IsUnproctored)
                {
                    Response.Redirect(UrlMapper.CampaignDetails + "?id=" + id);
                    
                }

                lblCampaignName.Text = item.CampaignName;

                var partner = new PartnerService().GetPartner(item.PartnerId);
                var amountScheduled = new PartnerWalletScheduleService().GetTotalAmountScheduled(item.PartnerId);

                lblAmountLeft.Text = (partner.WalletBalance - amountScheduled).ToString("#,##0.00");

                hidId.Value = id.ToString();

                var stats = campaignService.GetCampaignStats(id);

                lblStats.Text = String.Format(" Total = {0}, Scheduled = {1}, Unscheduled = {2}", stats.Total,
                                              stats.Scheduled, stats.Unscheduled);


                lblUnscheduleStats.Text = String.Format(" Total = {0}, Scheduled = {1}, Unscheduled = {2}", stats.Total,
                                        stats.Scheduled, stats.Unscheduled);

            }
            else
            {
                Response.Redirect(UrlMapper.ManageCampaigns);
            }
        }

        protected void bttnBackToCampaignDetails_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlMapper.CampaignDetails + "?id=" + hidId.Value);
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {

        }

        protected void listLocations_SelectedIndexChanged(object sender, EventArgs e)
        {
            listCenters.DataBind();
            listSessions.DataBind();

            lblTotalInLocation.Text =
                new PartnerCampaignService().GetCandidateCountInLocation(Int32.Parse(hidId.Value),
                                                                  Int32.Parse(listLocations.SelectedValue)).ToString(
                                                                      "#,##0");

        }

        protected void chkScheduleMessage_CheckedChanged(object sender, EventArgs e)
        {
           
               
           
        }

        protected void RadGrid1_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if(e.CommandName.Equals("Schedule"))
            {
                if (listSessions.SelectedIndex >= 0)
                {
                    int sessionId = Int32.Parse(listSessions.SelectedValue);

                    int campaignId = Int32.Parse(hidId.Value);
                    int locationId = Int32.Parse(listLocations.SelectedValue);


                    int index = e.Item.ItemIndex;

                    var entryId = Int32.Parse(e.Item.OwnerTableView.DataKeyValues[index]["EntryId"].ToString());


                    var totalPossible = GetTotalPossible();

                    if (totalPossible > 0)
                    {

                        new CampaignEntryService().ScheduleCandidate(entryId, sessionId);


                        UpdateUI(campaignId, locationId);
                    }

                }
            }
        }

        protected void bttnSchedule_Click(object sender, EventArgs e)
        {
           
            ProcessSchedule();

        }

        private void ProcessSchedule()
        {
            if (listSessions.SelectedIndex >= 0)
            {
                int CampaignId = Int32.Parse(hidId.Value);
                int LocationId = Int32.Parse(listLocations.SelectedValue);

                var campaignService = new PartnerCampaignService();

                int totalInLocation = campaignService.GetCandidateCountInLocation(CampaignId, LocationId);
                int sessionId = Int32.Parse(listSessions.SelectedValue);

                int slots = new TestSessionService().GetSessionDetails(sessionId).AvailableSlots;

                int numToSchedule = totalInLocation >= slots ? slots : totalInLocation;

                //process based on available wallet balance
                var totalPossible = GetTotalPossible();

                numToSchedule = totalPossible >= numToSchedule ? numToSchedule : totalPossible;


                //end processing

                new CampaignEntryService().ScheduleCandidates(CampaignId, LocationId, sessionId, numToSchedule);


                UpdateUI(CampaignId, LocationId);
               
            }
        }

        private int GetTotalPossible()
        {
            var admin = new AdminUserService().GetCurrentAdmin();
            var center = new PartnerCenterService().GetCenter(Int32.Parse(listCenters.SelectedValue));
            var partner = new PartnerService().GetPartner(admin.PartnerId.Value);
            var amountScheduled = new PartnerWalletScheduleService().GetTotalAmountScheduled(admin.PartnerId.Value);

            var amountLeft = partner.WalletBalance - amountScheduled;

            var centerCost = center.IsPrivateCenter ? partner.CostPerTestPrivate.Value : partner.CostPerTestPublic;

            int totalPossible = (int) (amountLeft/centerCost);
            return totalPossible;
        }

        private void ProcessUnschedule()
        {
            if (listUnscheduleSession.SelectedIndex >= 0)
            {
                int CampaignId = Int32.Parse(hidId.Value);
                int LocationId = Int32.Parse(listUnscheduleLocation.SelectedValue);

                int sessionId = Int32.Parse(listUnscheduleSession.SelectedValue);


                new CampaignEntryService().UnscheduleCandidates(CampaignId, sessionId);

                UpdateUI(CampaignId, LocationId);
            }
        }

        private void UpdateUI(int CampaignId, int LocationId)
        {
            var campaignService = new PartnerCampaignService();

            listLocations.DataBind();
            listCenters.DataBind();
            listSessions.DataBind();
            RadGrid1.DataBind();

            listUnscheduleLocation.DataBind();
            listUnscheduleCenter.DataBind();
            listUnscheduleSession.DataBind();
            RadGrid2.DataBind();



            lblTotalInLocation.Text = campaignService.GetCandidateCountInLocation(CampaignId, LocationId).ToString("#,##0");

            lblUnscheduleCandidatesInLocation.Text = campaignService.GetScheduledCandidateCountInLocation(Int32.Parse(hidId.Value),Int32.Parse(listUnscheduleLocation.SelectedIndex >= 0 ? listUnscheduleLocation.SelectedValue : "0")).ToString( "#,##0");


            var stats = campaignService.GetCampaignStats(CampaignId);

            lblStats.Text = String.Format(" Total = {0}, Scheduled = {1}, Unscheduled = {2}", stats.Total,
                                         stats.Scheduled, stats.Unscheduled);


            lblUnscheduleStats.Text = String.Format(" Total = {0}, Scheduled = {1}, Unscheduled = {2}", stats.Total,
                                    stats.Scheduled, stats.Unscheduled);


            var item = campaignService.GetCampaign(CampaignId);

            var partner = new PartnerService().GetPartner(item.PartnerId);
            var amountScheduled = new PartnerWalletScheduleService().GetTotalAmountScheduled(item.PartnerId);

            lblAmountLeft.Text = (partner.WalletBalance - amountScheduled).ToString("#,##0.00");
        }


        protected void listUnscheduleLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            listUnscheduleCenter.DataBind();
            listUnscheduleSession.DataBind();

            lblUnscheduleCandidatesInLocation.Text =
                new PartnerCampaignService().GetScheduledCandidateCountInLocation(Int32.Parse(hidId.Value),
                                                                  Int32.Parse(listUnscheduleLocation.SelectedValue)).ToString(
                                                                      "#,##0");
        }

        protected void RadGrid2_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("Unschedule"))
            {
                if (listUnscheduleSession.SelectedIndex >= 0)
                {
                    int sessionId = Int32.Parse(listUnscheduleSession.SelectedValue);

                    int campaignId = Int32.Parse(hidId.Value);
                    int locationId = Int32.Parse(listUnscheduleLocation.SelectedValue);


                    int index = e.Item.ItemIndex;

                    var entryId = Int32.Parse(e.Item.OwnerTableView.DataKeyValues[index]["EntryId"].ToString());

                    new CampaignEntryService().UnscheduleCandidate(entryId, sessionId);
                    
                    UpdateUI(campaignId, locationId);
                }
            }
        }

        protected void bttnUnschedule_Click(object sender, EventArgs e)
        {
            
                 ProcessUnschedule();
             
        }

        protected void listUnscheduleLocation_DataBound(object sender, EventArgs e)
        {
            if (listUnscheduleLocation.Items.Count > 0)
            {
                lblUnscheduleCandidatesInLocation.Text =
                    new PartnerCampaignService().GetScheduledCandidateCountInLocation(Int32.Parse(hidId.Value),
                                                                                      Int32.Parse(
                                                                                          listUnscheduleLocation
                                                                                              .SelectedValue)).ToString(
                                                                                                  "#,##0");
            }
        }

        protected void listLocations_DataBound(object sender, EventArgs e)
        {
            if (listLocations.Items.Count > 0)
            {
                lblTotalInLocation.Text =
                    new PartnerCampaignService().GetCandidateCountInLocation(Int32.Parse(hidId.Value),
                                                                             Int32.Parse(listLocations.SelectedValue))
                                                .ToString(
                                                    "#,##0");
            }
        }
    }
}