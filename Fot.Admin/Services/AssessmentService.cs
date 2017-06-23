using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using Fot.Admin.Infrastructure;
using Fot.Admin.Models;

namespace Fot.Admin.Services
{
    public class AssessmentService : ServiceBase
    {
     

        public List<AssessmentViewModel> GetAssessments(string searchTerm, int startRow, int maxRows)
        {
            var currentAdmin = new AdminUserService().GetCurrentAdmin();


            IEnumerable<AssessmentViewModel> query = null;


            if (currentAdmin.IsGlobalAdmin)
            {
                
                  query =  Context.Assessments.Where(x => x.OwnerPartnerId.HasValue == false || (x.AllowGTDView.HasValue && x.AllowGTDView.Value)).OrderByDescending(x => x.AssessmentId).Select(
                        x => new AssessmentViewModel
                            {
                                AssessmentId = x.AssessmentId,
                                Name = x.Name,
                                AssessmentType = x.AssessmentType,
                                QuestionCount =
                                    x.AssessmentType == AssessmentType.MCQ
                                        ? x.AssessmentQuestions.Count()
                                        : x.EssayTopics.Count(),
                     //           IsValid = x.AssessmentType == AssessmentType.MCQ ? x.AssessmentQuestions.Any() && !x.AssessmentQuestions.Any(y => y.AssessmentAnswers.Count() < 2 || !y.AssessmentAnswers.Any(t => t.IsCorrect)) : x.EssayTopics.Any()
                            });
            }
            else
            {

                query = Context.AuthorAssignedAssessments.Where(x => x.AdminId == currentAdmin.AdminId && (x.Assessment.OwnerPartnerId.HasValue == false || (x.Assessment.AllowGTDView.HasValue && x.Assessment.AllowGTDView.Value)))
                               .Select(x => new AssessmentViewModel
                                   {
                                       AssessmentId = x.Assessment.AssessmentId,
                                       Name = x.Assessment.Name,
                                       AssessmentType = x.Assessment.AssessmentType,
                                       QuestionCount =
                                           x.Assessment.AssessmentType == AssessmentType.MCQ
                                               ? x.Assessment.AssessmentQuestions.Count()
                                               : x.Assessment.EssayTopics.Count(),
                                    //   IsValid = x.Assessment.AssessmentType == AssessmentType.MCQ ? x.Assessment.AssessmentQuestions.Any() && !x.Assessment.AssessmentQuestions.Any(y => y.AssessmentAnswers.Count() < 2 || !y.AssessmentAnswers.Any(t => t.IsCorrect)) : x.Assessment.EssayTopics.Any()
                                   }).OrderByDescending(x => x.AssessmentId);
            }


            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }


            if (startRow >= 0)
            {
                query = query.Skip(startRow).Take(maxRows);
            }

            return query.ToList();
        }

