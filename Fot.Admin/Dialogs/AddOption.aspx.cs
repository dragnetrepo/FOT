using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;

namespace Fot.Admin.Dialogs
{
    public partial class AddOption : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int qid = 0;
               

                if (Int32.TryParse(Request.QueryString["qid"], out qid))
                {
                    LoadForAdd(qid);
                }
                else
                {
                    form1.Visible = false;
                }
              
            }
        }

        private void LoadForAdd(int qid)
        {
            var item = new AssessmentQuestionService().GetQuestion(qid);
            if(item != null)
            {
                hidQId.Value = item.QuestionId.ToString();
            }
            else
            {
                form1.Visible = false;
            }
        }

        protected void bttnAdd_Click(object sender, EventArgs e)
        {
            var index = Int32.Parse(listOptionType.SelectedValue);

            if (index == 1)
            {
                AddAnswerOption();
            }
            else
            {
                UploadAnswerImage();
            }
        }

        private void UploadAnswerImage()
        {
            if (ValidateUpload()) return;

            var answerService = new AssessmentAnswerService();

            var img = System.Drawing.Image.FromStream(fileImage.PostedFile.InputStream);

            var ms = new MemoryStream();

            img.Save(ms, ImageFormat.Png);

            var item = new AssessmentAnswer
                {
                    QuestionId = Int32.Parse(hidQId.Value),
                    AnswerImage = ms.ToArray(),
                    IsImage = true,
                    IsCorrect = chkCorrect.Checked
                };

           var app = answerService.Add(item);

            lblStatus.ShowMessage(app);

            if(app.IsDone)
            {
                chkCorrect.Checked = false;
                

                RegisterScript();

                var admin = new AdminUserService().GetCurrentAdmin();

                new AccessLogService().LogEntry(new AccessLog { AdminId = admin.AdminId, LogEntryType = "Added an option", LogEntryDetails = "User added a new option [" + item.AnswerId + "]", LogDate = DateTime.Now, IpAddress = Request.UserHostAddress, UserAgent = Request.UserAgent });

            }
        }

        private void AddAnswerOption()
        {
            var answerService = new AssessmentAnswerService();

            var item = new AssessmentAnswer
            {
                QuestionId = Int32.Parse(hidQId.Value),
                AnswerText = txtOptionText.Text,
                IsImage = false,
                IsCorrect = chkCorrect.Checked
            };

            var app = answerService.Add(item);

            lblStatus.ShowMessage(app);

            if (app.IsDone)
            {
                chkCorrect.Checked = false;
                txtOptionText.Text = string.Empty;

                RegisterScript();

                var admin = new AdminUserService().GetCurrentAdmin();

                new AccessLogService().LogEntry(new AccessLog { AdminId = admin.AdminId, LogEntryType = "Added an option", LogEntryDetails = "User added a new option [" + item.AnswerId + "]", LogDate = DateTime.Now, IpAddress = Request.UserHostAddress, UserAgent = Request.UserAgent });

            }
          
        }


        private bool ValidateUpload()
        {
            if (!fileImage.HasFile)
            {
                var appStatus = new AppMessage { Message = "No file selected.", Status = MessageStatus.Error, IsDone = false };
                lblStatus.ShowMessage(appStatus);
                return true;
            }

            string fileExt = Path.GetExtension(fileImage.FileName).ToLower();


            if (!(fileExt.Equals(".jpg") || fileExt.Equals(".png")))
            {
                var appStatus = new AppMessage
                {
                    Message =
                        "Only images files with *.jpg or *.png extensions are allowed.",
                    Status = MessageStatus.Error,
                    IsDone = false
                };
                lblStatus.ShowMessage(appStatus);
                return true;
            }



            var img = System.Drawing.Image.FromStream(fileImage.PostedFile.InputStream);

            if (img.Width > 700 || img.Height > 110)
            {
                var appStatus = new AppMessage
                {
                    Message =
                        "Banner image width must not be larger than 700 pixels and height must not be larger than 110 pixels",
                    Status = MessageStatus.Error,
                    IsDone = false
                };
                lblStatus.ShowMessage(appStatus);
                return true;
            }


            return false;
        }

        public void RegisterScript()
        {


            String csname1 = "PopupScript";
            Type cstype = this.GetType();

            // Get a ClientScriptManager reference from the Page class.

            ClientScriptManager cs = Page.ClientScript;

            // Check to see if the startup script is already registered.

            if (!cs.IsStartupScriptRegistered(cstype, csname1))
            {
                StringBuilder cstext1 = new StringBuilder();
                // cstext1.Append("alert('Hello World!');");
                var fnName = "refreshGrid";
                cstext1.Append("CallFunctionOnParentPage('" + fnName + "');");


                cs.RegisterStartupScript(cstype, csname1, cstext1.ToString(), true);
            }

        }

        protected void listOptionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = Int32.Parse(listOptionType.SelectedValue);

            if(index == 1)
            {
                trText.Visible = true;
                trImage.Visible = false;
                trImage2.Visible = false;
                bttnAdd.Text = "Add Option";
            }
            else
            {
                trText.Visible = false;
                trImage.Visible = true;
               
                trImage2.Visible = true;
                bttnAdd.Text = "Upload Image";
            }
        }
    }
}