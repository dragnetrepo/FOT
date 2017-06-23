using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fot.DTO;
using ResultSync.AppServices;
using ResultSync.FotService;

namespace ResultSync
{
    public class Syncher
    {
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

        public  void ProcessTested()
        {

            try
            {
                var context = new AppServiceClient();


                var tested =  context.GetTestedList();



                    var service = new FotServiceClient();

                   


                    foreach (var candidate in tested)
                    {
                        var item = new ResultUpdateModel
                        {
                            CandidateId = candidate.CandidateId,
                            CampaignId = candidate.CampaignId,
                            SessionId = candidate.SessionId,
                            CandidatePhoto = candidate.CandidatePhoto,
                            TestStartTime = candidate.DateTimeStarted,
                            TestEndTime = candidate.DateTimeCompleted,
                            PhotoCapturedBy =  candidate.PhotoCapturedByAdminId,
                            Results = new List<ResultEntryModel>()
                        };


                        if (candidate.TestFeedback != null)
                        {
                            item.Feedback = new FeedbackModel();

                            item.Feedback.Directions = candidate.TestFeedback.Directions;
                            item.Feedback.WaitTime = candidate.TestFeedback.WaitTime;
                            item.Feedback.Professionalism = candidate.TestFeedback.Professionalism;
                            item.Feedback.StartTime = candidate.TestFeedback.StartTime;
                            item.Feedback.Briefing = candidate.TestFeedback.Briefing;
                            item.Feedback.Registration = candidate.TestFeedback.Registration;
                            item.Feedback.Overall = candidate.TestFeedback.Overall;
                            item.Feedback.UnsatisfactoryArea = candidate.TestFeedback.UnsatisfactoryArea;
                            item.Feedback.SatisfactoryArea = candidate.TestFeedback.SatisfactoryArea;
                            item.Feedback.Comments = candidate.TestFeedback.Comments;


                        }


                        foreach (var result in candidate.AssessmentResults)
                        {
                            if (!string.IsNullOrWhiteSpace(result.Score))
                            {
                                item.Results.Add(new ResultEntryModel
                                    {
                                        AssessmentId = result.AssessmentId,
                                        TestScore = Int32.Parse(FotSecurity<string>.Deserialize(Convert.FromBase64String(result.Score))),
                                        CandidateOptions = result.CandidateOptions
                                    });
                            }
                            else if(result.SelectedEssayId.HasValue)
                            {
                                item.Results.Add(new ResultEntryModel
                                {
                                    AssessmentId = result.AssessmentId,
                                    SelectedEssayId = result.SelectedEssayId,
                                    EssayText = result.EssayText
                                });
                            }
                        }


                        if (service.ResultUpdate(item)) //should work now.
                        {


                             context.SynchronizeCandidate(candidate.CandidateEntryId);


                            WriteToFile(DateTime.Now.ToLongTimeString() +
                                        " INFO: Successfully synchronized score for candidate id: = " +
                                        candidate.CandidateId);
                        }



                    }


                
            }
            catch (Exception ex)
            {
                WriteToFile(DateTime.Now.ToLongTimeString() +
                                      " ERROR: An error occurred Error Details: " + ex.Message);
            }
        }
    }
}