        public int Count(string searchTerm)
        {
            var currentAdmin = new AdminUserService().GetCurrentAdmin();

            if (currentAdmin.IsGlobalAdmin)
            {
                return string.IsNullOrWhiteSpace(searchTerm) ? Context.Assessments.Count() : Context.Assessments.Count(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }
            else
            {
                return string.IsNullOrWhiteSpace(searchTerm) ? Context.AuthorAssignedAssessments.Count(x => x.AdminId == currentAdmin.AdminId) : Context.AuthorAssignedAssessments.Count(x => x.AdminId == currentAdmin.AdminId && x.Assessment.Name.ToLower().Contains(searchTerm.ToLower()));
            }
        }


        public Assessment GetAssessment(int AssessmentId)
        {
            new AuthorAssignedAssessmentService().CheckAuthorAccess(AssessmentId);

            return Context.Assessments.FirstOrDefault(x => x.AssessmentId == AssessmentId);
        }

        public byte[] GetInstructionImage(int AssessmentId)
        {
            var item = GetAssessment(AssessmentId);

            return item == null ? null : item.InstructionImage;
        }


        public AppMessage Add(Assessment item)
        {
            try
            {
                Context.Assessments.Add(item);
                Context.SaveChanges();

                Directory.CreateDirectory(
                    HttpContext.Current.Server.MapPath(UrlMapper.RootResourcesDirectory + item.AssessmentId));

                var currentAdmin = new AdminUserService().GetCurrentAdmin();

                if (!currentAdmin.IsGlobalAdmin && currentAdmin.CanAuthor)
                {
                    new AuthorAssignedAssessmentService().AssignAssessmentToAuthor(item.AssessmentId,
                                                                                   currentAdmin.AdminId);
                }


                return new AppMessage
                    {
                        IsDone = true,
                        Message = "Added assessment successfully.",
                        Status = MessageStatus.Success,
                        Data = item.AssessmentId
                    };
            }

            catch (Exception ex)
            {
                return new AppMessage
                    {IsDone = false, Message = "An error occured." + ex.Message, Status = MessageStatus.Error};
            }
        }


        public void UpdateQuestionsPerTest(int AssessmentId, int QuestionsPerTest)
        {
            var item = GetAssessment(AssessmentId);

            if (item.AdvancedOutputOptions)
            {
                item.QuestionsPerTest = QuestionsPerTest;
                Update(item);
            }
        }

        public AppMessage Update(Assessment item)
        {
            try
            {
                Context.Entry(item).State = EntityState.Modified;

                Context.SaveChanges();

                return new AppMessage
                    {IsDone = true, Message = "Updated assessment successfully.", Status = MessageStatus.Success};
            }

            catch (Exception)
            {
                return new AppMessage {IsDone = false, Message = "An error occured.", Status = MessageStatus.Error};
            }
        }

        public void Delete(int AssessmentId)
        {
            Assessment item = Context.Assessments.Find(AssessmentId);

            if (item != null)
            {
                Context.Assessments.Remove(item);
                Context.SaveChanges();

                string dirPath = HttpContext.Current.Server.MapPath(UrlMapper.RootResourcesDirectory + AssessmentId);

                if (Directory.Exists(dirPath)) Directory.Delete(dirPath, true);
            }
        }


        public List<AssessmentBundleEntryViewModel> GetAssessmentsInBundle(int BundleId)
        {
            return
                Context.AssessmentBundleEntries.Where(x => x.BundleId == BundleId).Select(
                    x =>
                    new AssessmentBundleEntryViewModel
                        {
                            EntryId = x.EntryId,
                            Name = x.Assessment.Name,
                            AssessmentType = x.Assessment.AssessmentType
                        }).
                        ToList();
        }

        public AppMessage AddAssessmentToBundle(int assessmentId, int BundleId)
        {
            try
            {
                var item = new AssessmentBundleEntry {BundleId = BundleId, AssessmentId = assessmentId};

                Context.AssessmentBundleEntries.Add(item);
                Context.SaveChanges();

                return new AppMessage
                    {
                        IsDone = true,
                        Message = "Added entry successfully.",
                        Status = MessageStatus.Success
                    };
            }

            catch (Exception ex)
            {
                return new AppMessage
                    {
                        IsDone = false,
                        Message = "An error occured." + ex.Message,
                        Status = MessageStatus.Error
                    };
            }
        }

        public void DeleteAssessmentFromBundle(int EntryId)
        {
            AssessmentBundleEntry item = Context.AssessmentBundleEntries.Find(EntryId);

            if (item != null)
            {
                Context.AssessmentBundleEntries.Remove(item);
                Context.SaveChanges();
            }
        }

        public List<AssessmentViewModel> GetAssessmentsNotInBundle(int BundleId)
        {
            return
                Context.Assessments.Where(x => x.AssessmentBundleEntries.Any(y => y.BundleId == BundleId) == false)
                       .Select(x => new AssessmentViewModel
                           {
                               AssessmentId = x.AssessmentId,
                               Name = x.Name
                           }).ToList();
        }


        public List<AssessmentViewModel> GetMCQAssessmentsInBundle(int CampaignId)
        {
            var campaign = new CampaignService().GetCampaign(CampaignId);

            return
                Context.AssessmentBundleEntries.Where(
                    x => x.BundleId == campaign.BundleId && x.Assessment.AssessmentType == AssessmentType.MCQ).Select(
                        x =>
                        new AssessmentViewModel
                            {
                                AssessmentId = x.AssessmentId,
                                Name = x.Assessment.Name,
                            }).
                        ToList();
        }



       
    }
}