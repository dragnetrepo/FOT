using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Context;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;
using WebSupergoo.ABCpdf8;

namespace Fot.Admin
{
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Admin)]
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Result)]
    public partial class ResultReview : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if(!Page.IsPostBack)
            {
                if (!Page.IsPostBack)
                {
                    int id = 0;


                    if (Int32.TryParse(Request.QueryString["id"], out id))
                    {
                        LoadInitial(id);
                    }
                    else
                    {
                        Response.Redirect(UrlMapper.ManageCampaigns);
                    }
                }
            }



        }

        private void LoadInitial(int id)
        {
            var item = new AssessmentResultService().GetResultEntry(id);

            if(item != null)
            {
                var options = item.CandidateOptions;

                List<ResultReviewModel> results = ResultReviewModel.ToResultReviewList(options, item.AssessmentId);
                int currentIndex = 0;
               
                Session["results"] = results;


                ShowQuestions(currentIndex);

               

                Session["currentIndex"] = currentIndex;


                hidId.Value = item.CampaignEntry.CampaignId.ToString();

                lblCandidateName.Text = item.CampaignEntry.Candidate.FirstName + " " + item.CampaignEntry.Candidate.LastName;

                lblAssessmentName.Text = item.Assessment.Name;

             

            }
            else
            {
                Response.Redirect(UrlMapper.ManageCampaigns);
            }

        }

        private void ShowQuestions(int index)
        {
            var resultOptions = Session["results"] as List<ResultReviewModel>;

            if (resultOptions != null)
            {
                bttnPrevious.Visible = index > 0;


                lblQuestionCount.Text = "Question " + (index + 1) + " of " + resultOptions.Count;


                bttnNext.Visible = index < resultOptions.Count - 1;

                
                
                    var item = resultOptions[index];

                    imgQuestion.Src = "ImageHandler.ashx?t=q&id=" + item.QuestionId;

                    bool correct = IsCorrect(item.QuestionId, item.Options);


                    lblOptions.Text = GetOptions(item.QuestionId, item.Options, correct);

                    lblRightOrWrong.Text = correct ? "<h2 style='color:green;font-weight:bold;'>The correct Answer Option was selected!</h2>" : "<h2 style='color:#ff0000;font-weight:bold;'>An incorrect Answer Option was selected!</h2>";

                    if(item.Options.Count == 1 && item.Options[0] == 0)
                    {
                        lblRightOrWrong.Text =  "<h2 style='color:#ff0000;font-weight:bold;'>No Answer Option was selected!</h2>";

                    }

                
            }


        }


        public bool IsCorrect(int QuestionId, List<int> optionsChosen)
        {

         

            var realOptions = new AssessmentAnswerService().GetAnswers(QuestionId).Where(x => x.IsCorrect).Select(x => x.AnswerId).ToList();

            

            return optionsChosen.OrderBy(x => x).SequenceEqual(realOptions.OrderBy(x => x));

    
        }


        public string GetOptions(int QuestionId, List<int> optionsChosen, bool correct)
        {
            var str = @"  <div class='divOption' style='{1}'>
                        <div style='float: left; margin-right: 10px; margin-top: 5px; min-width: 20px;'>
                           {0}
                        </div>
                        <div style='float: left; margin-right: 20px; margin-top: 5px; '>
                           {2}
                        </div>
                    </div>";


            var realOptions = new AssessmentAnswerService().GetAnswers(QuestionId);



            var sb = new StringBuilder();



            foreach (var item in realOptions)
            {
                var param0 = item.IsCorrect ? "<img src='images/accept.png'>" : string.Empty;
                var param1 = string.Empty;
                if (correct)
                {
                    param1 = optionsChosen.Any(x => x == item.AnswerId) ? "border: 2px solid green;" : string.Empty;
                }
                else
                {
                    param1 = optionsChosen.Any(x => x == item.AnswerId) ? "border: 2px solid #ff0000;" : string.Empty;
                }
                var param2 = item.IsImage ? "<img src='ImageHandler.ashx?t=a&id=" + item.AnswerId +"' style='height: 70px; vertical-align: top;'>" : item.AnswerText ;

                var html = String.Format(str, param0, param1, param2);

                sb.Append(html);

            }

            return sb.ToString();


        }

        protected void bttnNext_Click(object sender, EventArgs e)
        {
            int index = Int32.Parse(Session["currentIndex"].ToString());

            index++;
            Session["currentIndex"] = index;
            ShowQuestions(index);

            
        }

        protected void bttnPrevious_Click(object sender, EventArgs e)
        {
            int index = Int32.Parse(Session["currentIndex"].ToString());
            index--;
            Session["currentIndex"] = index;
            ShowQuestions(index);

            
        }

        protected void bttnBackToResults_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlMapper.Results + "?id=" + hidId.Value);
        }

        protected void bttnDownloadPDF_Click(object sender, EventArgs e)
        {
            DownloadReviewPDF();
        }

        public bool IsCorrectForPDF(int QuestionId, List<int> optionsChosen, List<AssessmentAnswer> realOptions )
        {



            var realOptionsId = realOptions.Where(x => x.IsCorrect).Select(x => x.AnswerId).ToList();



            return optionsChosen.OrderBy(x => x).SequenceEqual(realOptionsId.OrderBy(x => x));


        }

        public string GetOptionsForPDF(int QuestionId, List<int> optionsChosen, bool correct, List<AssessmentAnswer> realOptions )
        {
            
            var str = @"  <div class='divOption' style='{1}'>
                        <div style='float: left; margin-right: 10px; margin-top: 5px; min-width: 20px;'>
                           {0}
                        </div>
                        <div style='float: left; margin-right: 20px; margin-top: 5px; '>
                           {2}
                        </div>
                    </div>";


            var sb = new StringBuilder();


            int index = 0;
            foreach (var item in realOptions)
            {
                var acceptImgStr = Convert.ToBase64String(File.ReadAllBytes(Server.MapPath("~/images/accept.png")));

                var param0 = item.IsCorrect ? "<img src='data:image/png;base64,"+ acceptImgStr +"'>" : string.Empty;
                var param1 = string.Empty;
                if (correct)
                {
                    param1 = optionsChosen.Any(x => x == item.AnswerId) ? "border: 2px solid green;" : string.Empty;
                }
                else
                {
                    param1 = optionsChosen.Any(x => x == item.AnswerId) ? "border: 2px solid #ff0000;" : string.Empty;
                }
                var param2 = item.IsImage ? "<img src='data:image/png;base64,"+ Convert.ToBase64String(item.AnswerImage) +"' style='height: 70px; vertical-align: top;'>" : item.AnswerText;

                var html = String.Format(str, param0, param1, param2);

                sb.Append(html);

            }

            return sb.ToString();


        }

        private void DownloadReviewPDF()
        {
            #region htmlPageStr

            var headerHtmlStr = @"<!DOCTYPE html>
<html>
<head>
    <title></title>

    <style type='text/css'>
        .divOption
        {
            margin: 0px;
            padding: 5px;
            border: 1px solid #ccc;
            /*border-top: none*/;
            height: 70px;
            clear: both;
        }

        html, body
        {
            font: 14px 'HelveticaNeue', 'Helvetica Neue', Helvetica, Arial, sans-serif;
            color: #444;
        }
    </style>

</head>";

var htmlPageStr = @"
<body>

    <div style='padding: 10px; border: 1px solid #f0f0f0; margin-top: 40px; margin-left: 50px; width: 1040px;'>

        <table style='width: 1020px;'>
            <tr>
                <td style='font-weight: 700'>
                    <div>
                        <div style='float: left'>{0} <!-- Question x of y --></div>
                        <div style='float: right'>{1} <!--No Answer Was Chosen --></div>
                    </div>
                </td>
            </tr>
            <tr>
                <td style='height: 250px;'>

                    <div style='height: 250px; width: 1020px; padding: 3px; overflow: hidden; border: 1px solid #f0f0f0;'>
                        {2} <!-- <img id='imgQuestion'  alt='Image Area' /> -->
                    </div>

                </td>
            </tr>
            <tr>
                <td style='height: 50px;'>{3} <!-- Additional Text -->
                </td>
            </tr>
            <tr>
                <td style='height: 300px; vertical-align: top; padding: 5px;'> {4} <!-- options area --></td>
            </tr>

            <tr>
                <td style='height: 50px;' align='center'>&nbsp;
                </td>
            </tr>
        </table>
    </div>

</body>
</html>"; 
            #endregion

            var resultOptions = Session["results"] as List<ResultReviewModel>;

            if (resultOptions != null)
            {

                var questionList = new AssessmentQuestionService().GetQuestionsAndOptions(resultOptions[0].AssessmentId);

                var documents = new Doc();
                documents.Rect.Inset(15, 15);
                documents.HtmlOptions.Engine = EngineType.Gecko;


                var memory = new MemoryStream();


                for (int index = 0; index < resultOptions.Count; index++)
                {


                     var item = resultOptions[index];


                    var currentQuestionText = "Question " + (index + 1) + " of " + resultOptions.Count;



                    var currentQuestion = questionList.First(x => x.QuestionId == item.QuestionId);


                    var imgHtmlStr = "<img src='data:image/png;base64," + Convert.ToBase64String(currentQuestion.QuestionImage) + "'>";



                    var additionalStr = string.IsNullOrWhiteSpace(currentQuestion.AdditionalText)? string.Empty : currentQuestion.AdditionalText;

                    bool correct = IsCorrectForPDF(item.QuestionId, item.Options, currentQuestion.AssessmentAnswers.ToList());
                    

                    var optionsHtmlStr = GetOptionsForPDF(item.QuestionId, item.Options, correct, currentQuestion.AssessmentAnswers.ToList());

                    var lblRightOrWrongHtmlStr = correct
                                               ? "<h2 style='color:green;font-weight:bold;'>The correct Answer Option was selected!</h2>"
                                               : "<h2 style='color:#ff0000;font-weight:bold;'>An incorrect Answer Option was selected!</h2>";

                    if (item.Options.Count == 1 && item.Options[0] == 0)
                    {
                        lblRightOrWrongHtmlStr =
                            "<h2 style='color:#ff0000;font-weight:bold;'>No Answer Option was selected!</h2>";

                    }


                    var htmlStr = string.Format(htmlPageStr, currentQuestionText, lblRightOrWrongHtmlStr, imgHtmlStr,
                                                additionalStr, optionsHtmlStr);


                    documents.Page = documents.AddPage();


                    documents.AddImageHtml(string.Concat(headerHtmlStr,htmlStr));

                }


                documents.Save(memory);

                memory.Position = 0;

                Response.Clear();

                Response.Buffer = true;


                Response.ContentType = "application/pdf";


                Response.AddHeader("content-disposition", "attachment;filename=Result_Review_" + DateTime.Today.ToString("dd-MMM-yyyy") + ".pdf");


                Response.AddHeader("content-length", memory.Length.ToString());

                Response.BinaryWrite(memory.ToArray());

                documents.Dispose();

                memory.Dispose();

                Response.Flush();


            }

        }
    }
}