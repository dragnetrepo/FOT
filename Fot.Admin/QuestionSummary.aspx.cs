using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Models;
using Fot.Admin.Services;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Fot.Admin
{
    public partial class QuestionSummary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSummary();
        }



        public void LoadSummary()
        {
            var id = Int32.Parse(Request.QueryString["id"]);

            var ctx = new ServiceBase().Context;

            var assessment = ctx.Assessments.Find(id);

            if (assessment != null)
            {
                lblSummary.Text = assessment.Name;
            }
            else
            {
                Response.Redirect("Assessments.aspx");
            }
        }


        public List<QuestionSummaryViewModel> GetDetails()
        {
            var id = Int32.Parse(Request.QueryString["id"]);

            var ctx = new ServiceBase().Context;

            var items =
                    ctx.AssessmentQuestions.Where(x => x.AssessmentId == id).Select(x => new QuestionSummaryViewModel
                    {
                        QuestionId = x.QuestionId,
                        AnswerType = x.AnswerType,
                        Level = x.DifficultyLevel.HasValue ? x.QuestionDifficultyLevel.LevelName : string.Empty,
                        Topic = x.TopicId.HasValue ? x.AssessmentTopic.Topic : string.Empty,
                        Group = x.GroupId.HasValue ? x.QuestionGroup.GroupName : string.Empty,
                        OptionCount = x.AssessmentAnswers.Count(),
                        TotalServed = ctx.ShownQuestions.Count(y => y.QuestionId == x.QuestionId),
                        TotalRight = ctx.ShownQuestions.Count(y => y.QuestionId == x.QuestionId && y.Correct.Value),
                        TotalWrong = ctx.ShownQuestions.Count(y => y.QuestionId == x.QuestionId && y.Correct.Value == false)
                    }).OrderByDescending(x => x.QuestionId).ToList();


            return items;
        }


        public void DoDownload()
        {
            var id = Int32.Parse(Request.QueryString["id"]);


            var ctx = new ServiceBase().Context;

            var list =
                    ctx.AssessmentQuestions.Where(x => x.AssessmentId == id).Select(x => new QuestionSummaryViewModel
                    {
                        QuestionId = x.QuestionId,
                        AnswerType = x.AnswerType,
                        Level = x.DifficultyLevel.HasValue ? x.QuestionDifficultyLevel.LevelName : string.Empty,
                        Topic = x.TopicId.HasValue ? x.AssessmentTopic.Topic : string.Empty,
                        Group = x.GroupId.HasValue ? x.QuestionGroup.GroupName : string.Empty,
                        OptionCount = x.AssessmentAnswers.Count(),
                        TotalServed = ctx.ShownQuestions.Count(y => y.QuestionId == x.QuestionId),
                        TotalRight = ctx.ShownQuestions.Count(y => y.QuestionId == x.QuestionId && y.Correct.Value),
                        TotalWrong = ctx.ShownQuestions.Count(y => y.QuestionId == x.QuestionId && y.Correct.Value == false)
                    }).OrderByDescending(x => x.QuestionId).ToList();


            

            var sheetName = "Question_Summary_"+ id +"_" + DateTime.Today.ToString("dd-MMM-yyyy");

            using (var package = new ExcelPackage())
            {


                var worksheet = package.Workbook.Worksheets.Add(sheetName);





                #region headerRegion

                worksheet.Cells[1, 1].Value = "S/N";
                worksheet.Cells[1, 2].Value = "QUESTION ID";
                worksheet.Cells[1, 3].Value = "TOTAL";
                worksheet.Cells[1, 4].Value = "RIGHT";
                worksheet.Cells[1, 5].Value = "WRONG";
                worksheet.Cells[1, 6].Value = "OPTIONS";
                worksheet.Cells[1, 7].Value = "LEVEL";
                worksheet.Cells[1, 8].Value = "GROUP";
                worksheet.Cells[1, 9].Value = "TOPIC";
                worksheet.Cells[1, 10].Value = "TYPE";


                worksheet.Cells[1, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells[1, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells[1, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells[1, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells[1, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;


                using (var range = worksheet.Cells[1, 1, 1, 7])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Font.Color.SetColor(Color.Teal);
                    range.AutoFitColumns(10);


                }

                using (var range = worksheet.Cells[1, 8, 1, 9])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Font.Color.SetColor(Color.Teal);
                    range.AutoFitColumns(40);


                }

                using (var range = worksheet.Cells[1, 10, 1, 10])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Font.Color.SetColor(Color.Teal);
                    range.AutoFitColumns(10);


                }




                #endregion

                for (int i = 0; i < list.Count; i++)
                {


                    int row = i + 2;



                    worksheet.Cells[row, 1].Value = (i + 1).ToString();
                    worksheet.Cells[row, 2].Value = list[i].QuestionId;
                    worksheet.Cells[row, 3].Value = list[i].TotalServed;
                    worksheet.Cells[row, 4].Value = list[i].TotalRight;
                    worksheet.Cells[row, 5].Value = list[i].TotalWrong;
                    worksheet.Cells[row, 6].Value = list[i].OptionCount;
                    worksheet.Cells[row, 7].Value = list[i].Level;
                    worksheet.Cells[row, 8].Value = list[i].Group;
                    worksheet.Cells[row, 9].Value = list[i].Topic;
                    worksheet.Cells[row, 10].Value = list[i].AnswerType;


                }


                var admin = new AdminUserService().GetCurrentAdmin();

                new AccessLogService().LogEntry(new AccessLog { AdminId = admin.AdminId, LogEntryType = "Question Summary Download", LogEntryDetails = sheetName, LogDate = DateTime.Now, IpAddress = Request.UserHostAddress, UserAgent = Request.UserAgent });




                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + sheetName + ".xlsx");
                Response.BinaryWrite(package.GetAsByteArray());
                Response.End();

            }



        }

        protected void bttnDownload_Click(object sender, EventArgs e)
        {
            DoDownload();

   }
    }
}