using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Client.Models;
using Fot.Client.Services;
using WebSupergoo.ABCpdf8;

namespace Fot.Client
{
    public partial class Results : System.Web.UI.Page
    {
        public string pageHtmlStr = @"<!DOCTYPE html>
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

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public List<ResultsViewModel> GetEntries(int CandidateId)
        {

                var ctx = new CandidateService().Context;


            var items = ctx.CampaignEntries.Where(x => x.Campaign.ViewResult && x.CandidateId == CandidateId && x.Tested)
                  .Select(x => new ResultsViewModel
                  {
                      EntryId = x.EntryId,
                      CampaignName = x.Campaign.CampaignName,
                      AssessmentName = x.Campaign.AssessmentBundle.Name,
                      TestDate = x.DateTested.Value
                  }).ToList();


            return items;

        }

        protected void RadGrid1_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "Download")
            {
                int id = Int32.Parse(e.CommandArgument.ToString());

                DownloadResult(id);

            }
        }

        public void DownloadResult(int EntryId)
        {
            var ctx = new CandidateService().Context;

           var item = ctx.CampaignEntries.Where(x => x.EntryId == EntryId && x.Tested)
                      .Include(x => x.AssessmentResults).
                       Select(x => new AssessmentResultDownloadViewModel
                       {
                           CampaignEntryId = x.EntryId,
                           CampaignId = x.CampaignId,
                           CandidateId = x.Candidate.CandidateId,
                           ClientUniqueId = x.Candidate.ClientUniqueID,
                           Firstname = x.Candidate.FirstName,
                           Lastname = x.Candidate.LastName,
                           Email = x.Candidate.Email,
                           MobileNo = x.Candidate.MobileNo,
                           DateTested = x.DateTested.Value,
                           Locaton =
                                   x.SessionId.HasValue ? x.TestSession.Center.Location.LocationName : string.Empty,
                           CenterName = x.SessionId.HasValue ? x.TestSession.Center.CenterName : string.Empty,
                           ResultList =
                                   x.AssessmentResults.Where(q => q.Assessment.AssessmentType == AssessmentType.MCQ)
                                    .Select(y => new ResultDownloadViewModel
                                    {
                                        EntryId = y.EntryId,
                                        AssessmentId = y.AssessmentId,
                                        AssessmentName = y.Assessment.Name,
                                        TestScore = y.TestScore,
                                        CandidateOptions = y.CandidateOptions
                                    }).OrderBy(y => y.AssessmentId)
                       }).FirstOrDefault();





            var documents = new Doc();
            documents.Rect.Inset(15, 15);



            // documents.HtmlOptions.Engine = EngineType.Gecko;


            var memory = new MemoryStream();

            var photoDir = ConfigurationManager.AppSettings["photoDir"];

                var campaignFolder = photoDir + item.CampaignId.ToString();

                var imageUrl = Path.Combine(campaignFolder, string.Format("{0}_{1}.jpg", item.CandidateId, item.CampaignEntryId));
                var imageHtml = "<img src='data:image/jpg;base64,{0}'/>";

                if (File.Exists(imageUrl))
                {
                    byte[] ImageData = File.ReadAllBytes(imageUrl);
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
            

            documents.Save(memory);

            memory.Position = 0;

            Response.Clear();

            Response.Buffer = true;


            Response.ContentType = "application/pdf";


            Response.AddHeader("content-disposition", "attachment;filename=Candidate_Result_" + DateTime.Today.ToString("dd-MMM-yyyy") + ".pdf");


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
    }


    public class ResultsViewModel
    {
        public int EntryId { get; set; }

        public string CampaignName { get; set; }

        public string AssessmentName { get; set; }

        public DateTime TestDate { get; set; }
    }
}