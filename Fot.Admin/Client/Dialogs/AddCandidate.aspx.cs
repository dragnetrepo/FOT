using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;
using Fot.Admin.Services;

namespace Fot.Client.Dialogs
{
    public partial class AddCandidate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = 0;


                if (Int32.TryParse(Request.QueryString["id"], out id))
                {
                    LoadForAdd(id);
                }
                else
                {
                    form1.Visible = false;
                }

            }
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


        private void LoadForAdd(int id)
        {
            var item = new CampaignService().GetCampaign(id);

            if (item != null)
            {
               

                hidId.Value = id.ToString();

            }
            else
            {
                form1.Visible = false;
            }
        }

        protected void bttnAdd_Click(object sender, EventArgs e)
        {
            AddToCampaign();
        }

        private int GetTotalPossible()
        {
            var admin = new AdminUserService().GetCurrentAdmin();
            
            var partner = new PartnerService().GetPartner(admin.PartnerId.Value);
            var amountScheduled = new PartnerWalletScheduleService().GetTotalAmountScheduled(admin.PartnerId.Value);

            var amountLeft = partner.WalletBalance - amountScheduled;

            var centerCost = partner.CostPerTestPrivate.Value;

            int totalPossible = (int)(amountLeft / centerCost);
            return totalPossible;
        }

        private void AddToCampaign()
        {
            var campaign = new PartnerCampaignService().GetCampaign(Int32.Parse(hidId.Value));

            if (campaign.IsUnproctored)
            {
                var totalPossible = GetTotalPossible();

                if (totalPossible == 0)
                {
                    lblStatus.ShowMessage(new AppMessage{IsDone = false, Message = "Wallet has to be funded before candidates can be added for an unproctored campaign.", Status = MessageStatus.Error});

                    return;
                    
                }

            }


            var match = Regex.IsMatch(txtEmail.Text, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

            if (!match)
            {
                lblStatus.ShowMessage(new AppMessage { IsDone = false, Message = "Invalid Email", Status = MessageStatus.Error });

                return;

            }


            var mobileNumberValid = Regex.IsMatch(txtMobile.Text.Trim(), @"\d{11}");


            if (!mobileNumberValid)
            {
                lblStatus.ShowMessage(new AppMessage { IsDone = false, Message = "Invalid Mobile Number. Mobile number should be 11 digits (GSM format)", Status = MessageStatus.Error });

                return;

            }


            var candidateService = new CandidateService();
            var entryService = new CampaignEntryService();
            var campaignId = Int32.Parse(hidId.Value);


            var candidate = new Candidate
                {
                    ClientUniqueID = txtClientUniqueId.Text,
                    Email = txtEmail.Text,
                    Password = txtPassword.Text,
                    FirstName = txtFname.Text,
                    LastName = txtLname.Text,
                    MobileNo = txtMobile.Text,
                    LocationId = Int32.Parse(listLocations.SelectedValue),
                    DateAdded = DateTime.Now
                };

            var app = candidateService.Add(candidate);

            if(app.IsDone)
            {
                var entry = new CampaignEntry
                    {
                        CampaignId = campaignId,
                        CandidateId = (int) app.Data
                    };

                if (campaign.IsUnproctored)
                {
                    entry.CandidateAssessment = new CandidateAssessment
                    {
                        CandidateGuid = Guid.NewGuid().ToString().Replace("-", "")
                    };
                }
                
                var tempApp = entryService.Add(entry);

            if(tempApp.IsDone)
            {
                app.Message = app.Message + "<br/>" + tempApp.Message;
            }
            else
            {
                 app.Message = app.Message + "<br/> <strong style='color:red;'>" + tempApp.Message +"</strong>";
            }

                txtEmail.Text = string.Empty;
                txtFname.Text = string.Empty;
                txtLname.Text = string.Empty;
                txtMobile.Text = string.Empty;

                RegisterScript();

                var admin = new AdminUserService().GetCurrentAdmin();

                new AccessLogService().LogEntry(new AccessLog { AdminId = admin.AdminId, LogEntryType = "Candidate add", LogEntryDetails = "User added a candidate [" + candidate.Email + " ( " + candidate.FirstName + " " + candidate.LastName+ " ) " + "]", LogDate = DateTime.Now, IpAddress = Request.UserHostAddress, UserAgent = Request.UserAgent });

                int rows = candidateService.Context.Database.ExecuteSqlCommand(
                   "update Candidate set Username =  'DRG' + REPLACE(STR(CandidateId, 7), SPACE(1), '0') where Username is null");

            }

            lblStatus.ShowMessage(app);
        }
    }
}