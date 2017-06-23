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
    public partial class InvitationResponses : System.Web.UI.Page
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

        protected void bttnBackToCampaignDetails_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlMapper.CampaignDetails + "?id=" + hidId.Value);
        }

        protected void bttnDownload_Click(object sender, EventArgs e)
        {
            DownloadResponses();
        }

        private void DownloadResponses()
        {
            int campaignId = Int32.Parse(hidId.Value);

            var list = new CampaignEntryService().GetCandidatesResponsesForDownload(campaignId);

            if (list.Count > 0)
            {
                var sheetName = "Candidates_Responses_" + DateTime.Today.ToString("dd-MMM-yyyy");

                using (var package = new ExcelPackage())
                {


                    var worksheet = package.Workbook.Worksheets.Add(sheetName);







                    #region headerRegion

                    worksheet.Cells[1, 1].Value = "USERNAME";
                    worksheet.Cells[1, 2].Value = "UNIQUE ID";
                    worksheet.Cells[1, 3].Value = "FIRSTNAME";
                    worksheet.Cells[1, 4].Value = "LASTNAME";
                    worksheet.Cells[1, 5].Value = "LOCATION";
                    worksheet.Cells[1, 6].Value = "EMAIL";
                    worksheet.Cells[1, 7].Value = "MOBILE NO";
                    worksheet.Cells[1, 8].Value = "ACCEPTED";
                    worksheet.Cells[1, 9].Value = "DATE ACCEPTED";








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
                        worksheet.Cells[row, 2].Value = list[i].UniqueID;
                        worksheet.Cells[row, 3].Value = list[i].FirstName;
                        worksheet.Cells[row, 4].Value = list[i].LastName;
                        worksheet.Cells[row, 5].Value = list[i].LocationName;
                        worksheet.Cells[row, 6].Value = list[i].Email;
                        worksheet.Cells[row, 7].Value = list[i].MobileNo;

                        if (list[i].Response != null)
                        {
                            worksheet.Cells[row, 8].Value = list[i].Response.AcceptSchedule ? "YES" : "NO";
                            worksheet.Cells[row, 9].Value = list[i].Response.DateResponded.ToString("dd-MMM-yyyy");
                        }



                    }



                    Response.BinaryWrite(package.GetAsByteArray());
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=" + sheetName + ".xlsx");

                }
            }
        }
    }
}