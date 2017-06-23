using System;
using System.Security.Permissions;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;

namespace Fot.Admin.Client
{

    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Users)]
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Author)]
    public partial class AddOrEditQuestion : Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int qid = 0;
                int aid = 0;

                if (Int32.TryParse(Request.QueryString["aid"], out aid))
                {
                    LoadForAdd(aid);
                }

                else if (Int32.TryParse(Request.QueryString["qid"], out qid))
                {
                    LoadQuestion(qid);
                }
                else
                {
                    Response.Redirect(UrlMapper.Assessments);
                }

                editor.ImageManager.DeletePaths = new string[] {UrlMapper.RootResourcesDirectory + hidAId.Value};
                editor.ImageManager.UploadPaths = new string[] { UrlMapper.RootResourcesDirectory + hidAId.Value };
                editor.ImageManager.ViewPaths = new string[] { UrlMapper.RootResourcesDirectory + hidAId.Value };
            }
        }

        private void LoadQuestion(int qid)
        {
            AssessmentQuestion item = new PartnerAssessmentQuestionService().GetQuestion(qid);

            if (item != null)
            {
                Assessment aItem = new PartnerAssessmentService().GetAssessment(item.AssessmentId);
                hidAId.Value = aItem.AssessmentId.ToString();
                lblAssessmentName.Text = aItem.Name;

                hidQId.Value = qid.ToString();


                editor.Content = item.QuestionText;
                txtAdditionalText.Text = item.AdditionalText;
                listOptionsType.SelectedValue = item.AnswerType;
                listTopic.SelectedValue = item.TopicId.HasValue ? item.TopicId.Value.ToString() : "0";
                listGroup.SelectedValue = item.GroupId.HasValue ? item.GroupId.Value.ToString() : "0";
                listLevel.SelectedValue = item.DifficultyLevel.HasValue ? item.DifficultyLevel.Value.ToString() : "0";

                listOptionsLayout.SelectedValue = item.OptionsLayoutIsVertical ? "1" : "0";

                bttnAdd.Visible = false;
                bttnUpdate.Visible = true;

                divOptions.Visible = true;
            }
            else
            {
                Response.Redirect(UrlMapper.Assessments);
            }
        }

        private void LoadForAdd(int aid)
        {
            Assessment item = new PartnerAssessmentService().GetAssessment(aid);

            if (item != null)
            {
                hidAId.Value = aid.ToString();
                lblAssessmentName.Text = item.Name;
            }
            else
            {
                Response.Redirect(UrlMapper.Assessments);
            }
        }

        protected void bttnBackToQuestions_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlMapper.Questions + "?id=" + hidAId.Value);
        }

        protected void bttnAdd_Click(object sender, EventArgs e)
        {
            AddQuestion();
        }

        private void AddQuestion()
        {
            string content = editor.Content;
            content = Utilities.FormatHtmlContentForQuestion(content);

            string html = @"<table width='950px' border='0' cellpadding='1' cellspacing='0'> <tr><td>" + content +
                          @"</td></tr></table>";

            byte[] htmlImage = new Html2ImageBinary(html).GetImage();

            var item = new AssessmentQuestion
                {
                    AssessmentId = Int32.Parse(hidAId.Value),
                    TopicId = listTopic.SelectedValue.Equals("0") ? default(int?) : Int32.Parse(listTopic.SelectedValue),
                    GroupId = listGroup.SelectedValue.Equals("0") ? default(int?) : Int32.Parse(listGroup.SelectedValue),
                    DifficultyLevel =
                        listLevel.SelectedValue.Equals("0") ? default(int?) : Int32.Parse(listLevel.SelectedValue),
                    AnswerType = listOptionsType.SelectedValue,
                    AdditionalText = txtAdditionalText.Text,
                    QuestionText = editor.Content,
                    QuestionImage = htmlImage,
                    OptionsLayoutIsVertical = listOptionsLayout.SelectedValue.Equals("1")
                };

            AppMessage app = new PartnerAssessmentQuestionService().Add(item);

            if (app.IsDone)
            {
                hidQId.Value = ((int) app.Data).ToString();

                bttnAdd.Visible = false;
                bttnUpdate.Visible = true;

                divOptions.Visible = true;
            }

            lblStatus.ShowMessage(app);
        }

        protected void bttnUpdate_Click(object sender, EventArgs e)
        {
            UpdateQuestion();
        }

        private void UpdateQuestion()
        {
            string content = editor.Content;
            content = Utilities.FormatHtmlContentForQuestion(content);

            string html = @"<table width='950px' border='0' cellpadding='1' cellspacing='0'> <tr><td>" + content +
                          @"</td></tr></table>";

            byte[] htmlImage = new Html2ImageBinary(html).GetImage();

            var questionService = new PartnerAssessmentQuestionService();

            AssessmentQuestion item = questionService.GetQuestion(Int32.Parse(hidQId.Value));

            var formerAnswerType = item.AnswerType;


            if (item != null)
            {
                item.TopicId = listTopic.SelectedValue.Equals("0")
                                   ? default(int?)
                                   : Int32.Parse(listTopic.SelectedValue);
                item.GroupId = listGroup.SelectedValue.Equals("0")
                                   ? default(int?)
                                   : Int32.Parse(listGroup.SelectedValue);
                item.DifficultyLevel = listLevel.SelectedValue.Equals("0")
                                           ? default(int?)
                                           : Int32.Parse(listLevel.SelectedValue);
                item.QuestionText = editor.Content;
                item.QuestionImage = htmlImage;
                item.AdditionalText = txtAdditionalText.Text;
                item.AnswerType = listOptionsType.SelectedValue; //reset options to single if was multiple uncheck  all if more than 1

                item.OptionsLayoutIsVertical = listOptionsLayout.SelectedValue.Equals("1");


                var app = questionService.Update(item);

                if (app.IsDone)
                {

                    if (formerAnswerType.Equals("Multiple") && item.AnswerType.Equals("Single"))
                    {
                        questionService.ResetAllAnswersIfMoreThanOne(item.QuestionId);
                    }
                }

                lblStatus.ShowMessage(app);

                RadListView1.DataBind();
            }
        }

        protected void RadListView1_ItemCommand(object sender, Telerik.Web.UI.RadListViewCommandEventArgs e)
        {
            var service = new PartnerAssessmentAnswerService();

            var id = Int32.Parse(e.CommandArgument.ToString());

            if(e.CommandName.Equals("Set"))
            {
                service.SetAsCorrectAnswer(id, Int32.Parse(hidQId.Value));
            }
            else if(e.CommandName.Equals("Unset"))
            {
                service.UnsetAsCorrectAnswer(id);
            }
            else if(e.CommandName.Equals("Delete"))
            {
                service.Delete(id);
            }

            RadListView1.DataBind();

        }

        protected void RadListView1_ItemDeleting(object sender, Telerik.Web.UI.RadListViewCommandEventArgs e)
        {

        }

        protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {
            if(e.Argument.Equals("Rebind"))
            {
                RadListView1.DataBind();
            }
            else if (e.Argument.Equals("RebindGroups"))
            {
                listGroup.Items.Clear();
                listGroup.Items.Add(new ListItem("Not Specified", "0"));
                listGroup.DataBind();
            }
        }

        protected void bttnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlMapper.AddOrEditQuestion + "?aid=" + hidAId.Value);
        }
    }
}