using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Client;
using Fot.Admin.Infrastructure;
using Fot.Admin.Services;
using OfficeOpenXml;

namespace Fot.Admin
{
    public partial class CampaignFeedback : System.Web.UI.Page
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

            var ctx = new ServiceBase().Context;

            if (item != null)
            {
                lblCampaignName.Text = item.CampaignName;
                hidId.Value = id.ToString();

                var centers = ctx.CampaignEntries.Where(x => x.CampaignId == id && x.Tested)
                    .Select(x => new {x.TestSession.Center.CenterName, x.TestSession.CenterId}).Distinct().ToList();

                listCenters.Items.Clear();

                centers.ForEach(x => listCenters.Items.Add(new ListItem(x.CenterName, x.CenterId.ToString())));

                listCenters.Items.Insert(0, new ListItem("All Centers", "0"));

                listCenters.SelectedValue = "0";

            }
        }


        protected void bttnBackToCampaignDetails_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlMapper.CampaignDetails + "?id=" + hidId.Value);
        }

        public List<FeedbackViewModel> GetStats()
        {

            int id = Int32.Parse(Request.QueryString["id"]);

            var ctx = new ServiceBase().Context;

            var centerId = 0;

            if (!string.IsNullOrWhiteSpace(listCenters.SelectedValue))
            {
                centerId = Int32.Parse(listCenters.SelectedValue);
            }

            if (centerId == 0)
            {

                var entries = ctx.TestFeedbacks.Where(x => x.CampaignEntry.CampaignId == id)
                    .Select(x => new FeedbackViewModel
                    {
                        Directions = x.Directions,
                        WaitTime = x.WaitTime,
                        Professionalism = x.Professionalism,
                        StartTime = x.StartTime,
                        Briefing = x.Briefing,
                        Registration = x.Registration,
                        Overall = x.Overall,
                        UnsatisfactoryArea = x.UnsatisfactoryArea,
                        SatisfactoryArea = x.SatisfactoryArea

                    }).AsNoTracking().ToList();

                return entries;
            }
            else
            {

                var entries = ctx.TestFeedbacks.Where(x => x.CampaignEntry.CampaignId == id && x.CampaignEntry.TestSession.CenterId == centerId)
                    .Select(x => new FeedbackViewModel
                    {
                        Directions = x.Directions,
                        WaitTime = x.WaitTime,
                        Professionalism = x.Professionalism,
                        StartTime = x.StartTime,
                        Briefing = x.Briefing,
                        Registration = x.Registration,
                        Overall = x.Overall,
                        UnsatisfactoryArea = x.UnsatisfactoryArea,
                        SatisfactoryArea = x.SatisfactoryArea

                    }).AsNoTracking().ToList();

                return entries;
                
            }
        }

        protected void listCenters_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void bttnDownload_Click(object sender, EventArgs e)
        {
            DownloadComments();
        }

        private void DownloadComments()
        {
            int id = Int32.Parse(Request.QueryString["id"]);

            var ctx = new ServiceBase().Context;


            var list = ctx.TestFeedbacks.Where(x => x.CampaignEntry.CampaignId == id)
                    .Select(x => new 
                    {
                        Username = x.CampaignEntry.Candidate.Username,
                        Comments = x.Comments

                    }).AsNoTracking().ToList();

            if (list.Count > 0)
            {
                var sheetName = "All_Feedback_Comments_" + DateTime.Today.ToString("dd-MMM-yyyy");

                using (var package = new ExcelPackage())
                {


                    var worksheet = package.Workbook.Worksheets.Add(sheetName);







                    #region headerRegion

                    worksheet.Cells[1, 1].Value = "USERNAME";
                    worksheet.Cells[1, 2].Value = "COMMENTS";








                    using (var range = worksheet.Cells[1, 1, 1, 3])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Font.Color.SetColor(Color.Teal);
                        range.AutoFitColumns(40);
                    }

                    worksheet.Cells[1,2].AutoFitColumns(150); 


                    #endregion

                    for (int i = 0; i < list.Count; i++)
                    {


                        int row = i + 2;



                        worksheet.Cells[row, 1].Value = list[i].Username;
                        worksheet.Cells[row, 2].Value = list[i].Comments;



                    }



                    Response.BinaryWrite(package.GetAsByteArray());
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=" + sheetName + ".xlsx");

                }
            }

        }
    }



}