using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using NLog;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace MessageService
{
    public partial class SenderService : ServiceBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public SenderService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            TimerProcess.Enabled = true;


            var initLogStr = $"[MessageService] Service started";

            WriteToLog(initLogStr, LogLevel.Info);

        }

        protected override void OnStop()
        {

            TimerProcess.Enabled = false;

            var initLogStr = $"[MessageService] Service stopped";

            WriteToLog(initLogStr, LogLevel.Info);


        }




        private void TimerProcess_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                ProcessSMS();
            }
            catch (Exception ex)
            {
                var logStr = $"[MessageService][SMS] Sending Error occured ({ex.Message + "| " + ex.InnerException?.Message + " | " + ex.InnerException?.InnerException?.Message})";

                WriteToLog(logStr, LogLevel.Error);
            }

            try
            {
                ProcessEmail();
            }
            catch (Exception ex)
            {
                var logStr = $"[MessageService][EMAIL] Sending Error occured ({ex.Message + "| " + ex.InnerException?.Message + " | " + ex.InnerException?.InnerException?.Message})";

                WriteToLog(logStr, LogLevel.Error);
            }

            TimerProcess.Enabled = true;

        }

        private void ProcessEmail()
        {
            using (var ctx = new SmsContext())
            {


                var emailList = ctx.EmailQueues.Where(x => x.Sent == false)
                       .Include(x => x.CampaignEntry.AssessmentResults)
                       .Select(x => new CandidateDetailsViewModel
                       {
                           EntryId = x.EntryId,
                           BatchId = x.BatchId,
                           Username = x.CampaignEntry.Candidate.Username,
                           UniqueId = x.CampaignEntry.Candidate.ClientUniqueID,
                           Password = x.CampaignEntry.Candidate.Password,
                           Firstname = x.CampaignEntry.Candidate.FirstName,
                           Lastname = x.CampaignEntry.Candidate.LastName,
                           Email = x.CampaignEntry.Candidate.Email,
                           MobileNo = x.CampaignEntry.Candidate.MobileNo,
                           DateTested = x.CampaignEntry.DateTested,
                           CenterAddress = x.CampaignEntry.SessionId.HasValue ? x.CampaignEntry.TestSession.Center.Address : string.Empty,
                           TestDate = x.CampaignEntry.SessionId.HasValue ? x.CampaignEntry.TestSession.TestDate : default(DateTime?),
                           TestTime = x.CampaignEntry.SessionId.HasValue ? x.CampaignEntry.TestSession.TimeText : string.Empty,
                           Locaton =
                                x.CampaignEntry.SessionId.HasValue ? x.CampaignEntry.TestSession.Center.Location.LocationName : string.Empty,
                           CenterName = x.CampaignEntry.SessionId.HasValue ? x.CampaignEntry.TestSession.Center.CenterName : string.Empty,
                           ResultList =
                                x.CampaignEntry.AssessmentResults.Where(q => q.Assessment.AssessmentType == (int)AssessmentType.MCQ)
                                 .Select(y => new ResultViewModel
                                 {
                                     EntryId = y.EntryId,
                                     AssessmentId = y.AssessmentId,
                                     AssessmentName = y.Assessment.Name,
                                     TestScore = y.TestScore,
                                     CandidateOptions = y.CandidateOptions
                                 })
                       }).Take(2000).AsNoTracking().ToList();

                var batchIds = emailList.Select(x => x.BatchId).Distinct().ToList();

                var batches = ctx.EmailBatches.Where(x => batchIds.Contains(x.BatchId)).AsNoTracking().ToList();


                var emailEntries = new List<EmailViewModel>();


                emailList.ForEach(x =>
                {
                    var temp = new EmailViewModel();
                    var batch = batches.First(y => y.BatchId == x.BatchId);

                    var type = (MessageType)batch.MessageType.Value;

                    temp.EntryId = x.EntryId;
                    temp.Email = x.Email;
                    temp.Message = DecidePlaceHolder(type, x, batch.EmailText);
                    temp.Subject = batch.EmailSubject;


                    emailEntries.Add(temp);
                });


                if (emailEntries.Any())
                {

                    Parallel.ForEach(emailEntries, DoEmail);


                    WriteToLog($"[MessageService][EMAIL] {emailEntries.Count} Emails Processed.", LogLevel.Info);
                }


                
            }

        }

        private void ProcessSMS()
        {
            using (var ctx = new SmsContext())
            {


                var smsList = ctx.MessageQueues.Where(x => x.Sent == false && x.Valid)
                      .Include(x => x.CampaignEntry.AssessmentResults)
                      .Select(x => new CandidateDetailsViewModel
                      {
                          EntryId = x.EntryId,
                          BatchId = x.BatchId,
                          Username = x.CampaignEntry.Candidate.Username,
                          UniqueId = x.CampaignEntry.Candidate.ClientUniqueID,
                          Password = x.CampaignEntry.Candidate.Password,
                          Firstname = x.CampaignEntry.Candidate.FirstName,
                          Lastname = x.CampaignEntry.Candidate.LastName,
                          MobileNo = x.MobileNumber,
                          ResultList =
                              x.CampaignEntry.AssessmentResults.Where(q => q.Assessment.AssessmentType == (int)AssessmentType.MCQ)
                               .Select(y => new ResultViewModel
                               {
                                   EntryId = y.EntryId,
                                   AssessmentId = y.AssessmentId,
                                   AssessmentName = y.Assessment.Name,
                                   TestScore = y.TestScore,
                                   CandidateOptions = y.CandidateOptions
                               })
                      }).Take(2000).AsNoTracking().ToList();

                var batchIds = smsList.Select(x => x.BatchId).Distinct().ToList();

                var batches = ctx.MessageBatches.Where(x => batchIds.Contains(x.BatchId)).AsNoTracking().ToList();



                var messageList = new List<MessageViewModel>();

                smsList.ForEach(x =>
                {
                    var temp = new MessageViewModel();
                    var batch = batches.First(y => y.BatchId == x.BatchId);

                    temp.EntryId = x.EntryId;
                    temp.Message = FormatPlaceHoldersForSms(x, batch.Message);
                    temp.MessageFrom = batch.MessageFrom;
                    temp.MobileNo = x.MobileNo;

                    messageList.Add(temp);
                });


                if (messageList.Any())
                {

                    Parallel.ForEach(messageList, DoSMS);

                    WriteToLog($"[MessageService][SMS] {messageList.Count} SMS Processed.", LogLevel.Info);
                }



                ctx.Database.ExecuteSqlCommand("Update MessageQueue set Valid = 0 where RetryCount >= {0} and Valid = 1", 5);



            }

        }

        public void DoEmail(EmailViewModel model)
        {
            var ctx = new SmsContext();

            if (SendMail(model.Email, model.Subject, model.Message))
            {
                var today = DateTime.Now;

                var sql = "update EmailQueue set sent = 1, DateTimeSent = {0} where EntryId = {1}";


                ctx.Database.ExecuteSqlCommand(sql, today, model.EntryId);



            }

        }


        public void DoSMS(MessageViewModel model)
        {
            var ctx = new SmsContext();

            var today = DateTime.Now;

            if (SendSms(model.MessageFrom, model.MobileNo, model.Message))
            {

                var sql = "update MessageQueue set sent = 1, DateTimeSent = {0} where EntryId = {1}";


                ctx.Database.ExecuteSqlCommand(sql, today, model.EntryId);

            }
            else
            {       

                    ctx.Database.ExecuteSqlCommand("update MessageQueue set RetryCount = RetryCount + 1 where EntryId = {0}", model.EntryId);
            }

        }

        private string FormatPlaceHoldersForSms(CandidateDetailsViewModel item, string emailMessage)
        {
            var str = emailMessage;

            if (str.Contains("[[USERNAME]]"))
            {
                str = str.Replace("[[USERNAME]]", item.Username);
            }


            if (str.Contains("[[UNIQUE_ID]]"))
            {
                str = str.Replace("[[UNIQUE_ID]]", item.UniqueId);
            }

            if (str.Contains("[[PASSWORD]]"))
            {
                str = str.Replace("[[PASSWORD]]", item.Password);
            }

            if (str.Contains("[[AGG_SCORE]]"))
            {

                var aggScore = (item.ResultList == null || item.ResultList.Any() == false) ? string.Empty : ((Convert.ToDouble(item.OverallScore) / item.OverallTotalQuestions) * 100.00).ToString("#,##0.##") + "%";

                str = str.Replace("[[AGG_SCORE]]", aggScore);
            }


            return str;

        }

        public bool SendSms(string smsHeader, string mobileNumber, string message)
        {
            string provider = ConfigurationManager.AppSettings["SMS_PROVIDER"];

            switch (provider)
            {

                case "CLICKATELL":
                    {
                        return SendClickatell(smsHeader, mobileNumber, message);
                      

                    }
                case "CELLENT":
                    {
                        return SendCellent(smsHeader, mobileNumber, message);
                       

                    }

                case "DUDUMOBILE":
                    {
                        return SendDuduMobile(smsHeader, mobileNumber, message);
                       

                    }

                case "MAGICBULK":
                    {
                        return SendMagicBulk(smsHeader, mobileNumber, message);
                       

                    }

                case "INFOBIP":
                    {
                        return SendInfoBip(smsHeader, mobileNumber, message);
                      

                    }

                default:
                    {
                        WriteToLog("No compatible provider was found. valid options are 'CELLENT', 'CLICKATELL', 'DUDUMOBILE', 'MAGICBULK', 'INFOBIP'", LogLevel.Error);

                        return false;


                    }

            }
        }


        private bool SendInfoBip(string smsHeader, string mobileNumber, string message)
        {

            var client = new WebClient();

            string message2 = HttpUtility.UrlEncode(message.Trim());


            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; .NET CLR 2.0.50727; .NET CLR 3.0.04506.30; .NET CLR 1.1.4322; .NET CLR 3.5.20404)");

            client.QueryString.Add("user", "dragnetltd");

            client.QueryString.Add("password", "$Dragnet247");

            client.QueryString.Add("GSM", mobileNumber);

            client.QueryString.Add("SMSText", message2);

            client.QueryString.Add("sender", smsHeader);

            string baseurl = "http://api2.infobip.com/api/sendsms/plain";

            string s = client.DownloadString(baseurl);


            var temp = s.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            bool flag = long.Parse(temp[0]) > 0;

            return flag;

        }

        private bool SendClickatell(string smsHeader, string mobileNumber, string message)
        {

            var client = new WebClient();

            string message2 = HttpUtility.UrlEncode(message.Trim());


            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; .NET CLR 2.0.50727; .NET CLR 3.0.04506.30; .NET CLR 1.1.4322; .NET CLR 3.5.20404)");

            client.QueryString.Add("user", "dragnet");

            client.QueryString.Add("password", "dragnet147");

            client.QueryString.Add("api_id", "3376377");

            client.QueryString.Add("to", mobileNumber);

            client.QueryString.Add("text", message2);

            client.QueryString.Add("from", smsHeader);

            string baseurl = "http://api.clickatell.com/http/sendmsg";

            string s = client.DownloadString(baseurl);

            bool flag = s.StartsWith("ID");

            if (!flag)
            {
                // WriteToFile("Error: "+s);
            }

            return flag;

        }



        private bool SendMagicBulk(string smsHeader, string mobileNumber, string message)
        {

            var client = new WebClient();

            var username = "segun@dragnet-solutions.com";
            var password = "dragnet";


            var encodedMessage = HttpUtility.UrlEncode(message);



            var url = "http://api2.magicbulk.com/?username={0}&password={1}&destination={2}&source={3}&message={4}&type=0";


            var actualUrl = string.Format(url, username, password, mobileNumber, smsHeader, encodedMessage);


            var result = client.DownloadString(actualUrl);

            var list = result.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

            var sent = list[0].Equals("1701") || list[0].Equals("1026");

            if (!sent)
            {
                //WriteToFile("Error: " + result);
            }

            return sent;

        }



        public bool SendCellent(string header, string mobile, string message)
        {
            string str = string.Empty;
            str = mobile;
            string message2 = HttpUtility.UrlEncode(message.Trim());

            string url = "http://174.143.34.193/MtSendSMS/SingleSMS.aspx?usr=dragnet&pass=S1bxb4&msisdn=" + mobile + "&msg=" + message2 + "&sid=" + header + "&mt=0";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Credentials = CredentialCache.DefaultCredentials;

            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; .NET CLR 2.0.50727; .NET CLR 3.0.04506.30; .NET CLR 1.1.4322; .NET CLR 3.5.20404)";
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream);

            string responseFromServer = reader.ReadToEnd();

            string[] arr = responseFromServer.Split('-');
            bool flag = arr[0].Equals(mobile);
            return flag;

        }


        public bool SendDuduMobile(string header, string mobile, string message)
        {
            string str = string.Empty;

            str = mobile;

            string message2 = HttpUtility.UrlEncode(message.Trim());



            string url = "http://smsapi.dudumobile.com/index.php?user=dragnet&pass=dragnet&to=" + mobile + "&from=" + header + "&msg=" + message2;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Credentials = CredentialCache.DefaultCredentials;

            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; .NET CLR 2.0.50727; .NET CLR 3.0.04506.30; .NET CLR 1.1.4322; .NET CLR 3.5.20404)";
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream);

            string responseFromServer = reader.ReadToEnd();

            bool flag = responseFromServer.Trim().Equals("sent");

            return flag;
        }

        private void WriteToLog(string text, LogLevel logLevel)
        {

            var shouldLog = Convert.ToBoolean(ConfigurationManager.AppSettings["LOG"]);

            if (!shouldLog) return;

            logger.Log(logLevel, text);

        }

        public bool SendMail(string emailTo, string emailSubject, string emailText)
        {

            try
            {

                var settings = ConfigurationManager.AppSettings;



                using (var client = new SmtpClient(settings["SmtpServer"]))
                {



                    var newMail = new MailMessage();
                    newMail.To.Add(new MailAddress(emailTo));
                    newMail.From = new MailAddress(settings["EmailAddress"], settings["EmailName"]);
                    newMail.Subject = emailSubject;
                    newMail.IsBodyHtml = true;


                    string body = emailText;

                    var view = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                    newMail.AlternateViews.Add(view);


                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(settings["MailFrom"], settings["MailFromPassword"]);
                    client.Port = Int32.Parse(settings["SmtpPort"]);


                    client.Send(newMail);

                    return true;
                }

            }
            catch (Exception ex)
            {
                WriteToLog(DateTime.Now.ToShortTimeString() + " Error:" +ex.Message, LogLevel.Error);

                return false;
            }

        }



        public string DecidePlaceHolder(MessageType type, CandidateDetailsViewModel item, string emailMessage)
        {

            switch (type)
            {
                case MessageType.All_Candidates:
                    {
                        return FormatPlaceHolders(item, emailMessage);
                       
                    }

                case MessageType.Scheduled_Candidates:
                    {
                        return FormatPlaceHoldersForScheduled(item, emailMessage);
                        
                    }

                case MessageType.Unscheduled_Candidates:
                    {
                        return FormatPlaceHolders(item, emailMessage);
                       
                    }

                case MessageType.Tested_Candidates:
                    {
                        return FormatPlaceHoldersForTested(item, emailMessage);
                        
                    }
                case MessageType.Untested_Candidates:
                {
                    return FormatPlaceHolders(item, emailMessage);

                }

                default:
                    {
                        return FormatPlaceHolders(item, emailMessage);
                       
                    }
            }
        }



        public string FormatPlaceHolders(CandidateDetailsViewModel item, string emailMessage)
        {
            var str = emailMessage;

            if (str.Contains("[[NAME]]"))
            {
                str = str.Replace("[[NAME]]", item.Firstname + " " + item.Lastname);
            }

            if (str.Contains("[[UNIQUE_ID]]"))
            {
                str = str.Replace("[[UNIQUE_ID]]", item.UniqueId);
            }

            if (str.Contains("[[USERNAME]]"))
            {
                str = str.Replace("[[USERNAME]]", item.Username);
            }

            if (str.Contains("[[PASSWORD]]"))
            {
                str = str.Replace("[[PASSWORD]]", item.Password);
            }




            return str;

        }

        public string FormatPlaceHoldersForScheduled(CandidateDetailsViewModel item, string emailMessage)
        {
            var str = emailMessage;

            if (str.Contains("[[NAME]]"))
            {
                str = str.Replace("[[NAME]]", item.Firstname + " " + item.Lastname);
            }

            if (str.Contains("[[USERNAME]]"))
            {
                str = str.Replace("[[USERNAME]]", item.Username);
            }

            if (str.Contains("[[UNIQUE_ID]]"))
            {
                str = str.Replace("[[UNIQUE_ID]]", item.UniqueId);
            }

            if (str.Contains("[[PASSWORD]]"))
            {
                str = str.Replace("[[PASSWORD]]", item.Password);
            }

            if (str.Contains("[[CENTER_NAME]]"))
            {
                str = str.Replace("[[CENTER_NAME]]", item.CenterName);
            }

            if (str.Contains("[[CENTER_ADDRESS]]"))
            {
                str = str.Replace("[[CENTER_ADDRESS]]", item.CenterAddress);
            }

            if (str.Contains("[[LOCATION]]"))
            {
                str = str.Replace("[[LOCATION]]", item.Locaton);
            }

            if (str.Contains("[[TEST_DATE]]"))
            {
                str = str.Replace("[[TEST_DATE]]", item.TestDate.HasValue ? item.TestDate.Value.ToString("dd-MMM-yyyy") : string.Empty);
            }

            if (str.Contains("[[TEST_TIME]]"))
            {
                str = str.Replace("[[TEST_TIME]]", item.TestTime);
            }


            return str;

        }

        public string FormatPlaceHoldersForTested(CandidateDetailsViewModel item, string emailMessage)
        {
            var str = emailMessage;

            if (str.Contains("[[NAME]]"))
            {
                str = str.Replace("[[NAME]]", item.Firstname + " " + item.Lastname);
            }

            if (str.Contains("[[USERNAME]]"))
            {
                str = str.Replace("[[USERNAME]]", item.Username);
            }

            if (str.Contains("[[UNIQUE_ID]]"))
            {
                str = str.Replace("[[UNIQUE_ID]]", item.UniqueId);
            }

            if (str.Contains("[[PASSWORD]]"))
            {
                str = str.Replace("[[PASSWORD]]", item.Password);
            }

            if (str.Contains("[[ASSESSMENT_SCORES]]"))
            {
                str = str.Replace("[[ASSESSMENT_SCORES]]", GetAssessmentHtml(item));
            }

            if (str.Contains("[[DATE_TESTED]]"))
            {
                str = str.Replace("[[DATE_TESTED]]", (item.TestDate.HasValue ? item.TestDate.Value.ToString("dd-MMM-yyyy") : string.Empty));
            }

            if (str.Contains("[[TEST_CENTER]]"))
            {
                str = str.Replace("[[TEST_CENTER]]", item.CenterName);
            }


            return str;

        }



        public string GetAssessmentHtml(CandidateDetailsViewModel item)
        {

            if (item.ResultList == null || item.ResultList.Any() == false) return string.Empty;



            var overallScore = (Convert.ToDouble(item.OverallScore) / item.OverallTotalQuestions) * 100.00;

            var overallScoreStr = overallScore.ToString("#,##0.##") + "%";

            var assessmentScores = GetHtmlScores(item.ResultList);

            var htmlStr = @"  <table style='width:100%;'>
                        {0}
                        <tr>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top; font-weight:bold; border-bottom:1px solid #f0f0f0;'>Aggregate</td>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top; font-weight:bold; border-bottom:1px solid #f0f0f0; width:50px;'>{1}</td>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top; font-weight:bold; border-bottom:1px solid #f0f0f0; width:50px;'>{2}</td>
                        </tr>
                        <tr>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top;'>&nbsp;</td>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top;'>&nbsp;</td>
                            <td style='padding: 10px; font-family: Arial; font-size: 12px; color: #666; vertical-align: top;'>&nbsp;</td>
                        </tr>
                        </table>";


            return string.Format(htmlStr, assessmentScores, item.OverallScore, overallScoreStr);


        }


        public string GetHtmlScores(IEnumerable<ResultViewModel> resultList)
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
}
