using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;

namespace Fot.Admin.Services
{
    public class ResultUpdateService
    {

       


        public bool ResultUpdate(ResultUpdateModel result)
        {
            using (var Context = new FotContext())
            {
                var item =
                       Context.CampaignEntries.FirstOrDefault(
                           x =>
                           (x.CampaignId == result.CampaignId) && (x.SessionId == result.SessionId) && (x.CandidateId == result.CandidateId) && x.Scheduled);


                if (item != null)
                {

                    if (item.Tested) return true;



                    if (result.CandidatePhoto != null && result.CandidatePhoto.Length > 3)
                    {
                        using(var ms = new MemoryStream(result.CandidatePhoto))
                        using (var image = Image.FromStream(ms))
                        {
                            var campaignFolder =
                                HttpContext.Current.Server.MapPath(UrlMapper.RootPhotosDirectory +
                                                                   item.CampaignId.ToString());

                            if (!Directory.Exists(campaignFolder)) Directory.CreateDirectory(campaignFolder);

                            image.Save(
                                Path.Combine(campaignFolder,
                                    string.Format("{0}_{1}.jpg", item.CandidateId, item.EntryId)), ImageFormat.Jpeg);
                        }

                        item.PhotoCaptured = true;


                        item.PhotoCapturedBy = result.PhotoCapturedBy;
                    }

                    item.Tested = true;
                    item.DateTested = DateTime.Today;



                    item.TestStartTime = result.TestStartTime;
                    item.TestEndTime = result.TestEndTime;

                    if (result.Feedback != null)
                    {
                        var feedback = new TestFeedback();

                        feedback.CampaignEntryId = item.EntryId;
                        feedback.Directions = result.Feedback.Directions;
                        feedback.WaitTime = result.Feedback.WaitTime;
                        feedback.Professionalism = result.Feedback.Professionalism;
                        feedback.StartTime = result.Feedback.StartTime;
                        feedback.Briefing = result.Feedback.Briefing;
                        feedback.Registration = result.Feedback.Registration;
                        feedback.Overall = result.Feedback.Overall;
                        feedback.UnsatisfactoryArea = result.Feedback.UnsatisfactoryArea;
                        feedback.SatisfactoryArea = result.Feedback.SatisfactoryArea;
                        feedback.Comments = result.Feedback.Comments;

                        Context.TestFeedbacks.Add(feedback);

                    }

                    foreach (var entry in result.Results)
                    {
                        item.AssessmentResults.Add(new AssessmentResult
                            {
                                CampaignEntryId = item.EntryId,
                                AssessmentId = entry.AssessmentId,
                                TestScore = entry.TestScore,
                                CandidateOptions = entry.CandidateOptions,
                                SelectedEssayId = entry.SelectedEssayId,
                                EssayText = entry.EssayText
                            });


                        if (!string.IsNullOrWhiteSpace(entry.CandidateOptions))
                        {
                            var questionList = entry.CandidateOptions.Split(';');

                            foreach (var question in questionList)
                            {

                                var arr = question.Split(':');

                                var showQuestion = new ShownQuestion
                                    {
                                        CampaignId = item.CampaignId,
                                        CampaignEntryId = item.EntryId,
                                        QuestionId = Int32.Parse(arr[0]),
                                        EntryDate = DateTime.Today
                                    };

                                if (!arr[1].Equals("0"))
                                {
                                    var answerList = arr[1].Split(',').ToList();

                                    answerList.ForEach(
                                        x => showQuestion.ChosenOptions.Add(new ChosenOption { AnswerId = Int32.Parse(x) }));
                                }

                                Context.ShownQuestions.Add(showQuestion);
                            }
                        }
                    }


                    var partner = Context.Campaigns.Where(x => x.CampaignId == result.CampaignId).Select(x => x.Partner).FirstOrDefault();

                    if (partner.IsSelfManaged)
                    {
                        var center =
                            Context.TestSessions.Where(x => x.SessionId == result.SessionId)
                                   .Select(x => x.Center)
                                   .FirstOrDefault();
                        var amount = center.IsPrivateCenter ? partner.CostPerTestPrivate.Value : partner.CostPerTestPublic.Value;

                        Context.PartnerWalletDebits.Add(new PartnerWalletDebit
                            {
                                PartnerId = partner.PartnerId,
                                CampaignEntryId = item.EntryId,
                                Amount = amount,
                                DebitDate = DateTime.Today
                            });

                        partner.WalletBalance = partner.WalletBalance - amount;

                    }

                    if (partner.IsIntegrationPartner.HasValue &&
                        (partner.IsIntegrationPartner.Value && !string.IsNullOrWhiteSpace(partner.ResultsPostUrl)))
                    {

                        //TO DO: add entry for scheduled results notification via results post url

                    }


                    Context.SaveChanges();

                    return true;
                }

                else
                {
                    return false;
                } 
            }
        }
    }
}