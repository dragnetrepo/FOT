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
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Author)]
    public partial class Assessments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void bttnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlMapper.AddOrEditAssessment);
        }

        protected void bttnImportAssessment_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlMapper.ImportAssessment);
        }

        protected void bttnSearch_Click(object sender, EventArgs e)
        {
            RadGrid1.DataBind();
        }

        protected void bttnDownload_Click(object sender, EventArgs e)
        {
            DoDownLoad();
        }

        private void DoDownLoad()
        {
            var ctx = new ServiceBase().Context;

            var list = ctx.Assessments
                .Select(x => new
                {
                    AssessmentName = x.Name,
                    Developer = x.AuthorId.HasValue ? x.AssessmentAuthor.AuthorName : string.Empty,
                    YearCreated = x.DateAdded.Year.ToString(),
                    AssessmentType = x.AssessmentType.ToString(),
                    Deployments =
                        x.AssessmentBundleEntries.Any()
                            ? x.AssessmentBundleEntries.Sum(
                                y => y.AssessmentBundle.Campaigns.Count(z => z.CampaignEntries.Any(v => v.Tested)))
                            : 0


                }).ToList();



            if (list.Count > 0)
            {
                var sheetName = "Assessments_Summaries_" + DateTime.Today.ToString("dd-MMM-yyyy");

                using (var package = new ExcelPackage())
                {


                    var worksheet = package.Workbook.Worksheets.Add(sheetName);







                    #region headerRegion

                    worksheet.Cells[1, 1].Value = "ASSESSMENT NAME";
                    worksheet.Cells[1, 2].Value = "ASSESSMENT DEVELOPER";
                    worksheet.Cells[1, 3].Value = "ASSESSMENT TYPE";
                    worksheet.Cells[1, 4].Value = "YEAR CREATED";
                    worksheet.Cells[1, 5].Value = "DEPLOYMENTS";








                    using (var range = worksheet.Cells[1, 1, 1, 5])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Font.Color.SetColor(Color.Teal);
                        range.AutoFitColumns(40);


                    }




                    #endregion

                    for (int i = 0; i < list.Count; i++)
                    {


                        int row = i + 2;



                        worksheet.Cells[row, 1].Value = list[i].AssessmentName;
                        worksheet.Cells[row, 2].Value = list[i].Developer;
                        worksheet.Cells[row, 3].Value = list[i].AssessmentType;
                        worksheet.Cells[row, 4].Value = list[i].YearCreated;
                        worksheet.Cells[row, 5].Value = list[i].Deployments;
     


                    }



                    
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=" + sheetName + ".xlsx");
                    Response.BinaryWrite(package.GetAsByteArray());
                    Response.End();
                }
            }


        }
    }
}