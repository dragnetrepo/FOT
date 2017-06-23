using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;

namespace Fot.Admin.Client
{
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Users)]
    public partial class Reports : System.Web.UI.Page
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
            var item = new PartnerCampaignService().GetCampaign(id);

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



     


        public string GetOptions(int QuestionId)
        {
            var str = @"  <div class='divOption'>
                        <div style='float: left; margin-right: 10px; margin-top: 5px; min-width: 20px;'>
                           {0}
                        </div>
                        <div style='float: left; margin-right: 20px; margin-top: 5px; '>
                           {1}
                        </div>
 <div style='float: right; margin-right: 10px; margin-top: 5px; min-width: 20px;'>
                           {2}
                        </div>
                    </div>";

            var header = @" <div class='divOption' style='border-top: 1px solid #ccc'>
                        <div style='float: left; margin-right: 10px; margin-top: 5px; min-width: 20px;'>
                           &nbsp;
                        </div>
                        <div style='float: left; margin-right: 20px; margin-top: 5px; '>
                           <strong>Option</strong>
                        </div>
 <div style='float: right; margin-right: 10px; margin-top: 5px; min-width: 20px;'>
                          <strong>Count</strong>
                        </div>
                    </div>";

            var footer = @" <div class='divOption'>
                        <div style='float: left; margin-right: 10px; margin-top: 5px; min-width: 20px;'>
                           &nbsp;
                        </div>
                        <div style='float: left; margin-right: 20px; margin-top: 5px; '>
                           <strong>Correct Answer</strong>
                        </div>
 <div style='float: right; margin-right: 10px; margin-top: 5px; min-width: 20px;'>
                          <strong>{0}</strong>
                        </div>
                    </div>";


            var realOptions = new PartnerAssessmentAnswerService().GetAnswers(QuestionId);

            var answerList = new ShownQuestionService().GetChosenOptions(Int32.Parse(hidId.Value), QuestionId);


            var sb = new StringBuilder();

            sb.Append(header);


            foreach (var item in realOptions)
            {
                var param0 = item.IsCorrect ? "<img src='../images/accept.png'>" : string.Empty;
               
         
                var param1 = item.IsImage ? "<img src='ImageHandler.ashx?t=a&id=" + item.AnswerId + "' style='height: 70px; vertical-align: top;'>" : item.AnswerText;

                var paramCount = answerList.Count(x => x.AnswerId == item.AnswerId);

                var html = String.Format(str, param0, param1, paramCount.ToString("#,##0"));

                sb.Append(html);

            }

            var correctOptions = realOptions.Where(x => x.IsCorrect).ToList();

            var totalCorrect = 0;

            if (correctOptions.Count == 1)
            {
                totalCorrect = answerList.Count(x => x.AnswerId == correctOptions[0].AnswerId);
            }
            else
            {
                var questionEntryIdList = answerList.Select(x => x.ShowQuestionEntryId).Distinct().ToList();

                var correctList = correctOptions.Select(x => x.AnswerId).ToList();
                

                foreach (var i in questionEntryIdList)
                {
                    var tempI = i;
                    var tempAnswersList =
                        answerList.Where(x => x.ShowQuestionEntryId == tempI).Select(x => x.AnswerId.Value).ToList();

                   if (Enumerable.SequenceEqual(correctList.OrderBy(x => x), tempAnswersList.OrderBy(x => x)))
                   {
                       totalCorrect++;
                   }


                }

            }


            sb.Append(string.Format(footer, totalCorrect.ToString("#,##0")));

            return sb.ToString();


        }

        private void ShowQuestion(int index)
        {
            var resultOptions = Session["campaign_questions"] as List<AssessmentQuestion>;

            if (resultOptions != null)
            {
                bttnPrevious.Visible = index > 0;


                lblQuestionCount.Text = "Question " + (index + 1) + " of " + resultOptions.Count;


                bttnNext.Visible = index < resultOptions.Count - 1;



                var item = resultOptions[index];

                imgQuestion.Src = "ImageHandler.ashx?t=q&id=" + item.QuestionId;

            


                lblOptions.Text = GetOptions(item.QuestionId);

                lblQuestionShownCount.Text = new ShownQuestionService().QuestionShownCount(Int32.Parse(hidId.Value),
                                                                                           item.QuestionId)
                                                                       .ToString("#,##0");

                if (item.TopicId.HasValue)
                {
                    lblTopic.Text = item.AssessmentTopic.Topic;
                    trTopic.Visible = true;
                }
                else
                {
                    trTopic.Visible = false;
                }

                if (item.DifficultyLevel.HasValue)
                {
                    lblDifficultyLevel.Text = item.QuestionDifficultyLevel.LevelName;
                    trLevel.Visible = true;
                }
                else
                {
                    trLevel.Visible = false;
                }



            }


        }

        protected void bttnShowDetails_Click(object sender, EventArgs e)
        {
            if(listAssessments.SelectedIndex >= 0) ShowDetails();
        }

        private void ShowDetails()
        {
            int assessmentId = Int32.Parse(listAssessments.SelectedValue);

            var questionList = new AssessmentQuestionService().GetQuestionsForCampaignReports(assessmentId);

            Session["campaign_questions"] = questionList;

            int current_index = 0;
            Session["current_index"] = current_index;

            ShowQuestion(current_index);

            if (questionList.Count > 0)
            {
                divReport.Visible = true;
            }
        }

        protected void bttnNext_Click(object sender, EventArgs e)
        {
            var current_index = (int)Session["current_index"];
            Session["current_index"] = ++current_index;
            ShowQuestion(current_index);

        }

        protected void bttnPrevious_Click(object sender, EventArgs e)
        {
            var current_index = (int)Session["current_index"];
            Session["current_index"] = --current_index;
            ShowQuestion(current_index);
        }
    }
}