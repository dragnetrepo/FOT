using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Fot.Lan;
using Fot.Lan.Infrastructure;
using Fot.Lan.Models;
using Fot.Lan.Services;
using Telerik.Web.UI;

namespace Fot.Lan
{
    public partial class Default : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
           // Utilities.CheckSerial();

            if (!Page.IsPostBack)
            {
            }
        }


        protected void bttnLogin_Click(object sender, EventArgs e)
        {
            DoLogin();
        }

        public void DoLogin()
        {
            var candidateService = new CandidateService();
            var item = candidateService.GetCandidateByUsername(txtUsername.Text);


            if (item != null)
            {
                if (item.Password.Equals(txtPassword.Text))
                {
                    if (item.AssessmentCompleted)
                    {
                        lblStatus.ShowMessage(new AppMessage
                            {
                                IsDone = false,
                                Message = "You have already completed the assessment.",
                                Status = MessageStatus.Error
                            });

                        return;
                    }
                    var settingService = new SettingService();

                    if (settingService.GetImageCapatureSetting() && item.CandidatePhoto == null)
                    {
                        lblStatus.ShowMessage(new AppMessage
                            {
                                IsDone = false,
                                Message = "The photo capture process is required before taking the assessment.",
                                Status = MessageStatus.Error
                            });
                        return;
                    }

                    if(candidateService.AssessmentBundleExists(item.BundleId))
                    {
                        lblStatus.Text = string.Empty;

                        string cua = item.CandidateGuid;
                        trLogin.Visible = false;
                        trTakeTest.Visible = true;

                        lblCandidate.Text = item.Firstname + " " + item.Lastname;

                        lblAssessmentName.Text = candidateService.AssessmentName(item.BundleId);

                        // string location = "AppPage.aspx";
                        string location = "Tests/TakeTest/";

                        // bttnTakeTest.Attributes.Add("onclick", "MM_openBrWindow('" + location + "?id=" + cua + "','Assessment','toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,');return false;");
                        bttnTakeTest.Attributes.Add("onclick", "MM_openBrWindow('" + location + cua + "','Assessment','toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,');return false;");
            
                        
                    }
                    else
                    {
                        lblStatus.ShowMessage(new AppMessage { IsDone = false, Message = "The specified assessment has not yet been downloaded. Please alert center administrator.", Status = MessageStatus.Error });
                    }
                }


                else
                {
                    lblStatus.ShowMessage(new AppMessage
                        {IsDone = false, Message = "Invalid username or password", Status = MessageStatus.Error});
                }
            }
            else
            {
                lblStatus.ShowMessage(new AppMessage
                    {IsDone = false, Message = "Invalid username or password", Status = MessageStatus.Error});
            }
        }
    }
}