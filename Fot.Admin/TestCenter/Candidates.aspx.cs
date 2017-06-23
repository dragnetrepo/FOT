using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Models;
using Fot.Admin.Services;
using OfficeOpenXml;

namespace Fot.Admin.TestCenter
{
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.CenterAdmin)]
    public partial class ViewCandidates : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = 0;


                if (Int32.TryParse(Request.QueryString["id"], out id))
                {
                    LoadSession(Request.QueryString["id"]);
                }
                else
                {
                    Response.Redirect("Schedules.aspx");
                }


            }
        }

        private void LoadSession(string dateStr)
        {
            if (dateStr.Length != 8)
            {
                Response.Redirect("Schedules.aspx");
            }
            else
            {
                var d = dateStr.Substring(0, 2);
                var m = dateStr.Substring(2, 2);
                var y = dateStr.Substring(4, 4);

                DateTime date;

                if (DateTime.TryParse(m + "-" + d + "-" + y, out date))
                {

                    hidId.Value = date.ToString("dd-MMM-yyyy");

                    lblDate.Text = date.ToString("dd-MMM-yyyy");

                }
                else
                {
                    Response.Redirect("Schedules.aspx");
                }
            }
        }

        protected void bttnDownload_Click(object sender, EventArgs e)
        {
            DoDownload();
        }

        private void DoDownload()
        {
            var service = new TestSessionService();

            var list = service.GetCandidatesScheduledInCenter(DateTime.Parse(hidId.Value), -1, -1);

            if (list.Count > 0)
            {

                var sheetName = "Scheduled_Candidates_" + hidId.Value;

                using (var excelEngine = new ExcelPackage())
                {


                    var sheet = excelEngine.Workbook.Worksheets.Add(sheetName);


                    sheet.Cells[1, 1].Value = "Username";
                    sheet.Cells[1, 2].Value = "Firstname";
                    sheet.Cells[1, 3].Value = "Lastname";
                    sheet.Cells[1, 4].Value = "Session Time";


                  

                    using (var range = sheet.Cells["A1:D1"])
                    {

                        range.Style.Font.Bold = true;
                        range.Style.Font.Color.SetColor(Color.Teal);
                        range.AutoFitColumns(40);
                    }


                    for (int i = 0; i < list.Count; i++)
                    {
                        sheet.Cells[i + 2, 1].Value = list[i].Username;
                        sheet.Cells[i + 2, 2].Value = list[i].Firstname;
                        sheet.Cells[i + 2, 3].Value = list[i].Lastname;
                        sheet.Cells[i + 2, 4].Value = list[i].SessionTime;
                    }


                    Response.BinaryWrite(excelEngine.GetAsByteArray());
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=" + sheet.Name + ".xlsx");
                }

            }


        }
    }
}