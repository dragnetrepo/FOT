using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;
using OfficeOpenXml;

namespace Fot.Admin
{
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Admin)]
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Schedule)]
    public partial class CampaignCandidates : System.Web.UI.Page
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

                if (item.IsUnproctored)
                {
                    RadGrid1.Columns[6].Visible = false;
                    RadTabStrip1.Tabs[2].Visible = false;
                }

                var admin = new AdminUserService().GetCurrentAdmin();

                if (!admin.IsGlobalAdmin)
                {
                    RadTabStrip1.Tabs[2].Visible = false;
                }
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
            if (e.Argument.Equals("Rebind"))
            {
                RadGrid1.DataBind();
            }
        }

        protected void bttnSearch_Click(object sender, EventArgs e)
        {
            RadGrid1.DataBind();
        }

        protected void bttnUploadCandidates_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlMapper.CandidateUpload + "?id=" + hidId.Value);
        }

        protected void bttnDownload_Click(object sender, EventArgs e)
        {
            DoDownload();
        }

        private void DoDownload()
        {
            int campaignID = Int32.Parse(hidId.Value);

            int type = Int32.Parse(listDownloadType.SelectedValue);

            if (type == 1)
            {
                DownloadAll(campaignID);
            }
            else
            {
                DownloadScheduled(campaignID);
            }
        }

        private void DownloadAll(int campaignID)
        {
            var list = new CampaignEntryService().GetCampaignCandidates(campaignID, null, -1, -1);

            if (list.Count > 0)
            {
                var sheetName = "All_Candidates_" + DateTime.Today.ToString("dd-MMM-yyyy");

                using (var package = new ExcelPackage())
                {


                    var worksheet = package.Workbook.Worksheets.Add(sheetName);







                    #region headerRegion

                    worksheet.Cells[1, 1].Value = "USERNAME";
                    worksheet.Cells[1, 2].Value = "PASSWORD";
                    worksheet.Cells[1, 3].Value = "FIRSTNAME";
                    worksheet.Cells[1, 4].Value = "LASTNAME";
                    worksheet.Cells[1, 5].Value = "MOBILE NUMBER";
                    worksheet.Cells[1, 6].Value = "EMAIL";
                    worksheet.Cells[1, 7].Value = "LOCATION";







                    using (var range = worksheet.Cells[1, 1, 1, 9])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Font.Color.SetColor(Color.Teal);
                        range.AutoFitColumns(40);


                    }




                    #endregion

                    for (int i = 0; i < list.Count; i++)
                    {


                        int row = i + 2;



                        worksheet.Cells[row, 1].Value = list[i].Username;
                        worksheet.Cells[row, 2].Value = list[i].Password;
                        worksheet.Cells[row, 3].Value = list[i].FirstName;
                        worksheet.Cells[row, 4].Value = list[i].LastName;
                        worksheet.Cells[row, 5].Value = list[i].MobileNo;
                        worksheet.Cells[row, 6].Value = list[i].Email;
                        worksheet.Cells[row, 7].Value = list[i].LocationName;


                    }




                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=" + sheetName + ".xlsx");
                    Response.BinaryWrite(package.GetAsByteArray());
                    Response.End();

                }
            }

        }

        private void DownloadScheduled(int campaignID)
        {
            var list = new CampaignEntryService().GetCampaignCandidateScheduledWithCenterInfo(campaignID);

            if (list.Count > 0)
            {
                var sheetName = "All_Scheduled_Candidates_" + DateTime.Today.ToString("dd-MMM-yyyy");

                using (var package = new ExcelPackage())
                {


                    var worksheet = package.Workbook.Worksheets.Add(sheetName);







                    #region headerRegion

                    worksheet.Cells[1, 1].Value = "USERNAME";
                    worksheet.Cells[1, 2].Value = "PASSWORD";
                    worksheet.Cells[1, 3].Value = "FIRSTNAME";
                    worksheet.Cells[1, 4].Value = "LASTNAME";
                    worksheet.Cells[1, 5].Value = "MOBILE NUMBER";
                    worksheet.Cells[1, 6].Value = "LOCATION";
                    worksheet.Cells[1, 7].Value = "CENTER";
                    worksheet.Cells[1, 8].Value = "TEST DATE";
                    worksheet.Cells[1, 9].Value = "TEST TIME";
                    worksheet.Cells[1, 10].Value = "EMAIL";







                    using (var range = worksheet.Cells[1, 1, 1, 11])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Font.Color.SetColor(Color.Teal);
                        range.AutoFitColumns(40);


                    }




                    #endregion

                    for (int i = 0; i < list.Count; i++)
                    {


                        int row = i + 2;



                        worksheet.Cells[row, 1].Value = list[i].Username;
                        worksheet.Cells[row, 2].Value = list[i].Password;
                        worksheet.Cells[row, 3].Value = list[i].FirstName;
                        worksheet.Cells[row, 4].Value = list[i].LastName;
                        worksheet.Cells[row, 5].Value = list[i].MobileNo;
                        worksheet.Cells[row, 6].Value = list[i].LocationName;
                        worksheet.Cells[row, 7].Value = list[i].CenterName;
                        worksheet.Cells[row, 8].Value = list[i].TestDate.ToString("dd-MMM-yyyy");
                        worksheet.Cells[row, 9].Value = list[i].TestTime;
                        worksheet.Cells[row, 10].Value = list[i].Email;



                    }




                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=" + sheetName + ".xlsx");
                    Response.BinaryWrite(package.GetAsByteArray());
                    Response.End();

                }
            }
        }

        protected void bttnChangeLocation_Click(object sender, EventArgs e)
        {
            ChangeLocations();
        }

        private void ChangeLocations()
        {
            int campaignId = Int32.Parse(hidId.Value);

            int selectedLocationId = Int32.Parse(listLocation.SelectedValue);

            new CampaignEntryService().UpdateLocationForUnscheduledCandidates(campaignId, selectedLocationId);

            RadGrid1.DataBind();

        }
    }
}