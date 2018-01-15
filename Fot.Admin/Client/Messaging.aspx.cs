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
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Users)]
    [PrincipalPermission(SecurityAction.Demand, Role = RoleModel.Schedule)]
    public partial class Messaging : System.Web.UI.Page
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
                    Response.Redirect("Campaigns.aspx");
                }


            }
        }

        private void LoadForUpdate(int id)
        {
            var ctx = new ServiceBase().Context;

            var currentAdmin = new AdminUserService().GetCurrentAdmin();

            var item = ctx.Campaigns.FirstOrDefault(x => x.CampaignId == id && x.PartnerId == currentAdmin.PartnerId);


            if (item != null)
            {
                lblCampaignName.Text = item.CampaignName;

                hidId.Value = id.ToString();



                var candidateCountStr = new string[] { "{0} Candidates found", "{0} Candidate found" };
                var totalCandidates = ctx.CampaignEntries.Count(x => x.CampaignId == id);

                lblCandidateCount.Text = string.Format(totalCandidates == 1 ? candidateCountStr[1] : candidateCountStr[0], totalCandidates);


                if (item.IsUnproctored)
                {
                    listReceipients.Items.Remove(new ListItem("Scheduled Candidates In The Campaign", "2"));
                    listReceipients.Items.Remove(new ListItem("Unscheduled Candidates In The Campaign", "3"));
                }

                bttnSend.Visible = totalCandidates > 0;
            }
            else
            {
                Response.Redirect("Campaigns.aspx");
            }
        }

        protected void bttnBackToCampaignDetails_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlMapper.CampaignDetails + "?id=" + hidId.Value);
        }

        protected void listMessageType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var type = Int32.Parse(listMessageType.SelectedValue);


            if (type == 1)
            {
                trEmail.Visible = true;
                trEmailSubject.Visible = true;

                trSms.Visible = true;
                trSmsHeader.Visible = true;

                chkPreview.Visible = true;
            }
            else if (type == 2)
            {
                trEmail.Visible = true;
                trEmailSubject.Visible = true;

                trSms.Visible = false;
                trSmsHeader.Visible = false;
                chkPreview.Visible = true;
            }
            else
            {
                trEmail.Visible = false;
                trEmailSubject.Visible = false;
                chkPreview.Visible = false;

                trSms.Visible = true;
                trSmsHeader.Visible = true;
            }
        }

        protected void listReceipients_SelectedIndexChanged(object sender, EventArgs e)
        {
            var scheduledPlaceHolders = @"<strong>Allowed Placeholders</strong><br />
                                    [[NAME]]
                                    <br />
                                    [[UNIQUE_ID]]
                                    <br />
                                    [[USERNAME]]
                                    <br />
                                    [[PASSWORD]]
                                    <br />
                                    [[CENTER_NAME]]
                                    <br />
                                    [[CENTER_ADDRESS]]
                                    <br />
                                    [[LOCATION]]
                                    <br />
                                    [[TEST_DATE]]
                                    <br />
                                    [[TEST_TIME]]
                                    <br />";

            var defaultPlaceHolders = @"<strong>Allowed Placeholders</strong><br />
                                    [[NAME]]
                                    <br />
                                    [[UNIQUE_ID]]
                                    <br />
                                    [[USERNAME]]
                                    <br />
                                    [[PASSWORD]]
                                    <br />";

            var testedPlaceHolders = @"<strong>Allowed Placeholders</strong><br />
                                    [[NAME]]
                                    <br />
                                    [[UNIQUE_ID]]
                                    <br />
                                    [[USERNAME]]
                                    <br />
                                    [[PASSWORD]]
                                    <br />
                                    [[ASSESSMENT_SCORES]]
                                    <br />
                                    [[DATE_TESTED]]
                                    <br /> 
                                    [[TEST_CENTER]]
                                    <br />";

            var defaultPlaceHoldersSms = @"<strong>Allowed Placeholders</strong><br />
                                    [[USERNAME]]
                                    <br />
                                    [[PASSWORD]]
                                    <br />
                                    [[UNIQUE_ID]]
                                    <br />";

            var testedPlaceHoldersSms = @"<strong>Allowed Placeholders</strong><br />
                                    [[USERNAME]]
                                    <br />
                                    [[PASSWORD]]
                                    <br />
                                    [[AGG_SCORE]]
                                    <br />
                                    [[UNIQUE_ID]]
                                    <br />";

            var type = (MessageType)Int32.Parse(listReceipients.SelectedValue);

            if (type == MessageType.Scheduled_Candidates)
            {
                trCenter.Visible = true;

                lblPlaceHolders.Text = scheduledPlaceHolders;
                lblSmsPlaceholders.Text = defaultPlaceHoldersSms;
                LoadCenters();
            }

            else if (type == MessageType.Tested_Candidates)
            {
                trCenter.Visible = false;
                trSessions.Visible = false;

                lblPlaceHolders.Text = testedPlaceHolders;
                lblSmsPlaceholders.Text = testedPlaceHoldersSms;
            }

            else if (type == MessageType.Untested_Candidates)
            {
                trCenter.Visible = false;
                trSessions.Visible = false;

                lblPlaceHolders.Text = defaultPlaceHolders;
                lblSmsPlaceholders.Text = defaultPlaceHoldersSms;
            }
            else
            {
                trCenter.Visible = false;
                trSessions.Visible = false;

                lblPlaceHolders.Text = defaultPlaceHolders;
                lblSmsPlaceholders.Text = defaultPlaceHoldersSms;
            }

            var candidateCountStr = new string[] { "{0} Candidates found", "{0} Candidate found" };

            var ctx = new ServiceBase().Context;

            IQueryable<CampaignEntry> query = null;

            int campaignId = Int32.Parse(hidId.Value);

            switch (type)
            {
                case MessageType.All_Candidates:
                    {
                        query = ctx.CampaignEntries.Where(x => x.CampaignId == campaignId);
                        break;
                    }

                case MessageType.Scheduled_Candidates:
                    {
                        query = ctx.CampaignEntries.Where(x => x.CampaignId == campaignId && x.Scheduled && x.Tested == false);
                        break;
                    }

                case MessageType.Unscheduled_Candidates:
                    {
                        query = ctx.CampaignEntries.Where(x => x.CampaignId == campaignId && x.Scheduled == false && x.Tested == false);
                        break;
                    }

                case MessageType.Tested_Candidates:
                    {
                        query = ctx.CampaignEntries.Where(x => x.CampaignId == campaignId && x.Tested);
                        break;
                    }

                case MessageType.Untested_Candidates:
                {
                    query = ctx.CampaignEntries.Where(x => x.CampaignId == campaignId && x.Tested == false);
                    break;
                }

                default:
                    {
                        query = ctx.CampaignEntries.Where(x => x.CampaignId == campaignId);
                        break;
                    }
            }
            var totalCandidates = query.Count();

            lblCandidateCount.Text = string.Format(totalCandidates == 1 ? candidateCountStr[1] : candidateCountStr[0], totalCandidates);

            bttnSend.Visible = totalCandidates > 0;

        }

        private void LoadCenters()
        {
            var list = new PartnerCenterService().GetCentersForCandidatesInCampaign(Int32.Parse(hidId.Value));

            if (list.Count > 0)
            {
                listCenters.Items.Clear();

                listCenters.Items.Add(new ListItem("All Centers", "0"));

                list.ForEach(x => listCenters.Items.Add(new ListItem(x.CenterName, x.CenterId.ToString())));
            }



        }

        protected void listCenters_SelectedIndexChanged(object sender, EventArgs e)
        {
            var type = Int32.Parse(listCenters.SelectedValue);

            if (type > 0)
            {
                var list = new TestSessionService().GetCurrentSessionsForCenter(type);

                chkListSessions.Items.Clear();

                if (list.Count > 0)
                {
                    list.ForEach(x => chkListSessions.Items.Add(new ListItem(x.DisplayTextForUnscheduled, x.SessionId.ToString())));
                }


                trSessions.Visible = true;
            }
            else
            {
                trSessions.Visible = false;
            }
        }

        protected void bttnSend_Click(object sender, EventArgs e)
        {
            ProcessMessageOptions();
        }

        private void ProcessMessageOptions()
        {
            var type = (MessageType)Int32.Parse(listReceipients.SelectedValue);

            int selectedCenter = Int32.Parse(string.IsNullOrWhiteSpace(listCenters.SelectedValue) ? "0" : listCenters.SelectedValue);

            var ctx = new ServiceBase().Context;
            IQueryable<CampaignEntry> query = null;

            int campaignId = Int32.Parse(hidId.Value);

            switch (type)
            {
                case MessageType.All_Candidates:
                    {

                        query = ctx.CampaignEntries.Where(x => x.CampaignId == campaignId);
                        break;
                    }

                case MessageType.Scheduled_Candidates:
                    {
                        if (selectedCenter == 0)
                        {

                            query =
                                ctx.CampaignEntries.Where(
                                    x => x.CampaignId == campaignId && x.Scheduled && x.Tested == false);

                        }
                        else
                        {


                            var IDList = chkListSessions.Items.Cast<ListItem>()
                                                   .Where(item => item.Selected)
                                                   .Select(x => Int32.Parse(x.Value))
                                                   .ToList();



                            query = ctx.CampaignEntries.Where(x => x.CampaignId == campaignId && x.Scheduled && x.Tested == false & IDList.Any(y => y == x.SessionId));

                        }
                        break;
                    }

                case MessageType.Unscheduled_Candidates:
                    {

                        query = ctx.CampaignEntries.Where(x => x.CampaignId == campaignId && x.Scheduled == false && x.Tested == false);
                        break;
                    }

                case MessageType.Tested_Candidates:
                    {

                        query = ctx.CampaignEntries.Where(x => x.CampaignId == campaignId && x.Tested);
                        break;
                    }

                case MessageType.Untested_Candidates:
                {

                    query = ctx.CampaignEntries.Where(x => x.CampaignId == campaignId && x.Tested == false);
                    break;
                }
            }


            

            var list = query.Select(x =>
                            new CandidateMessagingViewModel
                            {
                                Email = x.Candidate.Email,
                                MobileNo = x.Candidate.MobileNo,
                                CampaignEntryId = x.EntryId,
                            }).ToList();



            int mediumType = Int32.Parse(listMessageType.SelectedValue);

            bool sendEmail = false;
            bool sendSms = false;

            if (mediumType == 1) sendEmail = sendSms = true;
            if (mediumType == 2) sendEmail = true;
            if (mediumType == 3) sendSms = true;

            SendMessages(list, mediumType, sendEmail, sendSms, type);


        }



        public void SendMessages(List<CandidateMessagingViewModel> messageList, int mediumType, bool sendEmail, bool sendSms, MessageType type)
        {

            var str = string.Empty;


            if (sendEmail)
            {
                var emailMessage = editor.Content;

                var emailBatch = new EmailBatch();
                emailBatch.BatchDate = DateTime.Now;
                emailBatch.MessageType = (int) type;
                emailBatch.EmailFrom = "admin@fot.com.ng";
                emailBatch.EmailName = "FOT Administrator";
                emailBatch.EmailSubject = txtEmailSubject.Text;
                emailBatch.EmailText = emailMessage;


                var emailService = new EmailBatchService();
                int emailBatchId = emailService.AddBatch(emailBatch);

                var emailList = new List<EmailQueueModel>();

                foreach (var item in messageList)
                {

                    var emailQueue = new EmailQueueModel
                    {
                        CampaignEntryId = item.CampaignEntryId,
                        EmailTo = item.Email,
                        BatchId = emailBatchId,
                        Sent = false,
                        DateTimeQueued =  DateTime.Now
                    };

                   
                    emailList.Add(emailQueue);
                }

                

                Utilities.BulkInsert(emailService.Context.Database.Connection.ConnectionString, "EmailQueue", emailList);


                str = "Email Messages queued for sending: " + messageList.Count;
            }


            if (sendSms)
            {
                var smsMessage = txtSms.Text;

                var messageBatch = new MessageBatch();
                messageBatch.BatchDate = DateTime.Now;
                messageBatch.MessageType = (int) type;
                messageBatch.MessageFrom = txtSmsHeader.Text;
                messageBatch.Message = smsMessage;


                var smsService = new MessageBatchService();
                int batchId = smsService.AddBatch(messageBatch);

                var smsList = new List<MessageQueueModel>();

                foreach (var item in messageList)
                {

                    var messageQueue = new MessageQueueModel
                    {
                        CampaignEntryId = item.CampaignEntryId,
                        MobileNumber = FormatMobile(item.MobileNo),
                        DateTimeQueued = DateTime.Now,
                        BatchId = batchId,
                        Sent = false
                    };

                    smsList.Add(messageQueue);
                }


                Utilities.BulkInsert(smsService.Context.Database.Connection.ConnectionString, "MessageQueue", smsList);

                str = str.Equals(string.Empty) ? "Sms Messages queued for sending:  " + messageList.Count : str + "<br/>Sms Messages queued for sending: " + messageList.Count;
            }


            lblStatus.ShowMessage(new AppMessage { IsDone = true, Message = str, Status = MessageStatus.Success });


        }


 

        public string FormatMobile(string mobile)
        {


            if (mobile[0].ToString().Equals("0") && mobile.Length == 11)
            {
                return "234" + mobile.Substring(1);
            }
            else
            {
                return mobile;
            }
        }
    }
}