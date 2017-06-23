using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SmsSender
{
    class Sender
    {


        public void RetrieveAndSendMessages()
        {
            using (var ctx = new SmsContext())
            {


                var smsList = ctx.MessageQueues.Where(x => x.Sent == false)
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
                      }).AsNoTracking().ToList();

                var batchIds = smsList.Select(x => x.BatchId).Distinct().ToList();

                var batches = ctx.MessageBatches.Where(x => batchIds.Contains(x.BatchId)).AsNoTracking().ToList();

               
                int success_count = 0;
                try
                {

                    foreach(var entry in smsList)
                    {
                        var batch = batches.First(x => x.BatchId == entry.BatchId);

                        var today = DateTime.Now;
                        var entryId = entry.EntryId;

                        var message = FormatPlaceHoldersForSms(entry,batch.Message);
                        var messageFrom = batch.MessageFrom;
                        var mobileNumber = entry.MobileNo;


                        if (SendSms(messageFrom, mobileNumber, message))
                        {
                           


                            var sql = "update MessageQueue set sent = 1, DateTimeSent = {0} where EntryId = {1}";


                            ctx.Database.ExecuteSqlCommand(sql, today, entryId);



                            success_count++;
                        }

                    

                    }

                    
                    

                    if (success_count > 0)
                        WriteToFile(DateTime.Now.ToLongTimeString() + " Sms sent successfully");
                }
                catch (Exception ex)
                {
                    WriteToFile(DateTime.Now.ToShortTimeString() + " Success = " + success_count + " Error:" +
                                ex.Message);
                }
            }


        }


        public bool SendSms(string smsHeader, string mobileNumber, string message)
        {
            string provider = ConfigurationManager.AppSettings["SMS_PROVIDER"];

            switch (provider)
            {
                
                case "CLICKATELL":
                    {
                        return SendClickatell(smsHeader, mobileNumber, message);
                        break;
                        
                    }
                case "CELLENT":
                    {
                        return SendCellent(smsHeader, mobileNumber, message);
                        break;

                    }

                case "DUDUMOBILE":
                    {
                        return SendDuduMobile(smsHeader, mobileNumber, message);
                        break;

                    }

                case "MAGICBULK":
                    {
                        return SendMagicBulk(smsHeader, mobileNumber, message);
                        break;

                    }

                case "INFOBIP":
                    {
                        return SendInfoBip(smsHeader, mobileNumber, message);
                        break;

                    }

                default :
                    {
                        WriteToFile("No compatible provider was found. valid options are 'CELLENT', 'CLICKATELL', 'DUDUMOBILE', 'MAGICBULK', 'INFOBIP'");

                        return false;
                        break;
                        
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



        public bool SendCellent(string header,string mobile, string message )
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

            WriteToFile(responseFromServer);
            WriteToFile(mobile);

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


        public string FormatPlaceHoldersForSms(CandidateDetailsViewModel item, string emailMessage)
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
    }
}
