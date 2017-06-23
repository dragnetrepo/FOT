using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace EmailSender
{
    class Sender
    {

        public void RetrieveAndSendMessages()
        {
            using (var ctx = new EmailContext())
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
                        }).AsNoTracking().ToList();

                var batchIds = emailList.Select(x => x.BatchId).Distinct().ToList();

                var batches = ctx.EmailBatches.Where(x => batchIds.Contains(x.BatchId)).AsNoTracking().ToList();



                int success_count = 0;
                try
                {
                    foreach(var entry in emailList)
                    {
                        var entryId = entry.EntryId;

                        var batch = batches.First(x => x.BatchId == entry.BatchId);
                       
                        var today = DateTime.Now;

                        var subject = batch.EmailSubject;

                        var type = (MessageType)batch.MessageType.Value;

                        var message = DecidePlaceHolder(type, entry, batch.EmailText); //format placeholders

                        var candidateEmail = entry.Email;



                        if (SendMail(candidateEmail, subject, message))
                        {
                           

                            var sql = "update EmailQueue set sent = 1, DateTimeSent = {0} where EntryId = {1}";
                            

                            ctx.Database.ExecuteSqlCommand(sql, today, entryId);


                            success_count++;
                        }



                    }




                    if (success_count > 0)
                        WriteToFile(DateTime.Now.ToLongTimeString() + " | " + success_count + " Emails sent successfully");
                }
                catch (Exception ex)
                {
                    WriteToFile(DateTime.Now.ToShortTimeString() + " Success = " + success_count + " Error:" +
                                ex.Message);

                    throw;
                }
            }


        }

        private void WriteToFile(string text)
        {
            bool logFlag = Convert.ToBoolean(ConfigurationManager.AppSettings["LOG"]);
            var logDir = ConfigurationManager.AppSettings["LOGDIR"];

            if (logFlag && !string.IsNullOrEmpty(logDir))
            {
                string fileName = DateTime.UtcNow.ToString("yyyyMMddTHHmmss") + ".txt";
                string path = logDir;

                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                string fullpath = Path.Combine(path, fileName);

                using (StreamWriter sw = new StreamWriter(fullpath, true))
                {
                    sw.WriteLine(text);
                    sw.Close();
                }
            }
        }

        public void ProcessSentMessages()
        {
            using (var ctx = new EmailContext())
            {
                var emailList = ctx.EmailQueues.Where(x => x.Sent).ToList();

                foreach (var emailQueue in emailList)
                {

                    ctx.EmailQueues.Remove(emailQueue);
                }

                ctx.SaveChanges();
            }


        }




        public bool SendMail(string emailTo, string emailSubject, string emailText)
        {

          //  WriteToFile((emailText));

           // return true;


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
                    WriteToFile(DateTime.Now.ToShortTimeString()+ " Error:" +
                               ex.Message);
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
                    break;
                }

                case MessageType.Scheduled_Candidates:
                {
                    return FormatPlaceHoldersForScheduled(item, emailMessage);
                    break;
                }

                case MessageType.Unscheduled_Candidates:
                {
                    return FormatPlaceHolders(item, emailMessage);
                    break;
                }

                case MessageType.Tested_Candidates:
                {
                    return FormatPlaceHoldersForTested(item, emailMessage);
                    break;
                }

                default:
                {
                    return FormatPlaceHolders(item, emailMessage);
                    break;
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
