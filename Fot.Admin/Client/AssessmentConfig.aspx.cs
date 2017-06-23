using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;

namespace Fot.Admin.Client
{
    
    public partial class AssessmentConfig : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = 0;


                if (Int32.TryParse(Request.QueryString["id"], out id))
                {
                    LoadDefault(id);
                }
                else
                {
                    Response.Redirect(UrlMapper.Assessments);
                }
            }
        }

        private void LoadDefault(int id)
        {
            var item = new AssessmentService().GetAssessment(id);

            if(item != null)
            {
                hidId.Value = id.ToString();

                if(!item.AdvancedOutputOptions)
                {
                    Response.Redirect(UrlMapper.AddOrEditAssessment + "?id=" + hidId.Value);
                }

                lblAssessmentName.Text = item.Name;
                lblQuestionsPerTest.Text = item.QuestionsPerTest.ToString();
                hidId.Value = id.ToString();

                var list = new AssessmentTopicService().GetTopics(id, -1, -1);
                listTopics.DataSource = list;
                listTopics.DataBind();

                var list2 = new QuestionDifficultyLevelService().GetLevels(id, -1, -1);
                listLevels.DataSource = list2;
                listLevels.DataBind();


               CalculateAvailableTotal();

            }
            else
            {
                Response.Redirect(UrlMapper.Assessments);
            }
        }


        public void CalculateAvailableTotal()
        {
            var questionService = new AssessmentQuestionService();

            int? topicId =  listTopics.SelectedValue.Equals("0") ? default(int?) : Int32.Parse(listTopics.SelectedValue);

            int? levelId = listLevels.SelectedValue.Equals("0") ? default(int?) : Int32.Parse(listLevels.SelectedValue);

            int assessmentId = Int32.Parse(hidId.Value);

            int totalAvailable = questionService.GetTotalCountForTopicAndLevel(assessmentId, topicId, levelId);

            lblTotalQuestionsPossible.Text = totalAvailable.ToString("#,##0");


        }

        protected void listLevels_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateAvailableTotal();
        }

        protected void listTopics_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateAvailableTotal();
        }

        protected void bttnAdd_Click(object sender, EventArgs e)
        {
            AddEntry();
        }

        private void AddEntry()
        {
            var item = new AssessmentOutputConfig
                {
                    AssessmentId = Int32.Parse(hidId.Value),
                    TopicId = listTopics.SelectedValue.Equals("0") ? default(int?) : Int32.Parse(listTopics.SelectedValue),
                    DifficultyLevel = listLevels.SelectedValue.Equals("0") ? default(int?) : Int32.Parse(listLevels.SelectedValue),
                    NumQuestions = Int32.Parse(txtNumQuestions.Text)
                    
                };

            var configService = new AssessmentOutputConfigService();

            var app = configService.Add(item);

            lblStatus.ShowMessage(app);

            if (app.IsDone)
            {
                RadGrid1.DataBind();
                var ret = configService.GetQuestionCountByAssessment(item.AssessmentId);

                lblQuestionsPerTest.Text = ret.HasValue ? ret.Value.ToString() : "0";

            }

        }

        protected void bttnBackToDetails_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlMapper.AddOrEditAssessment + "?id=" + hidId.Value);
        }

        protected void RadGrid1_ItemDeleted(object sender, Telerik.Web.UI.GridDeletedEventArgs e)
        {
            var configService = new AssessmentOutputConfigService();
            var ret = configService.GetQuestionCountByAssessment(Int32.Parse(hidId.Value));

            lblQuestionsPerTest.Text = ret.HasValue ? ret.Value.ToString() : "0";
        }
    }
}