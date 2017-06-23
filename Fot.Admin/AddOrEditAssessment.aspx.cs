using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web.UI;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;
using Telerik.Web.UI;

namespace Fot.Admin
{
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Admin)]
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Author)]
    public partial class AddOrEditAssessment : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = 0;


                if (Int32.TryParse(Request.QueryString["id"], out id))
                {
                    LoadAssessment(id);
                }

                
            }
        }

        private void LoadAssessment(int id)
        {
            Assessment item = new AssessmentService().GetAssessment(id);

            if (item != null)
            {

               

                editor.Content = item.InstructionText;
                txtAssessmentName.Text = item.Name;
                chkTimed.Checked = item.Timed;

                if (item.Timed)
                {
                    txtDuration.Text = item.Duration.ToString();
                }
                listType.SelectedValue = ((int)item.AssessmentType).ToString();
                chkRandomizeQuestions.Checked = item.RandomizeQuestions;
                chkRandomizeOptions.Checked = item.RandomizeOptions;

                listRetrievalOptions.SelectedValue = item.AdvancedOutputOptions ? "Advanced" : "Simple";

                txtQuestionsPerTest.Text = item.QuestionsPerTest > 0 ? item.QuestionsPerTest.ToString() : string.Empty;

                chkShowCalculator.Checked = item.ShowCalculator;


                hidId.Value = id.ToString();
                bttnAdd.Visible = false;
                bttnUpdate.Visible = true;

                trRandonmizationOptions.Visible = trRetrievalOptions.Visible = trQuestionsPerTest.Visible = true;


                divTopics.Visible = true;

                bttnQuestions.Visible = true;
                bttnQuestions.Text = item.AssessmentType == AssessmentType.MCQ ? "Questions" : "Essay Topics";

                chkFixedQuestions.Checked = item.HasFixedQuestions;


                if (item.AuthorId.HasValue)
                {
                    listAuthor.SelectedValue = item.AuthorId.ToString();
                }


            }
            else
            {
                Response.Redirect(UrlMapper.Assessments);
            }
        }

        public List<AssessmentAuthor> GetAuthors()
        {
            var ctx = new ServiceBase().Context;

            return ctx.AssessmentAuthors.ToList();
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument.Equals("Rebind"))
            {
                topicsGrid.DataBind();
                difficultyLevelGrid.DataBind();
            }
        }

        protected void bttnAdd_Click(object sender, EventArgs e)
        {
            AddAssessment();
        }

        private void AddAssessment()
        {

            string content = editor.Content;
            content = Utilities.FormatHtmlContent(content);


            string html = @"<table width='950px' border='0' cellpadding='1' cellspacing='0'> <tr><td>" + content + @"</td></tr></table>";

            byte[] htmlImage = new Html2ImageBinary(html).GetImage();

            var item = new Assessment
                {
                    Name = txtAssessmentName.Text,
                    Timed = chkTimed.Checked,
                    Duration = chkTimed.Checked ? Int32.Parse(txtDuration.Text) : 0,
                    InstructionText = editor.Content,
                    InstructionImage = htmlImage,
                    AssessmentType = (AssessmentType)Int32.Parse(listType.SelectedValue),
                    RandomizeQuestions = true,
                    RandomizeOptions = true,
                    AdvancedOutputOptions = false,
                    QuestionsPerTest = 0,
                    DateAdded = DateTime.Today,
                    DateLastUpdated = DateTime.Today,
                    ShowCalculator = chkShowCalculator.Checked
                };

            if (!string.IsNullOrWhiteSpace(listAuthor.SelectedValue))
            {
                item.AuthorId = Int32.Parse(listAuthor.SelectedValue);
            }

            var service = new AssessmentService();

            AppMessage app = service.Add(item);

            if (app.IsDone)
            {
                var assessmentId = (int) app.Data;

                LoadAssessment(assessmentId);

                var admin = new AdminUserService().GetCurrentAdmin();

                new AccessLogService().LogEntry(new AccessLog { AdminId = admin.AdminId, LogEntryType = "Added an assessment", LogEntryDetails = "User added a new assessment [" + item.Name + "]", LogDate = DateTime.Now, IpAddress = Request.UserHostAddress, UserAgent = Request.UserAgent });

            }

            lblStatus.ShowMessage(app);
        }

        protected void bttnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAssessment();
        }

        private void UpdateAssessment()
        {
            var service = new AssessmentService();

            Assessment item = service.GetAssessment(Int32.Parse(hidId.Value));


           



            if (item != null)
            {

                var initialQuestionPerTest = item.QuestionsPerTest;

                if (item.InstructionText != editor.Content)
                {
                    string content = editor.Content;
                    content = Utilities.FormatHtmlContent(content);


                    string html = @"<table width='950px' border='0' cellpadding='1' cellspacing='0'> <tr><td>" + content + @"</td></tr></table>";

                    byte[] htmlImage = new Html2ImageBinary(html).GetImage();

                    item.InstructionImage = htmlImage;

                    item.InstructionText = editor.Content;
                }


                item.Name = txtAssessmentName.Text;


                if (!string.IsNullOrWhiteSpace(listAuthor.SelectedValue))
                {
                    item.AuthorId = Int32.Parse(listAuthor.SelectedValue);
                }


                item.Timed = chkTimed.Checked;
                item.Duration = chkTimed.Checked ? Int32.Parse(txtDuration.Text) : 0;
                item.AssessmentType = (AssessmentType)Int32.Parse(listType.SelectedValue);
                item.RandomizeQuestions = chkRandomizeQuestions.Checked;
                item.RandomizeOptions = chkRandomizeOptions.Checked;
                item.ShowCalculator = chkShowCalculator.Checked;
                item.AdvancedOutputOptions = listRetrievalOptions.SelectedValue.Equals("Advanced");
                if (listRetrievalOptions.SelectedValue.Equals("Simple"))
                {
                    if (string.IsNullOrWhiteSpace(txtQuestionsPerTest.Text))
                    {
                        item.QuestionsPerTest = 0;
                    }
                    else if (QuestionPerTestIsValid(item.AssessmentId, Int32.Parse(txtQuestionsPerTest.Text)))
                    {
                        item.QuestionsPerTest = Int32.Parse(txtQuestionsPerTest.Text);
                    }
                    else
                    {
                        lblStatus.ShowMessage(new AppMessage
                            {
                                IsDone = false,
                                Message =
                                    "The value for <strong>Questions per test</strong> seems to be invalid. Ensure that the value is less than or equal to the total number of questions",
                                Status = MessageStatus.Error
                            });

                        return;
                    }


                }

                else if (listRetrievalOptions.SelectedValue.Equals("Advanced"))
                {
                    item.QuestionsPerTest = GetQuestionsPerTestFromOutputConfig(item.AssessmentId);

                }

                var ctx = service.Context;

                var numQuestions = string.IsNullOrWhiteSpace(txtQuestionsPerTest.Text) ? 0 : Int32.Parse(txtQuestionsPerTest.Text);

                if (chkFixedQuestions.Checked && item.HasFixedQuestions == false)
                {
                    //add questions

                    if (numQuestions > 0)
                    {
                        var list =
                            ctx.AssessmentQuestions.Where(x => x.AssessmentId == item.AssessmentId)
                                .Select(x => x.QuestionId)
                                .ToList();

                        list = list.OrderBy(x => Guid.NewGuid().ToString()).Take(numQuestions).ToList();

                        var fixedList = new List<FixedQuestion>();

                        list.ForEach(x => fixedList.Add(new FixedQuestion{AssessmentId = item.AssessmentId, QuestionId = x}));


                        ctx.FixedQuestions.AddRange(fixedList);
                    }




                    item.AdvancedOutputOptions = false;
                    item.HasFixedQuestions = true;
                }
                else if(chkFixedQuestions.Checked == false && item.HasFixedQuestions)
                {
                    ctx.Database.ExecuteSqlCommand("delete from FixedQuestion where AssessmentId = {0}",
                           item.AssessmentId); //delete

                    item.HasFixedQuestions = false;
                }
                else if (chkFixedQuestions.Checked && item.HasFixedQuestions)
                {
                    if (initialQuestionPerTest != numQuestions)
                    {

                        ctx.Database.ExecuteSqlCommand("delete from FixedQuestion where AssessmentId = {0}",
                            item.AssessmentId); //delete


                        if (numQuestions > 0)
                        {
                            //add questions

                            var list =
                            ctx.AssessmentQuestions.Where(x => x.AssessmentId == item.AssessmentId)
                                .Select(x => x.QuestionId)
                                .ToList();

                            list = list.OrderBy(x => Guid.NewGuid().ToString()).Take(numQuestions).ToList();

                            var fixedList = new List<FixedQuestion>();

                            list.ForEach(x => fixedList.Add(new FixedQuestion { AssessmentId = item.AssessmentId, QuestionId = x }));


                            ctx.FixedQuestions.AddRange(fixedList);
                        }
                    }
                }


                var app = service.Update(item);

                if(app.IsDone)
                {
                    LoadAssessment(Int32.Parse(hidId.Value));

                    var admin = new AdminUserService().GetCurrentAdmin();

                    new AccessLogService().LogEntry(new AccessLog { AdminId = admin.AdminId, LogEntryType = "Updated an assessment", LogEntryDetails = "User updated an assessment [" + item.Name + "]", LogDate = DateTime.Now, IpAddress = Request.UserHostAddress, UserAgent = Request.UserAgent });

                }

                lblStatus.ShowMessage(app);
            }
        }

        private int GetQuestionsPerTestFromOutputConfig(int assessmentId)
        {
            var configService = new AssessmentOutputConfigService();

            var num = configService.GetQuestionCountByAssessment(assessmentId);

            return num.HasValue ? num.Value : 0;

        }

        private bool QuestionPerTestIsValid(int assessmentId, int questionsPerTest)
        {
            var questionService = new AssessmentQuestionService();

            return questionService.QuestionCountByAssessment(assessmentId, 0) >= questionsPerTest;

        }

        protected void bttnConfigure_Click(object sender, EventArgs e)
        {
            UpdateAssessment();

            Response.Redirect(UrlMapper.AssessmentConfig + "?id=" + hidId.Value);
        }

        protected void bttnQuestions_Click(object sender, EventArgs e)
        {
            var service = new AssessmentService();
            Assessment item = service.GetAssessment(Int32.Parse(hidId.Value));

            if(item != null)
            {
                if(item.AssessmentType == AssessmentType.MCQ)
                {
                    Response.Redirect(UrlMapper.Questions + "?id=" + hidId.Value);
                }
                else
                {
                    Response.Redirect(UrlMapper.EssayTopics + "?id=" + hidId.Value);
                }
            }
        }

       
    }
}