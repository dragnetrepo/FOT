using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Forms.VisualStyles;
using Fot.Admin.Models;
using System.Data.Entity;

namespace Fot.Admin.Services
{
    public class AssessmentResultService : ServiceBase
    {
        public List<AssessmentResultViewModel> GetResults(int CampaignId, int startRow, int maxRows)
        {
            IEnumerable<AssessmentResultViewModel> query =
                Context.CampaignEntries.Where(x => x.CampaignId == CampaignId && x.Tested)
                       .Include(x => x.AssessmentResults)
                       .Select(x => new AssessmentResultViewModel
                            {
                                CampaignEntryId = x.EntryId,
                                CampaignId = x.CampaignId,
                                ProctorPlaybackId = x.ProctorPlaybackId,
                                CandidateName = x.Candidate.FirstName + " " + x.Candidate.LastName,
                                CandidateId = x.CandidateId,
                                DateTested = x.DateTested.Value,
                                StartTime = x.TestStartTime,
                                EndTime = x.TestEndTime,
                                CandidatePhoto = x.PhotoCaptured,
                                ResultList = x.AssessmentResults.Select(y => new ResultViewModel
                                    {
                                        EntryId = y.EntryId,
                                        AssessmentId = y.AssessmentId,
                                        AssessmentName = y.Assessment.Name,
                                        TestScore = y.TestScore,
                                        AssessmentType = y.Assessment.AssessmentType
                                    }).OrderBy(y => y.AssessmentId)
                            }).OrderBy(x => x.CampaignEntryId);

            if (startRow >= 0)
            {
                query = query.Skip(startRow).Take(maxRows);
            }

            return query.ToList();
        }

        public int Count(int CampaignId)
        {
            return Context.CampaignEntries.Count(x => x.CampaignId == CampaignId && x.Tested);
        }


        public AssessmentResult GetResultEntry(int EntryId)
        {
            return
                Context.AssessmentResults.Where(x => x.EntryId == EntryId)
                       .Include(x => x.CampaignEntry)
                       .Include(x => x.Assessment)
                       .Include(x => x.CampaignEntry.Candidate)
                       .FirstOrDefault();
        }

        public List<AssessmentResultDownloadViewModel> GetMCQResultsForDownload(int CampaignId)
        {
            return
                Context.CampaignEntries.Where(x => x.CampaignId == CampaignId && x.Tested)
                       .Include(x => x.AssessmentResults)
                       .
                        Select(x => new AssessmentResultDownloadViewModel
                            {
                                CampaignEntryId = x.EntryId,
                                CampaignId = x.CampaignId,
                                CandidateId = x.Candidate.CandidateId,
                                ClientUniqueId = x.Candidate.ClientUniqueID,
                                Firstname = x.Candidate.FirstName,
                                Lastname = x.Candidate.LastName,
                                Email = x.Candidate.Email,
                                MobileNo = x.Candidate.MobileNo,
                                DateTested = x.DateTested.Value,
                                Locaton =
                                    x.SessionId.HasValue ? x.TestSession.Center.Location.LocationName : string.Empty,
                                CenterName = x.SessionId.HasValue ? x.TestSession.Center.CenterName : string.Empty,
                                ResultList =
                                    x.AssessmentResults.Where(q => q.Assessment.AssessmentType == AssessmentType.MCQ)
                                     .Select(y => new ResultDownloadViewModel
                                         {
                                             EntryId = y.EntryId,
                                             AssessmentId = y.AssessmentId,
                                             AssessmentName = y.Assessment.Name,
                                             TestScore = y.TestScore,
                                             CandidateOptions = y.CandidateOptions
                                         }).OrderBy(y => y.AssessmentId)
                            }).ToList();
        }

        public List<AssessmentResultDownloadViewModel> GetMCQResultsForTopicDownload(int CampaignId)
        {
            var items =
                Context.CampaignEntries.Where(x => x.CampaignId == CampaignId && x.Tested)
                       .Include(x => x.AssessmentResults)
                       .
                        Select(x => new AssessmentResultDownloadViewModel
                        {
                            CampaignEntryId = x.EntryId,
                            CampaignId = x.CampaignId,
                            CandidateId = x.Candidate.CandidateId,
                            ClientUniqueId = x.Candidate.ClientUniqueID,
                            Firstname = x.Candidate.FirstName,
                            Lastname = x.Candidate.LastName,
                            Email = x.Candidate.Email,
                            MobileNo = x.Candidate.MobileNo,
                            DateTested = x.DateTested.Value,
                            Locaton =
                                x.SessionId.HasValue ? x.TestSession.Center.Location.LocationName : string.Empty,
                            CenterName = x.SessionId.HasValue ? x.TestSession.Center.CenterName : string.Empty,
                            ResultList =
                                x.AssessmentResults.Where(q => q.Assessment.AssessmentType == AssessmentType.MCQ)
                                 .Select(y => new ResultDownloadViewModel
                                 {
                                     EntryId = y.EntryId,
                                     AssessmentId = y.AssessmentId,
                                     AssessmentName = y.Assessment.Name,
                                     TestScore = y.TestScore,
                                     CandidateOptions = y.CandidateOptions
                                 }).OrderBy(y => y.AssessmentId)
                        }).ToList();


            if (items.Any())
            {

                var assessmentIds = items.First().ResultList.Select(x => x.AssessmentId).ToList();

                var questions =
                    Context.AssessmentQuestions.Where(x => assessmentIds.Contains(x.AssessmentId))
                        .Select(x => new {x.QuestionId, x.TopicId})
                        .ToList();

                var questionIds = questions.Select(x => x.QuestionId).ToList();


                var topics = Context.AssessmentTopics.Where(x => assessmentIds.Contains(x.AssessmentId)).ToList();

                var answers =
                    Context.AssessmentAnswers.Where(x => questionIds.Contains(x.QuestionId))
                        .Select(x => new {x.AnswerId, x.QuestionId, x.IsCorrect})
                        .ToList();


                if (!topics.Any()) return items;

                


                foreach (var item in items)
                {

                    foreach (var result in item.ResultList)
                    {
                        var resultReviewList = ResultReviewModel.ToResultReviewList(result.CandidateOptions,
                            result.AssessmentId);

                        foreach (var rlist in resultReviewList)
                        {

                            var realOptions = answers.Where(x => x.QuestionId == rlist.QuestionId && x.IsCorrect).Select(x => x.AnswerId).ToList();

                            rlist.Correct = rlist.Options.OrderBy(x => x).SequenceEqual(realOptions.OrderBy(x => x));


                            rlist.TopicId = questions.First(x => x.QuestionId == rlist.QuestionId).TopicId;

                        }

                        var topicIds = resultReviewList.Where(x => x.TopicId.HasValue).Select(x => x.TopicId.Value).Distinct().ToList();


                        if (!topicIds.Any()) return items;


                        var topicVm = new List<ResultTopicsViewModel>();

                        topicIds.ForEach(x =>
                        {
                            var topicId = x;
                            var name = topics.FirstOrDefault(y => y.TopicId == x).Topic;
                            var score = resultReviewList.Count(y => y.TopicId == x && y.Correct.HasValue && y.Correct.Value);

                            topicVm.Add(new ResultTopicsViewModel{TopicId = topicId, TopicName = name, TopicScore = score});
                        });

                        result.Topics = topicVm;
                    }

                    
                }


            }



            return items;
        }



         

