using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;
using OfficeOpenXml;
using WebSupergoo.ABCpdf8;

namespace Fot.Admin.Client
{
   [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Users)]
   [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Result)]
    public partial class Results : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = 0;


                if (Int32.TryParse(Request.QueryString["id"], out id))
                {
                    LoadInfo(id);
                }
                else
                {
                    Response.Redirect(UrlMapper.ManageCampaigns);
                }


            }

        }

        private void LoadInfo(int id)
        {
            var item = new PartnerCampaignService().Campaigns.Where(x => x.CampaignId == id).Select(x => new
                {
                   x.CampaignName,
                   x.AssessmentBundle.Name,
                   x.CampaignEntries,
                   HasTopicsConfigured = x.AssessmentBundle.AssessmentBundleEntries.Any(y => y.Assessment.AdvancedOutputOptions && y.Assessment.AssessmentOutputConfigs.Any())

                }).FirstOrDefault();

            if(item != null)
            {
                lblCampaignName.Text = item.CampaignName;
                lblAssessmentBundle.Text = item.Name;

                int total = item.CampaignEntries.Count(x => x.Tested);
                lblTotalTested.Text = total.ToString("#,##0");

                hidId.Value = id.ToString();

                bttnDownload.Visible = bttnDownloadPdf.Visible =  total > 0;



                if (total > 500)
                {
                    listDownloadRange.Visible = true;

                    var dict = GetList(total, 500);

                    var list = dict.Select(x => new { Value = x.Key, Text = x.Value }).ToList();

                    list.ForEach(x => listDownloadRange.Items.Add(new ListItem(x.Text, x.Value)));
                }

                divTopicDownload.Visible = item.HasTopicsConfigured;

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
            DoDownload();
        }



   

        public void DoDownload()
        {
            int id = Int32.Parse(hidId.Value);


            var list = new AssessmentResultService().GetMCQResultsForDownload(id);

            list = list.OrderByDescending(x => x.OverallScore).ToList();

            var sheetName = "Result_Entries_" + DateTime.Today.ToString("dd-MMM-yyyy");

            using (var package = new ExcelPackage())
            {


                var worksheet = package.Workbook.Worksheets.Add(sheetName);




                int assessmentCount = list[0].ResultList.Count();



                #region headerRegion

                worksheet.Cells[1, 1].Value = "CLIENT_UNIQUE_ID";
                worksheet.Cells[1, 2].Value = "CANDIDATE NAME";
                worksheet.Cells[1, 3].Value = "DATE TESTED";
                worksheet.Cells[1, 4].Value = "EMAIL";
                worksheet.Cells[1, 5].Value = "MOBILE NUMBER";
                worksheet.Cells[1, 6].Value = "LOCATION";
                worksheet.Cells[1, 7].Value = "CENTER";



                var firstList = list[0].ResultList.ToList();



                for (int i = 0, val = 0; i < assessmentCount; i++, val = val + 3)
                {

                    worksheet.Cells[1, val + 8].Value = firstList[i].AssessmentName + " ( Raw Score )";
                    worksheet.Cells[1, val + 9].Value = firstList[i].AssessmentName + " ( % )";
                    worksheet.Cells[1, val + 10].Value = firstList[i].AssessmentName + " Total Questions";


                }


                worksheet.Cells[1, (assessmentCount * 3) + 8].Value = "Aggregate ( Raw Score )";
                worksheet.Cells[1, (assessmentCount * 3) + 9].Value = "Aggregate ( % )";
                worksheet.Cells[1, (assessmentCount * 3) + 10].Value = "Aggregate Total Questions";


                using (var range = worksheet.Cells[1, 1, 1, (11 + (assessmentCount * 3))])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Font.Color.SetColor(Color.Teal);
                    range.AutoFitColumns(40);


                }




                #endregion

                for (int i = 0; i < list.Count; i++)
                {


                    int row = i + 2;



                    worksheet.Cells[row, 1].Value = list[i].ClientUniqueId;
                    worksheet.Cells[row, 2].Value = list[i].Firstname + " " + list[i].Lastname;
                    worksheet.Cells[row, 3].Value = list[i].DateTested.ToString("dd-MMM-yyyy");
                    worksheet.Cells[row, 4].Value = list[i].Email;
                    worksheet.Cells[row, 5].Value = list[i].MobileNo;
                    worksheet.Cells[row, 6].Value = list[i].Locaton;
                    worksheet.Cells[row, 7].Value = list[i].CenterName;

                    var currentItem = list[i].ResultList.ToList();



                    for (int j = 0, val = 0; j < assessmentCount; j++, val = val + 3)
                    {

                        int questionCount = currentItem[j].TotalQuestions;

                        worksheet.Cells[row, val + 8].Value = currentItem[j].TestScore.Value.ToString();

                        var scorePercent = ((Convert.ToDouble(currentItem[j].TestScore.Value) / questionCount)) * 100.00;
                        worksheet.Cells[row, val + 9].Value = scorePercent.ToString("#,##0.##");

                        worksheet.Cells[row, val + 10].Value = questionCount.ToString();


                    }

                    worksheet.Cells[row, (assessmentCount * 3) + 8].Value = list[i].OverallScore.ToString();
                    worksheet.Cells[row, (assessmentCount * 3) + 9].Value = ((Convert.ToDouble(list[i].OverallScore) / list[i].OverallTotalQuestions) * 100.00).ToString("#,##0.##");
                    worksheet.Cells[row, (assessmentCount * 3) + 10].Value = list[i].OverallTotalQuestions.ToString();

                }



                
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + sheetName + ".xlsx");
                Response.BinaryWrite(package.GetAsByteArray());
                Response.End();

            }




        }

        public void DoDownloadTopic()
        {
            int id = Int32.Parse(hidId.Value);


            var list = new AssessmentResultService().GetMCQResultsForTopicDownload(id);

            list = list.OrderByDescending(x => x.OverallScore).ToList();

            var sheetName = "Result_Topic_Entries_" + DateTime.Today.ToString("dd-MMM-yyyy");

            using (var package = new ExcelPackage())
            {


                var worksheet = package.Workbook.Worksheets.Add(sheetName);




                int assessmentCount = list[0].ResultList.Count();



                #region headerRegion

                worksheet.Cells[1, 1].Value = "CLIENT_UNIQUE_ID";
                worksheet.Cells[1, 2].Value = "CANDIDATE NAME";
                worksheet.Cells[1, 3].Value = "DATE TESTED";
                worksheet.Cells[1, 4].Value = "EMAIL";
                worksheet.Cells[1, 5].Value = "MOBILE NUMBER";
                worksheet.Cells[1, 6].Value = "LOCATION";
                worksheet.Cells[1, 7].Value = "CENTER";



                var firstList = list[0].ResultList.ToList();


                int totalTopics = firstList.Sum(x => x.Topics.Count);


                int offSet = 8;



                for (int i = 0; i < assessmentCount; i++)
                {


                    int topicCount = firstList[i].Topics.Count;


                    var orderedTopics = firstList[i].Topics.OrderBy(x => x.TopicId).ToList();

                    for (int x = 0; x < topicCount; x++)
                    {
                        worksheet.Cells[1, offSet++].Value = firstList[i].AssessmentName + " ( " + orderedTopics[x].TopicName + " )";
                    }

                    worksheet.Cells[1, offSet++].Value = firstList[i].AssessmentName + " ( Raw Score )";
                    worksheet.Cells[1, offSet++].Value = firstList[i].AssessmentName + " ( % )";
                    worksheet.Cells[1, offSet++].Value = firstList[i].AssessmentName + " Total Questions";


                }


                worksheet.Cells[1, (assessmentCount * 3) + totalTopics + 8].Value = "Aggregate ( Raw Score )";
                worksheet.Cells[1, (assessmentCount * 3) + totalTopics + 9].Value = "Aggregate ( % )";
                worksheet.Cells[1, (assessmentCount * 3) + totalTopics + 10].Value = "Aggregate Total Questions";


                using (var range = worksheet.Cells[1, 1, 1, (11 + totalTopics + (assessmentCount * 3))])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Font.Color.SetColor(Color.Teal);
                    range.AutoFitColumns(40);


                }




                #endregion

                for (int i = 0; i < list.Count; i++)
                {


                    int row = i + 2;



                    worksheet.Cells[row, 1].Value = list[i].ClientUniqueId;
                    worksheet.Cells[row, 2].Value = list[i].Firstname + " " + list[i].Lastname;
                    worksheet.Cells[row, 3].Value = list[i].DateTested.ToString("dd-MMM-yyyy");
                    worksheet.Cells[row, 4].Value = list[i].Email;
                    worksheet.Cells[row, 5].Value = list[i].MobileNo;
                    worksheet.Cells[row, 6].Value = list[i].Locaton;
                    worksheet.Cells[row, 7].Value = list[i].CenterName;

                    var currentItem = list[i].ResultList.ToList();

                    int OffSet2 = 8;

                    for (int j = 0; j < assessmentCount; j++)
                    {

                        int topicCount = firstList[j].Topics.Count;

                        var orderedTopics = currentItem[j].Topics.OrderBy(x => x.TopicId).ToList();

                        for (int x = 0; x < topicCount; x++)
                        {
                            worksheet.Cells[row, OffSet2++].Value = orderedTopics[x].TopicScore;
                        }

                        int questionCount = currentItem[j].TotalQuestions;

                        worksheet.Cells[row, OffSet2++].Value = currentItem[j].TestScore.Value.ToString();

                        var scorePercent = ((Convert.ToDouble(currentItem[j].TestScore.Value) / questionCount)) * 100.00;
                        worksheet.Cells[row, OffSet2++].Value = scorePercent.ToString("#,##0.##");

                        worksheet.Cells[row, OffSet2++].Value = questionCount.ToString();


                    }

                    worksheet.Cells[row, (assessmentCount * 3) + totalTopics + 8].Value = list[i].OverallScore.ToString();
                    worksheet.Cells[row, (assessmentCount * 3) + totalTopics + 9].Value = ((Convert.ToDouble(list[i].OverallScore) / list[i].OverallTotalQuestions) * 100.00).ToString("#,##0.##");
                    worksheet.Cells[row, (assessmentCount * 3) + totalTopics + 10].Value = list[i].OverallTotalQuestions.ToString();

                }



                
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + sheetName + ".xlsx");
                Response.BinaryWrite(package.GetAsByteArray());
                Response.End();

            }



        }


        protected void bttnDownloadPdf_Click(object sender, EventArgs e)
        {
            DoPDFDownload();
        }

        private void DoPDFDownload()
        {
            #region pageHtmlStr
            var pageHtmlStr = @"<!DOCTYPE html>
<html>
<head>
    <title></title>
 
 
</head>
<body>
    <div style='padding: 20px;'>
        

        <table style='width:800px; border: 1px solid #ccc; height: 700px;' align='center'>
            <tr>
                <td style='padding: 10px; border-bottom: 1px solid #ccc; height: 40px;'>
                    <DIV style='font-weight:bold; font-family:Arial; color: #666;'>PERSONAL INFORMATION</DIV></td>
            </tr>
            <tr>
                <td style='padding: 5px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top; height:250px;'>

                    <table style='width:100%;'>
                        <tr>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top; width: 100px; font-weight:bold;'>First Name</td>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top;'>{0}</td>
                            <td style='padding: 0px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top; height: 240px; width: 320px;' rowspan='5'>
                                {4} <!-- image --></td>
                        </tr>
                        <tr>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top; font-weight:bold;'>Last Name</td>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top;'>{1}</td>
                        </tr>
                        <tr>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top; font-weight:bold;'>Email</td>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top;'>{2}</td>
                        </tr>
                        <tr>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top; font-weight:bold;'>Mobile Number</td>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top;'>{3}</td>
                        </tr>
                        <tr>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top; font-weight:bold;'>Center Name</td>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top;'>{7}</td>
                        </tr>
                        <tr>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top; font-weight:bold;'>Location</td>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top;'>{8}</td>
                        </tr>
                        <tr>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top;'>&nbsp;</td>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top;'>&nbsp;</td>
                        </tr>
                        </table>
                </td>
            </tr>

            <tr>
                <td style='padding: 10px; font-family: Arial; color: #666; border-bottom: 1px solid #ccc; vertical-align: top; height: 40px;'>
                     <DIV style='font-weight:bold; font-family:Arial;'>TEST DETAILS</DIV></td>
            </tr>

            <tr>
                <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top;'>

                    <table style='width:100%;'>
                        {5}
                        <tr>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top; font-weight:bold; border-bottom:1px solid #f0f0f0;'>Aggregate</td>
                             <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top; font-weight:bold; border-bottom:1px solid #f0f0f0; width:50px;'>{9}</td>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top; font-weight:bold; border-bottom:1px solid #f0f0f0; width:50px;'>{6}</td>
                        </tr>
                        <tr>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top;'>&nbsp;</td>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top;'>&nbsp;</td>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top;'>&nbsp;</td>
                        </tr>
                        </table>
                </td>
            </tr>
            </table>
        

    </div>
</body>
</html>"; 
            #endregion

            int id = Int32.Parse(hidId.Value);


            var rangeSelectedStr = string.Empty;


            var list = new AssessmentResultService().GetMCQResultsForDownload(id);

            if (listDownloadRange.Visible)
            {
                int skip = Int32.Parse(listDownloadRange.SelectedValue);
                rangeSelectedStr = listDownloadRange.SelectedItem.Text.Replace(" ", string.Empty).Replace("-", "_");

                list = list.OrderBy(x => x.CenterName).Skip(skip).Take(500).ToList();
            }
            else
            {
                list = list.OrderBy(x => x.CenterName).ToList();
            }





            var documents = new Doc();
            documents.Rect.Inset(15, 15);



            documents.HtmlOptions.Engine = EngineType.Gecko;


            var memory = new MemoryStream();


            foreach (var item in list)
            {
                var campaignFolder = UrlMapper.RootPhotosDirectory + item.CampaignId.ToString();

                var imageUrl = Path.Combine(campaignFolder, string.Format("{0}_{1}.jpg", item.CandidateId, item.CampaignEntryId));
                var imageHtml = "<img src='data:image/jpg;base64,{0}'/>";

                if (File.Exists(Server.MapPath(imageUrl)))
                {
                    byte[] ImageData = File.ReadAllBytes(Server.MapPath(imageUrl));
                    var base64Str = Convert.ToBase64String(ImageData);
                    imageHtml = string.Format(imageHtml, base64Str);
                }
                else
                {
                    imageHtml = string.Empty;
                }


                var scoresHtml = GetHtmlScores(item.ResultList.ToList());

                var overallScore = (Convert.ToDouble(item.OverallScore) / item.OverallTotalQuestions) * 100.00;

                string htmlStr = string.Format(pageHtmlStr, item.Firstname, item.Lastname, item.Email, item.MobileNo,
                                          imageHtml, scoresHtml, overallScore.ToString("#,##0.##") + "%", item.CenterName, item.Locaton, item.OverallScore.Value);

                documents.Page = documents.AddPage();


                documents.AddImageHtml(htmlStr);
            }

            documents.Save(memory);

            memory.Position = 0;

            Response.Clear();

            Response.Buffer = true;


            Response.ContentType = "application/pdf";


            Response.AddHeader("content-disposition", "attachment;filename=Candidate_Profiles_" + DateTime.Today.ToString("dd-MMM-yyyy") + "_" + rangeSelectedStr + ".pdf");


            Response.AddHeader("content-length", memory.Length.ToString());

            Response.BinaryWrite(memory.ToArray());

            documents.Dispose();

            memory.Dispose();

            Response.Flush();



        }


        public string GetHtmlScores(List<ResultDownloadViewModel> resultList)
        {
            var sb = new StringBuilder();

            var htmlFragment = @"<tr>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top; border-bottom:1px solid #f0f0f0;'>{0}</td>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top; width:50px; border-bottom:1px solid #f0f0f0;'>{1}</td>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top; width:50px; border-bottom:1px solid #f0f0f0;'>{2}%</td>
                        </tr>";

            foreach (var item in resultList)
            {

                var scorePercent = ((Convert.ToDouble(item.TestScore.Value) / item.TotalQuestions)) * 100.00;

                sb.Append(string.Format(htmlFragment, item.AssessmentName, item.TestScore.Value, scorePercent.ToString("#,##0.##")));
            }


            return sb.ToString();

        }

        public Dictionary<string, string> GetList(int total, int max)
        {

            var list = new Dictionary<string, string>();

            if (total <= max)
            {
                list.Add(string.Format("{0}", "0"), string.Format("{0} - {1}", "1", total.ToString()));

                return list;
            }

            for (int i = 0; i < total; i += max)
            {

                var upperLimit = (i + max) > total ? total : i + max;

                list.Add(string.Format("{0}", (i).ToString()), string.Format("{0} - {1}", (i + 1).ToString(), upperLimit.ToString()));
            }

            return list;
        }


        protected void bttnDownloadTopic_Click(object sender, EventArgs e)
        {
            DoDownloadTopic();
        }

        protected void bttnSearch_Click(object sender, EventArgs e)
        {
            RadListView1.DataBind();
        }
      
    }
}