        public List<PartnerAssessmentResultViewModel> PartnerGetResults(int CampaignId, string searchTerm, int startRow, int maxRows)
        {

            var currentAdmin = new AdminUserService().GetCurrentAdmin();

            var ctx = new ServiceBase().Context;

            var setting = ctx.Settings.FirstOrDefault(x => x.SettingName == "ENABLE_REVIEWING_NON_OWNED_ASSESSMENTS");

            bool allow = false;

            if (setting != null)
            {
                allow = Convert.ToBoolean(setting.SettingValue);
            }


            IEnumerable<PartnerAssessmentResultViewModel> query = null;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
               query = Context.CampaignEntries.Where(x => x.CampaignId == CampaignId && x.Tested)
                    .Include(x => x.AssessmentResults)
                    .Select(x => new PartnerAssessmentResultViewModel
                    {
                        CampaignEntryId = x.EntryId,
                        ProctorPlaybackId = x.ProctorPlaybackId,
                        CandidateName = x.Candidate.FirstName + " " + x.Candidate.LastName,
                        DateTested = x.DateTested.Value,
                        StartTime = x.TestStartTime,
                        EndTime = x.TestEndTime,
                        CandidatePhoto = x.PhotoCaptured,
                        CandidateId = x.CandidateId,
                        CampaignId = x.CampaignId,
                        AllowReview = allow,
                        ResultList = x.AssessmentResults.Select(y => new PartnerResultViewModel
                        {
                            EntryId = y.EntryId,
                            AssessmentId = y.AssessmentId,
                            PartnerOwned =
                                y.Assessment.OwnerPartnerId.HasValue &&
                                (y.Assessment.OwnerPartnerId == currentAdmin.PartnerId),
                            AssessmentName = y.Assessment.Name,
                            TestScore = y.TestScore,
                            AssessmentType = y.Assessment.AssessmentType
                        }).OrderBy(y => y.AssessmentId)
                    }).OrderBy(x => x.CampaignEntryId);
            }

            else
            {

                query = query = Context.CampaignEntries.Where(x => x.CampaignId == CampaignId && x.Tested && x.Candidate.Username == searchTerm)
                    .Include(x => x.AssessmentResults)
                    .Select(x => new PartnerAssessmentResultViewModel
                    {
                        CampaignEntryId = x.EntryId,
                        ProctorPlaybackId = x.ProctorPlaybackId,
                        CandidateName = x.Candidate.FirstName + " " + x.Candidate.LastName,
                        DateTested = x.DateTested.Value,
                        StartTime = x.TestStartTime,
                        EndTime = x.TestEndTime,
                        CandidatePhoto = x.PhotoCaptured,
                        CandidateId = x.CandidateId,
                        CampaignId = x.CampaignId,
                        AllowReview = allow,
                        ResultList = x.AssessmentResults.Select(y => new PartnerResultViewModel
                        {
                            EntryId = y.EntryId,
                            AssessmentId = y.AssessmentId,
                            PartnerOwned =
                                y.Assessment.OwnerPartnerId.HasValue &&
                                (y.Assessment.OwnerPartnerId == currentAdmin.PartnerId),
                            AssessmentName = y.Assessment.Name,
                            TestScore = y.TestScore,
                            AssessmentType = y.Assessment.AssessmentType
                        }).OrderBy(y => y.AssessmentId)
                    }).OrderBy(x => x.CampaignEntryId);
            }


            if (startRow >= 0)
            {
                query = query.Skip(startRow).Take(maxRows);
            }

            return query.ToList();
        }


        public int PartnerCount(int CampaignId, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Context.CampaignEntries.Count(x => x.CampaignId == CampaignId && x.Tested);
            }
            else
            {
                return Context.CampaignEntries.Count(x => x.CampaignId == CampaignId && x.Tested && x.Candidate.Username == searchTerm);
            }
        }
    }
}